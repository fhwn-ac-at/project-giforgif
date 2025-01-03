namespace GameServer.Models
{
    public class Game
    {
        public Player? CurrentMover;
        public List<Player> Players { get; set; } = [];
        public bool Started { get; set; }
        private static Random rng = new Random();
        private GameBoard? _board;

        public Action<string, Player> Callback { get; set; }

        public void Setup()
        {
            if (this.Players.Count < 2)
            {
                throw new InvalidOperationException("Not enough players registered to start the game");
            }

            this.RandomizePlayerOrder();

            this.CurrentMover = this.Players.First();

            foreach (Player player in this.Players)
            {
                player.Currency = 1500;
            }

            _board = new GameBoard();

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
        }

        private void OnFieldEventOccurred(object? sender, FieldEventArgs e)
        {
            switch (e.MessageType)
            {
                case "BUY":
                    this.Callback("BUY", e.Data as Player);
                    break;
                case "PAYMENT":
                    this.Callback("PAYMENT", e.Data as Player);
                    break;
                case "INFO":
                    this.Callback("INFO", e.Data as Player);
                    break;
                default:
                    break;
            }
        }

        private void RandomizePlayerOrder()
        {
            Players = Players.OrderBy(a => rng.Next()).ToList();
        }
        // TODO ganzer game state mit movement und dies und das


        public int RollDice()
        {
			Random random = new Random();
			int rolled = random.Next(1, 7) + random.Next(1, 7);

            return rolled;
		}

		public void MovePlayer(Player player, int rolled)
		{
			if (_board == null)
			{
				throw new ArgumentNullException("The instance of the gameBoard must not be null");
			}

			IField newPosition = _board.Move(player, rolled);
            newPosition.LandOn(player);
		}
	}
}
