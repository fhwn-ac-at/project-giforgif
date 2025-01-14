namespace GameServer.GameLogic.EventArgs
{
    public class GameOverEventArgs
    {
        public string WinnerName { get; set; }
        public List<string> Players { get; set; }
    }
}
