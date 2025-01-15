using GameServer.Models.Fields;
using GameServer.Models.Packets;

namespace GameServer.GameLogic.Theme
{
    public class BoardWarhammerTheme : IBoardTheme
    {
        private GameBoard _board;

        public BoardWarhammerTheme(GameBoard board)
        {
            _board = board;
        }

        public void LoadBoardTheme(EventHandler<Packet> OnFieldEventOccurred)
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
                BuildingPrice = 50,
                RentPrices = new int[6] { 2, 10, 30, 90, 160, 250 },
                Housecount = 0,
                Group = _board.Groups["purple"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["purple"].Properties.Add((PropertyField)field);
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
                BuildingPrice = 50,
                RentPrices = new int[6] { 4, 20, 60, 180, 320, 450 },
                Housecount = 0,
                Group = _board.Groups["purple"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["purple"].Properties.Add((PropertyField)field);
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
                BuyingPrice = 100,
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["station"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Altdorf Outskirts",
                BuyingPrice = 100,
                BuildingPrice = 50,
                RentPrices = new int[6] { 6, 30, 90, 270, 400, 550 },
                Housecount = 0,
                Group = _board.Groups["white"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["white"].Properties.Add((PropertyField)field);
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
                BuildingPrice = 50,
                RentPrices = new int[6] { 6, 30, 90, 270, 400, 550 },
                Housecount = 0,
                Group = _board.Groups["white"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["white"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Reikland Hills",
                BuyingPrice = 120,
                BuildingPrice = 50,
                RentPrices = new int[6] { 8, 40, 100, 300, 450, 600 },
                Housecount = 0,
                Group = _board.Groups["white"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["white"].Properties.Add((PropertyField)field);
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
                BuildingPrice = 100,
                RentPrices = new int[6] { 10, 50, 150, 450, 625, 750 },
                Housecount = 0,
                Group = _board.Groups["pink"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["pink"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Utility()
            {
                Name = "Dwarfen Jewerly Forges",
                BuyingPrice = 100,
                Group = _board.Groups["utility"],
                RolledDice = 0
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["utility"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Nuln Foundry",
                BuyingPrice = 140,
                BuildingPrice = 100,
                RentPrices = new int[6] { 10, 50, 150, 450, 625, 750 },
                Housecount = 0,
                Group = _board.Groups["pink"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["pink"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Averheim Market",
                BuyingPrice = 160,
                BuildingPrice = 100,
                RentPrices = new int[6] { 12, 60, 180, 500, 700, 900 },
                Housecount = 0,
                Group = _board.Groups["pink"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["pink"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Station()
            {
                Name = "Kislev Train",
                BuyingPrice = 100,
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["station"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Marienburg Wharf",
                BuyingPrice = 180,
                BuildingPrice = 100,
                RentPrices = new int[6] { 14, 70, 200, 550, 750, 950 },
                Housecount = 0,
                Group = _board.Groups["orange"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["orange"].Properties.Add((PropertyField)field);
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
                BuildingPrice = 100,
                RentPrices = new int[6] { 14, 70, 200, 550, 750, 950 },
                Housecount = 0,
                Group = _board.Groups["orange"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["orange"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Talabheim Road",
                BuyingPrice = 200,
                BuildingPrice = 100,
                RentPrices = new int[6] { 16, 80, 220, 600, 800, 1000 },
                Housecount = 0,
                Group = _board.Groups["orange"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["orange"].Properties.Add((PropertyField)field);
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
                BuildingPrice = 150,
                RentPrices = new int[6] { 18, 90, 250, 700, 875, 1050 },
                Housecount = 0,
                Group = _board.Groups["red"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["red"].Properties.Add((PropertyField)field);
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
                BuildingPrice = 150,
                RentPrices = new int[6] { 18, 90, 250, 700, 875, 1050 },
                Housecount = 0,
                Group = _board.Groups["red"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["red"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Drakenhof Castle",
                BuyingPrice = 240,
                BuildingPrice = 150,
                RentPrices = new int[6] { 20, 100, 300, 750, 925, 1100 },
                Housecount = 0,
                Group = _board.Groups["red"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["red"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Station()
            {
                Name = "Northern Warmammoths",
                BuyingPrice = 100,
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["station"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Morrslieb's Rise",
                BuyingPrice = 260,
                BuildingPrice = 150,
                RentPrices = new int[6] { 22, 110, 330, 800, 975, 1150 },
                Housecount = 0,
                Group = _board.Groups["yellow"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["yellow"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Grimhold Mines",
                BuyingPrice = 260,
                BuildingPrice = 150,
                RentPrices = new int[6] { 22, 110, 330, 800, 975, 1150 },
                Housecount = 0,
                Group = _board.Groups["yellow"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["yellow"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Utility()
            {
                Name = "Imperial Brewery",
                BuyingPrice = 100,
                Group = _board.Groups["utility"],
                RolledDice = 0
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["utility"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Ekrund Hold",
                BuyingPrice = 280,
                BuildingPrice = 150,
                RentPrices = new int[6] { 24, 120, 360, 850, 1025, 1200 },
                Housecount = 0,
                Group = _board.Groups["yellow"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["yellow"].Properties.Add((PropertyField)field);
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
                BuildingPrice = 200,
                RentPrices = new int[6] { 26, 130, 390, 900, 1100, 1275 },
                Housecount = 0,
                Group = _board.Groups["green"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["green"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Site()
            {
                Name = "Karak Eight Peaks",
                BuyingPrice = 300,
                BuildingPrice = 200,
                RentPrices = new int[6] { 26, 130, 390, 900, 1100, 1275 },
                Housecount = 0,
                Group = _board.Groups["green"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["green"].Properties.Add((PropertyField)field);
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
                BuildingPrice = 200,
                RentPrices = new int[6] { 28, 150, 450, 1000, 1200, 1400 },
                Housecount = 0,
                Group = _board.Groups["green"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["green"].Properties.Add((PropertyField)field);
            _board.AddField(field);

            field = new Station()
            {
                Name = "Dwarven Tunnels",
                BuyingPrice = 100,
                RentPrices = new int[4] { 25, 50, 100, 200 },
                Group = _board.Groups["station"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["station"].Properties.Add((PropertyField)field);
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
                BuildingPrice = 200,
                RentPrices = new int[6] { 35, 175, 500, 1100, 1300, 1500 },
                Housecount = 0,
                Group = _board.Groups["blue"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["blue"].Properties.Add((PropertyField)field);
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
                BuildingPrice = 200,
                RentPrices = new int[6] { 50, 200, 600, 1400, 1700, 2000 },
                Housecount = 0,
                Group = _board.Groups["blue"]
            };
            field.FieldEventOccurred += OnFieldEventOccurred;
            _board.Groups["blue"].Properties.Add((PropertyField)field);
            _board.AddField(field);
        }
    }
}
