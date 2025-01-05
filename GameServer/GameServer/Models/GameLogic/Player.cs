using GameServer.Models.GameLogic;

namespace GameServer.Models
{
	public class Player
	{
		// das ist noch nicht der richtige content
		public string ConnectionId { get; set; }
		public string Name { get; set; }
		public int Currency { get; set; }
		public IField CurrentPosition { get; set; }

		public GameBoard Board;
		public List<Card> Cards { get; set; } = new();

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

		//public bool BuyCurrentField()
		//{
		//    if (this.CurrentPosition != typeof(PropertyField))
		//    {
		//        return false;
		//    }

		//    PropertyField field = (PropertyField)this.CurrentPosition;

		//    if (this.Currency < field.BuyingPrice)
		//    {
		//        return false;
		//    }

		//    this.Currency -= field.BuyingPrice;
		//    field.Owner = this;

		//    return true;
		//}

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

		public void DeclareBankruptcyToPlayer(Player recipient)
		{
			recipient.Currency += Currency;
			Currency = 0;

			var properties = Board.GetPropertyFieldsOf(this);

			if (properties == null)
				return;

			foreach (PropertyField property in properties)
			{
				if (property is Site site)
				{
					int sellvalue = site.Housecount * (site.BuyingPrice / 2);
					recipient.Currency += sellvalue;
					site.Housecount = 0;
				}

				property.Owner = recipient;
			}

			recipient.Cards.AddRange(Cards);
			Cards.Clear();
		}

		public void DeclareBankruptcyToBank()
		{
			Currency = 0;

			var properties = Board.GetPropertyFieldsOf(this);

			if (properties == null)
				return;

			foreach (PropertyField property in properties)
			{
				if (property is Site site)
				{
					site.Housecount = 0;
				}

				property.Owner = null;
			}

			Cards.Clear(); // Or return do deck if there is only a specific number of cards, and cards have to be returned
		}
	}
}
