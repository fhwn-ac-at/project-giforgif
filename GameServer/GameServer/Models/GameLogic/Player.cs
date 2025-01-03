namespace GameServer.Models
{
    public class Player
    {
        // das ist noch nicht der richtige content
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public int Currency { get; set; }
        public IField CurrentPosition { get; set; }

        public Player(string name, string connectionId)
        {
            this.Name = name;
            this.ConnectionId = connectionId;
        }

        public bool TransferCurrency(Player recipient, int amount)
        {
            if (this.Currency < amount)
            {
                return false;
            }

            this.Currency -= amount;
            recipient.Currency += amount;

            return true;
        }
    }
}
