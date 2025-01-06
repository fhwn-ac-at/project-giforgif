using GameServer.Models;
using GameServer.Models.Fields;
using GameServer.Models.Packets;
using System.Net.Sockets;
using System.Xml.Linq;

namespace GameServer.GameLogic
{
    public class Game
    {
        private static readonly Random Rng = new();
        private GameBoard? _board;
        private Thread? _startGameCounter;
        private int _counterId = 0;
        private IFieldVisitor? _fieldVisitor;
        private AuctionState? _currentAuction;

        public event EventHandler<Game>? OnGameStarted;
        public bool Started { get; set; }
        private bool _counterStarted = false;
        public Player? CurrentMover;
		public event EventHandler<Packet>? FieldEventOccurred;
		public List<Player> Players { get; set; } = [];

        public void Setup()
        {
            _fieldVisitor = new DefaultThemeVisitor(this);

            if (Players.Count < 2)
            {
                throw new InvalidOperationException("Not enough players registered to start the game");
            }

            RandomizePlayerOrder();

            CurrentMover = Players.First();
			_board = new GameBoard();

			foreach (Player player in Players)
            {
                player.Currency = 1500;
                player.Board = _board;
            }

            // fill all of the fields and cards 
            var field = new Site()
            {
                Name = "GO",
                BuyingPrice = 100,
                RentPrices = new int[6] { 100, 200, 300, 400, 500, 600 },
                Housecount = 0,
                Group = null
            };

            field.FieldEventOccurred += OnFieldEventOccurred;

            _board.AddField(field);

            // _board.AddCard(new Card() { Name = "Get Out Of Jail Free!" }); Load the cards somehow
        }

        public void StartCounter()
        {
            if (_counterStarted)
            {
                return;
            }
            
            _counterStarted = true;
            _startGameCounter = new Thread(() =>
            {
                int myId = _counterId;
                Thread.Sleep(10 * 1000);

                if (_counterId != myId)
                {
                    return;
                }

                Started = true;
                OnGameStarted?.Invoke(this, this);
            });

            _startGameCounter.Start();
        }

        public void StopCounter()
        {
            if (!_counterStarted)
            {
                return;
            }

            _counterId++;
            _counterStarted = false;
        }

        private void OnFieldEventOccurred(object? sender, Packet e)
        {
			FieldEventOccurred?.Invoke(this, e);
		}

        private void RandomizePlayerOrder()
        {
            Players = Players.OrderBy(a => Rng.Next()).ToList();
        }

        public int RollDice()
        {
			return Rng.Next(1, 7) + Rng.Next(1, 7);
		}

        public void MovePlayer(Player player, int rolled)
        {
            MovePlayerInternal(player, rolled);
        }

        public void SetPlayerPosition(Player player, int fieldId)
        {
            MovePlayerInternal(player, fieldId, isDirectMove: true);
        }

        public void StartAuction(int fieldId)
		{
			_currentAuction = new AuctionState(fieldId);
			_currentAuction.AuctionEnded += _currentAuction_AuctionEnded;
            _currentAuction.StartTimer();
		}

		private void _currentAuction_AuctionEnded(AuctionState auctionState)
		{
            Player winner = auctionState.HighestBidder;

            if (winner == null)
            {
                // If the auction failed, because noone has bid
                OnFieldEventOccurred(this, new AuctionResultPacket() { WinnerPlayerName = null, WinningBid = 0 });
            }
            else 
            {
                // if the auction was successfull 
                PropertyField? field = _board.GetFieldById(auctionState.FieldId) as PropertyField;

                if (field == null)
                    return;

                winner.BuyField(field, auctionState.HighestBid);

                OnFieldEventOccurred(this, new AuctionResultPacket() { WinnerPlayerName = winner.Name, WinningBid = auctionState.HighestBid }); 
            }

            _currentAuction = null;
		}

        private void MovePlayerInternal(Player player, int stepsOrFieldId, bool isDirectMove = false)
        {
            if (_board == null)
            {
                throw new ArgumentNullException("The instance of the gameBoard must not be null");
            }

            if (_fieldVisitor == null)
            {
                throw new ArgumentNullException("The instance of the field Visitor must not be null");
            }

            // Get current position
            int currentPositionIndex = player.CurrentPositionFieldId;

            if (currentPositionIndex == -1)
            {
                throw new InvalidOperationException("Player's current position is not on the game board.");
            }

            int totalFields = _board.GetFieldCount();
            int newPositionIndex;

            if (isDirectMove)
            {
                // Move directly to the specified field ID
                newPositionIndex = stepsOrFieldId;
            }
            else
            {
                // Calculate new position based on dice roll
                newPositionIndex = (currentPositionIndex + stepsOrFieldId) % totalFields;

                for (int i = 1; i <= stepsOrFieldId; i++)
                {
                    int passPositionIndex = (currentPositionIndex + i) % totalFields;
                    _board.GetFieldById(passPositionIndex).Accept(_fieldVisitor, player, false);
                }
            }

            // Update player's position
            player.CurrentPositionFieldId = _board.GetFieldById(newPositionIndex).Id;

            // Handle landing event
            IField newPosition = _board.GetFieldById(newPositionIndex);

            if (newPosition.GetType() == typeof(Utility))
            {
                Utility utility = (Utility)newPosition;
                if (!isDirectMove) // Only set rolled dice for non-direct moves
                {
                    utility.RolledDice = stepsOrFieldId;
                }
                utility.Accept(_fieldVisitor, player, true);
                return;
            }

            newPosition.Accept(_fieldVisitor, player, true);
        }

        public bool HandleAuctionBid(Player player, int amount)
		{
            if (_currentAuction == null)
                return false;

            if (!_currentAuction.PlaceBid(player, amount))
            {
                return false;
            }

            return true;
		}
	}
}
