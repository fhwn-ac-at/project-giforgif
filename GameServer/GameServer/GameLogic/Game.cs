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

            FillBoardWithWarhammerTheme();

            for (int i=1; i < _board.GetFieldCount(); i++)
            {
                if (_board.GetFieldById(i) is PropertyField propertyField)
                {
                    propertyField.Owner = Players[0];
                }
            }

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

            IField field = new Go() { Name = "GO" };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Drakwald Forest",
                BuyingPrice = 60,
                RentPrices = new int[6] { 2, 10, 30, 90, 160, 250 },
                Housecount = 0,
                Group = _board.Groups["purple"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new CommunityChest()
            {
                Name = "Community Chest"
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Hel Fenn",
                BuyingPrice = 60,
                RentPrices = new int[6] { 4, 20, 60, 180, 320, 450 },
                Housecount = 0,
                Group = _board.Groups["purple"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Tax()
            {
                Name = "Income Tax",
                Amount = 200
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Station()
            {
                Name = "Altdorf Train",
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Altdorf Outskirts",
                BuyingPrice = 100,
                RentPrices = new int[6] { 6, 30, 90, 270, 400, 550 },
                Housecount = 0,
                Group = _board.Groups["white"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Chance()
            {
                Name = "Chance"
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Carroburg Docks",
                BuyingPrice = 100,
                RentPrices = new int[6] { 6, 30, 90, 270, 400, 550 },
                Housecount = 0,
                Group = _board.Groups["white"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Reikland Hills",
                BuyingPrice = 120,
                RentPrices = new int[6] { 8, 40, 100, 300, 450, 600 },
                Housecount = 0,
                Group = _board.Groups["white"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Jail()
            {
                Name = "Jail"
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Middenheim Gate",
                BuyingPrice = 140,
                RentPrices = new int[6] { 10, 50, 150, 450, 625, 750 },
                Housecount = 0,
                Group = _board.Groups["pink"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Utility()
            {
                Name = "Dwarfen Jewerly Forges",
                Group = _board.Groups["utility"],
                RolledDice = 0
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Nuln Foundry",
                BuyingPrice = 140,
                RentPrices = new int[6] { 10, 50, 150, 450, 625, 750 },
                Housecount = 0,
                Group = _board.Groups["pink"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Averheim Market",
                BuyingPrice = 160,
                RentPrices = new int[6] { 12, 60, 180, 500, 700, 900 },
                Housecount = 0,
                Group = _board.Groups["pink"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Station()
            {
                Name = "Kislev Train",
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Marienburg Wharf",
                BuyingPrice = 180,
                RentPrices = new int[6] { 14, 70, 200, 550, 750, 950 },
                Housecount = 0,
                Group = _board.Groups["orange"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new CommunityChest()
            {
                Name = "Community Chest"
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Blackfire Pass",
                BuyingPrice = 180,
                RentPrices = new int[6] { 14, 70, 200, 550, 750, 950 },
                Housecount = 0,
                Group = _board.Groups["orange"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Talabheim Road",
                BuyingPrice = 200,
                RentPrices = new int[6] { 16, 80, 220, 600, 800, 1000 },
                Housecount = 0,
                Group = _board.Groups["orange"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new FreeParking()
            {
                Name = "Free Parking"
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Sylvania Crypts",
                BuyingPrice = 220,
                RentPrices = new int[6] { 18, 90, 250, 700, 875, 1050 },
                Housecount = 0,
                Group = _board.Groups["red"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Chance()
            {
                Name = "Chance"
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Templehof Ruins",
                BuyingPrice = 220,
                RentPrices = new int[6] { 18, 90, 250, 700, 875, 1050 },
                Housecount = 0,
                Group = _board.Groups["red"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Drakenhof Castle",
                BuyingPrice = 240,
                RentPrices = new int[6] { 20, 100, 300, 750, 925, 1100 },
                Housecount = 0,
                Group = _board.Groups["red"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Station()
            {
                Name = "Northern Warmammoths",
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Morrslieb's Rise",
                BuyingPrice = 260,
                RentPrices = new int[6] { 22, 110, 330, 800, 975, 1150 },
                Housecount = 0,
                Group = _board.Groups["yellow"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Grimhold Mines",
                BuyingPrice = 260,
                RentPrices = new int[6] { 22, 110, 330, 800, 975, 1150 },
                Housecount = 0,
                Group = _board.Groups["yellow"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Utility()
            {
                Name = "Imperial Brewery",
                Group = _board.Groups["utility"],
                RolledDice = 0
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Ekrund Hold",
                BuyingPrice = 280,
                RentPrices = new int[6] { 24, 120, 360, 850, 1025, 1200 },
                Housecount = 0,
                Group = _board.Groups["yellow"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new GoToJail()
            {
                Name = "Go To Jail"
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Karak Kadrin",
                BuyingPrice = 300,
                RentPrices = new int[6] { 26, 130, 390, 900, 1100, 1275 },
                Housecount = 0,
                Group = _board.Groups["green"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Karak Eight Peaks",
                BuyingPrice = 300,
                RentPrices = new int[6] { 26, 130, 390, 900, 1100, 1275 },
                Housecount = 0,
                Group = _board.Groups["green"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new CommunityChest()
            {
                Name = "Community Chest"
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Zharr Naggrund",
                BuyingPrice = 320,
                RentPrices = new int[6] { 28, 150, 450, 1000, 1200, 1400 },
                Housecount = 0,
                Group = _board.Groups["green"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Station()
            {
                Name = "Dwarven Tunnels",
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Chance()
            {
                Name = "Chance"
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Naggarond",
                BuyingPrice = 350,
                RentPrices = new int[6] { 35, 175, 500, 1100, 1300, 1500 },
                Housecount = 0,
                Group = _board.Groups["blue"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Tax()
            {
                Name = "Luxury Tax",
                Amount = 75
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);

            field = new Site()
            {
                Name = "Ulthuan",
                BuyingPrice = 400,
                RentPrices = new int[6] { 50, 200, 600, 1400, 1700, 2000 },
                Housecount = 0,
                Group = _board.Groups["blue"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.AddField(field);
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

            int nextPlayerIdx = (currentIndex + 1) % Players.Count;

            CurrentMover = Players[nextPlayerIdx];

            CurrentMoverRolled = false;
            return CurrentMover;
        }

        public int RollDice()
        {
            return 11;
			return Rng.Next(1, 7) + Rng.Next(1, 7);
		}

        public void MovePlayer(Player player, int rolled)
        {
            CurrentMoverRolled = true;
            MovePlayerInternal(player, rolled);
        }

        public void SetPlayerPosition(Player player, int FieldId)
        {
            MovePlayerInternal(player, FieldId, isDirectMove: true);
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
