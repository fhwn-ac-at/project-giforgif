namespace GameServer.Models
{
    public class Game
    {
        public Player? CurrentMover;
        public List<Player> Players { get; set; } = [];
        public bool Started { get; set; }
        private static Random rng = new Random();
        private GameBoard? _board;

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

			_board.Move(player, rolled);
		}
	}
}
