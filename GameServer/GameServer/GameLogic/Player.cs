using GameServer.Models;
using GameServer.Models.Fields;

namespace GameServer.GameLogic
{
	public class Player
	{
		// das ist noch nicht der richtige content
		public string ConnectionId { get; set; }
		public string Name { get; set; }
		public int Currency { get; set; }
		public int CurrentPositionFieldId { get; set; }
		public string Color { get; set; }
		public int RoundsLeftInJail { get; set; }

		public Player? OwesMoney { get; set; }
		public int AmountOwed { get; set; }

		public GameBoard? Board;
		public List<Card> Cards { get; set; } = new();

		public Player(string name, string connectionId) 
		{
			Name = name;
			ConnectionId = connectionId;
			CurrentPositionFieldId = 1;
		}

		public bool TransferCurrency(Player recipient, int amount)
		{
			if (!CanAfford(amount))
			{
				return false;
			}

			Currency -= amount;
			recipient.Currency += amount;

			return true;
		}

		public bool CanAfford(int amount)
		{
			return (Currency - AmountOwed) >= amount;
		}

		public bool BuyField(PropertyField field, int price)
		{
			if (!CanAfford(price))
			{
				return false;
			}

			Currency -= price;
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
					int sellvalue = site.Housecount * (site.BuildingPrice / 2);
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

		public void DeductCurrency(int amount)
		{
			Currency -= amount;
		}

		public int CalculateNetWorth()
		{
            int netWorth = Currency;

            var properties = Board.GetPropertyFieldsOf(this);

            if (properties == null)
                return netWorth;

            foreach (PropertyField property in properties)
			{
                if (property is Site site)
				{
                    netWorth += (site.BuildingPrice / 2) * site.Housecount;
                }

                netWorth += property.BuyingPrice;
            }

            return netWorth - AmountOwed;
        }

		public int SellProperty(PropertyField property)
		{
			if (property.Owner != this)
			{
                return 0;
            }

			int sellvalue = 0;

            if (property is Site site)
			{
				if (site.Housecount > 0)
				{
					return 0;
				}

                sellvalue += site.Housecount * (site.BuildingPrice / 2);
            }

            sellvalue += property.BuyingPrice;
            Currency += sellvalue;
            property.Owner = null;

			return sellvalue;
        }

		public int SellHouse(Site site)
		{
            if (site.Owner != this)
			{
                return 0;
            }

            if (site.Housecount == 0)
			{
                return 0;
            }

            Currency += site.BuildingPrice / 2;
            site.Housecount--;

            return site.BuildingPrice / 2;
        }
	}
}
