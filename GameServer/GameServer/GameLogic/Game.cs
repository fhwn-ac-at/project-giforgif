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
        private bool _counterStarted = false;

        public event EventHandler<Game>? OnGameStarted;
        public bool Started { get; set; }
        public Player? CurrentMover;
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

            FillBoardWithWarhammerTheme();

            // _board.AddCard(new Card() { Name = "Get Out Of Jail Free!" }); Load the cards somehow
        }

        private void FillBoardWithWarhammerTheme()
        {
            if (_board == null)
            {
                throw new ArgumentNullException("The instance of the gameBoard must not be null");
            }

            _board.Groups.Add("purple", new PropertyGroup());
            _board.Groups.Add("white", new PropertyGroup());
            _board.Groups.Add("pink", new PropertyGroup());
            _board.Groups.Add("orange", new PropertyGroup());
            _board.Groups.Add("red", new PropertyGroup());
            _board.Groups.Add("yellow", new PropertyGroup());
            _board.Groups.Add("green", new PropertyGroup());
            _board.Groups.Add("blue", new PropertyGroup());
            _board.Groups.Add("utility", new PropertyGroup());
            _board.Groups.Add("station", new PropertyGroup());

            _board.AddField(new Go() { Name = "GO" });

            _board.AddField(new Site()
            {
                Name = "Drakwald Forest",
                BuyingPrice = 60,
                RentPrices = new int[6] { 2, 10, 30, 90, 160, 250 },
                Housecount = 0,
                Group = _board.Groups["purple"]
            });

            _board.AddField(new CommunityChest()
            { 
                Name = "Community Chest"
            });

            _board.AddField(new Site()
            {
                Name = "Hel Fenn",
                BuyingPrice = 60,
                RentPrices = new int[6] { 4, 20, 60, 180, 320, 450 },
                Housecount = 0,
                Group = _board.Groups["purple"]
            });

            _board.AddField(new Tax()
            {
                Name = "Income Tax",
                Amount = 200 // Theoretically should be 200 or 10% of the player's total worth
            });

            _board.AddField(new Station()
            {
                Name = "Altdorf Train",
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            });

            _board.AddField(new Site()
            {
                Name = "Altdorf Outskirts",
                BuyingPrice = 100,
                RentPrices = new int[6] { 6, 30, 90, 270, 400, 550 },
                Housecount = 0,
                Group = _board.Groups["white"]
            });

            _board.AddField(new Chance()
            {
                Name = "Chance"
            });

            _board.AddField(new Site()
            {
                Name = "Carroburg Docks",
                BuyingPrice = 100,
                RentPrices = new int[6] { 6, 30, 90, 270, 400, 550 },
                Housecount = 0,
                Group = _board.Groups["white"]
            });

            _board.AddField(new Site()
            {
                Name = "Reikland Hills",
                BuyingPrice = 120,
                RentPrices = new int[6] { 8, 40, 100, 300, 450, 600 },
                Housecount = 0,
                Group = _board.Groups["white"]
            });

            _board.AddField(new Jail()
            {
                Name = "Jail"
            });

            _board.AddField(new Site()
            {
                Name = "Middenheim Gate",
                BuyingPrice = 140,
                RentPrices = new int[6] { 10, 50, 150, 450, 625, 750 },
                Housecount = 0,
                Group = _board.Groups["pink"]
            });

            _board.AddField(new Utility()
            {
                Name = "Dwarfen Jewerly Forges",
                Group = _board.Groups["utility"],
                RolledDice = 0 // We need another way of rolling dice here, cant be set at construction time
            });

            _board.AddField(new Site()
            {
                Name = "Nuln Foundry",
                BuyingPrice = 140,
                RentPrices = new int[6] { 10, 50, 150, 450, 625, 750 },
                Housecount = 0,
                Group = _board.Groups["pink"]
            });

            _board.AddField(new Site()
            {
                Name = "Averheim Market",
                BuyingPrice = 160,
                RentPrices = new int[6] { 12, 60, 180, 500, 700, 900 },
                Housecount = 0,
                Group = _board.Groups["pink"]
            });

            _board.AddField(new Station()
            {
                Name = "Kislev Train",
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            });

            _board.AddField(new Site()
            {
                Name = "Marienburg Wharf",
                BuyingPrice = 180,
                RentPrices = new int[6] { 14, 70, 200, 550, 750, 950 },
                Housecount = 0,
                Group = _board.Groups["orange"]
            });

            _board.AddField(new CommunityChest()
            {
                Name = "Community Chest"
            });

            _board.AddField(new Site()
            {
                Name = "Blackfire Pass",
                BuyingPrice = 180,
                RentPrices = new int[6] { 14, 70, 200, 550, 750, 950 },
                Housecount = 0,
                Group = _board.Groups["orange"]
            });

            _board.AddField(new Site()
            {
                Name = "Talabheim Road",
                BuyingPrice = 200,
                RentPrices = new int[6] { 16, 80, 220, 600, 800, 1000 },
                Housecount = 0,
                Group = _board.Groups["orange"]
            });

            _board.AddField(new FreeParking()
            {
                Name = "Free Parking"
            });

            _board.AddField(new Site()
            {
                Name = "Sylvania Crypts",
                BuyingPrice = 220,
                RentPrices = new int[6] { 18, 90, 250, 700, 875, 1050 },
                Housecount = 0,
                Group = _board.Groups["red"]
            });

            _board.AddField(new Chance()
            {
                Name = "Chance"
            });

            _board.AddField(new Site()
            {
                Name = "Templehof Ruins",
                BuyingPrice = 220,
                RentPrices = new int[6] { 18, 90, 250, 700, 875, 1050 },
                Housecount = 0,
                Group = _board.Groups["red"]
            });

            _board.AddField(new Site()
            {
                Name = "Drakenhof Castle",
                BuyingPrice = 240,
                RentPrices = new int[6] { 20, 100, 300, 750, 925, 1100 },
                Housecount = 0,
                Group = _board.Groups["red"]
            });

            _board.AddField(new Station()
            {
                Name = "Northern Warmammoths",
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            });

            _board.AddField(new Site()
            {
                Name = "Morrslieb's Rise",
                BuyingPrice = 260,
                RentPrices = new int[6] { 22, 110, 330, 800, 975, 1150 },
                Housecount = 0,
                Group = _board.Groups["yellow"]
            });

            _board.AddField(new Site()
            {
                Name = "Grimhold Mines",
                BuyingPrice = 260,
                RentPrices = new int[6] { 22, 110, 330, 800, 975, 1150 },
                Housecount = 0,
                Group = _board.Groups["yellow"]
            });

            _board.AddField(new Utility()
            {
                Name = "Imperial Brewery",
                Group = _board.Groups["utility"],
                RolledDice = 0 // We need another way of rolling dice here, cant be set at construction time
            });

            _board.AddField(new Site()
            {
                Name = "Ekrund Hold",
                BuyingPrice = 280,
                RentPrices = new int[6] { 24, 120, 360, 850, 1025, 1200 },
                Housecount = 0,
                Group = _board.Groups["yellow"]
            });

            _board.AddField(new GoToJail()
            {
                Name = "Go To Jail"
            });

            _board.AddField(new Site()
            {
                Name = "Karak Kadrin",
                BuyingPrice = 300,
                RentPrices = new int[6] { 26, 130, 390, 900, 1100, 1275 },
                Housecount = 0,
                Group = _board.Groups["green"]
            });

            _board.AddField(new Site()
            {
                Name = "Karak Eight Peaks",
                BuyingPrice = 300,
                RentPrices = new int[6] { 26, 130, 390, 900, 1100, 1275 },
                Housecount = 0,
                Group = _board.Groups["green"]
            });

            _board.AddField(new CommunityChest()
            {
                Name = "Community Chest"
            });

            _board.AddField(new Site()
            {
                Name = "Zharr Naggrund",
                BuyingPrice = 320,
                RentPrices = new int[6] { 28, 150, 450, 1000, 1200, 1400 },
                Housecount = 0,
                Group = _board.Groups["green"]
            });

            _board.AddField(new Station()
            {
                Name = "Dwarven Tunnels",
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            });

            _board.AddField(new Chance()
            {
                Name = "Chance"
            });

            _board.AddField(new Site()
            {
                Name = "Naggarond",
                BuyingPrice = 350,
                RentPrices = new int[6] { 35, 175, 500, 1100, 1300, 1500 },
                Housecount = 0,
                Group = _board.Groups["blue"]
            });

            _board.AddField(new Tax()
            {
                Name = "Luxury Tax",
                Amount = 75
            });

            _board.AddField(new Site()
            {
                Name = "Ulthuan",
                BuyingPrice = 400,
                RentPrices = new int[6] { 50, 200, 600, 1400, 1700, 2000 },
                Housecount = 0,
                Group = _board.Groups["blue"]
            });
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

            int nextPlayerIdx = (currentIndex + 1) % Players.Count;

            CurrentMover = Players[nextPlayerIdx];

            return CurrentMover;
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
