namespace GameServer.Models
{
    public class Game
    {
        public Player? CurrentMover;
        public List<Player> Players { get; set; } = [];
        public bool Started { get; set; }

        // TODO ganzer game state mit movement und dies und das


        public int RollDice()
        {
			Random random = new Random();
			int rolled = random.Next(1, 7) + random.Next(1, 7);

            return rolled;
		}
    }
}
