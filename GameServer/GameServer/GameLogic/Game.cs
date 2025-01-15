using GameServer.GameLogic.Politics;
using GameServer.GameLogic.Theme;
using GameServer.Models;
using GameServer.Models.Fields;
using GameServer.Models.Packets;
using GameServer.Models.Packets.Game.Outgoing;
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
        private bool _counterStarted = false;
        private Politic? _politic;

        public event EventHandler<Game>? OnGameStarted;
        public bool Started { get; set; }
        public Player? CurrentMover;
        public bool CurrentMoverRolled = false;
        public event EventHandler<Packet>? FieldEventOccurred;
		public List<Player> Players { get; set; } = [];
        public int ReadyPlayers { get; set; } = 0;

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

            List<string> colors = new List<string>() { "red", "blue", "green", "yellow"};

            if (Players.Count > colors.Count)
                throw new InvalidOperationException("The number of players must be equal to the number of colors");

            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].Color = colors[i];
                Players[i].Currency = 1500;
                Players[i].Board = _board;
            }

            IBoardTheme boardTheme = new BoardWarhammerTheme(_board);

            boardTheme.LoadBoardTheme(OnFieldEventOccurred);

            ICardTheme cardTheme = new CardsWarhammerTheme(_board.Dealer);

            cardTheme.LoadChanceCards();
            cardTheme.LoadCommunityCards();

            _politic = new Politic(_board, Players, OnFieldEventOccurred);
        }

        public Player GetAuctionHighestBidder()
        {
            if (_currentAuction == null)
                throw new InvalidOperationException("There is no auction running");

            return _currentAuction.HighestBidder;
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

        public Player GetNextPlayer()
        {
            int currentIndex = Players.IndexOf(CurrentMover);

            if (currentIndex == -1)
            {
                throw new InvalidOperationException("Current player is not in the list of players.");
            }

            if (currentIndex + 1 >= Players.Count)
            {
                //One whole round has been completed
                _politic?.TurnEnd();
            }

            int nextPlayerIdx;

            do
            {
                nextPlayerIdx = (currentIndex + 1) % Players.Count;
            } while (Players[nextPlayerIdx].IsBankrupt);

            CurrentMover = Players[nextPlayerIdx];

            CurrentMoverRolled = false;
            return CurrentMover;
        }

        public int RollDice()
        {
			return Rng.Next(1, 7) + Rng.Next(1, 7);
		}

        public void MovePlayer(Player player, int rolled)
        {
            CurrentMoverRolled = true;
            MovePlayerInternal(player, player.CurrentPositionFieldId + rolled, false);
        }

        public void SetPlayerPosition(Player player, int FieldId, bool isDirectMove)
        {
            MovePlayerInternal(player, FieldId, isDirectMove);
        }

        public void StartAuction(int FieldId)
		{
			_currentAuction = new AuctionState(FieldId);
			_currentAuction.AuctionEnded += _currentAuction_AuctionEnded;
            _currentAuction.StartTimer();
		}

		private void _currentAuction_AuctionEnded(AuctionState auctionState)
		{
            Player winner = auctionState.HighestBidder;

            if (winner == null)
            {
                // If the auction failed, because noone has bid
                OnFieldEventOccurred(this, new AuctionResultPacket() { WinnerPlayerName = null, WinningBid = 0, PropertyId = auctionState.FieldId });
            }
            else 
            {
                // if the auction was successfull 
                PropertyField? field = _board.GetFieldById(auctionState.FieldId) as PropertyField;

                if (field == null)
                    return;

                winner.BuyField(field, auctionState.HighestBid);

                OnFieldEventOccurred(this, new AuctionResultPacket() { WinnerPlayerName = winner.Name, WinningBid = auctionState.HighestBid, PropertyId = auctionState.FieldId }); 
            }

            _currentAuction = null;
		}

        private void MovePlayerInternal(Player player, int FieldId, bool isDirectMove)
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
            int newPositionIndex = FieldId;

            if (newPositionIndex > totalFields)
            {
                newPositionIndex -= totalFields;
            }

            if (!isDirectMove)
            {
                // Move is not direct, so calculate all passed fields

                // Calculate new position based on dice roll

                if (newPositionIndex > currentPositionIndex)
                {
                    for (int i = currentPositionIndex + 1; i <= newPositionIndex; i++)
                    {
                        _board.GetFieldById(i).Accept(_fieldVisitor, player, false);
                    }
                }
                else
                {
                    for (int i = currentPositionIndex + 1; i <= totalFields; i++)
                    {
                        _board.GetFieldById(i).Accept(_fieldVisitor, player, false);
                    }

                    for (int i = 1; i < newPositionIndex; i++)
                    {
                        _board.GetFieldById(i).Accept(_fieldVisitor, player, false);
                    }
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
                    utility.RolledDice = FieldId;
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

        public string CheckForWinner()
        {
            Console.WriteLine($"GAME CHECKS FOR WINNERS: {Players.Where(p => !p.IsBankrupt).Count()} non bankrupt players found.");

            if (Players.Where(p => !p.IsBankrupt).Count() == 1)
            {
                Player winner = Players.Where(p => !p.IsBankrupt).First();
                OnFieldEventOccurred(this, new PlayerWonPacket() { PlayerName = winner.Name });

                return winner.Name;
            }

            return string.Empty;
        }
    }
}
