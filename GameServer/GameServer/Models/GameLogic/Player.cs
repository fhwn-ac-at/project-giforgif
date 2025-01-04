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

        public bool BuyCurrentField()
        {
            if (this.CurrentPosition != typeof(PropertyField))
            {
                return false;
            }

            PropertyField field = (PropertyField)this.CurrentPosition;

            if (this.Currency < field.BuyingPrice)
            {
                return false;
            }

            this.Currency -= field.BuyingPrice;
            field.Owner = this;

            return true;
        }

        public bool BuyField(PropertyField field, int price)
        {
            if (this.Currency < price)
            {
                return false;
            }

            this.Currency -= price;
            field.Owner = this;

            return true;
        }
    }
}
