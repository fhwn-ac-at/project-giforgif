namespace GameServer.Models
{
    public class Game
    {
        public Player CurrentMover;
        public List<Player> Players { get; set; } = new List<Player>();

        // TODO ganzer game state mit movement und dies und das
    }
}
