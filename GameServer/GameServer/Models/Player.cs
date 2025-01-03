namespace GameServer.Models
{
    public class Player
    {
        // das ist noch nicht der richtige content
        public string ConnectionId { get; set; }
        public string Name { get; set; }

        public Player(string name, string connectionId)
        {
            this.Name = name;
            this.ConnectionId = connectionId;
        }
    }
}
