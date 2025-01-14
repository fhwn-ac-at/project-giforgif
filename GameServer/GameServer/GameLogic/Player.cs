using GameServer.Data.Models;
using GameServer.Models;
using GameServer.Models.Fields;
using GameServer.Models.Packets;
using GameServer.Models.Packets.SellPossesions.Outgoing;

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

		public bool IsBankrupt = false;

		public GameBoard? Board;
		public List<Card> Cards { get; set; } = new();

		public Player(string name, string connectionId) 
		{
			Name = name;
			ConnectionId = connectionId;
			CurrentPositionFieldId = 1;
		}

		public bool TransferCurrency(Player recipient, int amount, PropertyField callback)
		{
            // Callback is used to send events to frontend, can be null, will just not send the events if thats the case

            if (!CanAfford(amount))
            {
				// Player cant afford to Transfer the currency, has to go into "debt mode"
                Console.WriteLine($"Player {Name} can not pay the amount of {amount}. His networth is {CalculateNetWorth()}");

                if (CalculateNetWorth() < amount)
                {
                    IsBankrupt = true;
                    // If Player is not able to pay the owned rent, not even "debt mode" saves him
                    if (callback != null)
						DeclareBankruptcyToPlayer(callback.Owner);

					if (callback != null)
					{
						callback.RaiseEvent("BANKRUPTCY", new BankruptcyPacket() { PlayerName = Name });
						callback.RaiseEvent("TRANSFER_PROPERTIES", new TransferPropertiesPacket() { From = Name, To = callback.Owner.Name });
					}

                    Console.WriteLine($"Player {Name} declared Bankrupcty to {callback.Owner.Name}. All Properties got transferred.");

                    return false;
                }

                //Player can still sell properties to pay the rent -> goes into "debt mode"
                Console.WriteLine($"Player {Name} entered Debt Mode with {amount} in debt and to player {callback.Owner.Name}.");
                OwesMoney = callback.Owner;
                AmountOwed = amount;
				
				if (callback != null)
					callback.RaiseEvent("SELL_PROPERTIES", new SellPropertiesPacket() { Amount = amount - Currency, PlayerName = Name });

                return false;
			}

			Currency -= amount;
			recipient.Currency += amount;

			if (callback != null)
				callback.RaiseEvent("TRANSFER_MONEY", new PayPlayerPacket() { From = Name, To = recipient.Name, Amount = amount });

			return true;
		}

		public bool CanAfford(int amount)
		{
			return Currency >= amount;
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

		public void DeductCurrency(int amount, IField callback)
		{
			if (Currency < amount)
			{
                // Player cant afford to Transfer the currency, has to go into "debt mode"
                Console.WriteLine($"Player {Name} can not pay the amount of {amount}. His networth is {CalculateNetWorth()}");

                if (CalculateNetWorth() < amount)
				{
                    // If Player is not able to pay the owned rent, not even "debt mode" saves him
                    if (callback != null)
                        DeclareBankruptcyToBank();

					if (callback != null)
					{
						callback.RaiseEvent("BANKRUPTCY", new BankruptcyPacket() { PlayerName = Name });
						callback.RaiseEvent("TRANSFER_PROPERTIES", new TransferPropertiesPacket() { From = Name, To = null });
					}
					
                    Console.WriteLine($"Player {Name} declared Bankrupcty to Bank. All Properties got transferred.");
                    IsBankrupt = true;

                    return;
                }

                //Player can still sell properties to pay the rent -> goes into "debt mode"
				Console.WriteLine($"Player {Name} entered Debt Mode with {amount} in debt and to player BANK.");
                AmountOwed = amount;
				OwesMoney = new Player("Bank", "Bank"); // tmp for checks

				if (callback != null)
				{
					Console.WriteLine("CALLBACK WAS NULL.");
					callback.RaiseEvent("SELL_PROPERTIES", new SellPropertiesPacket() { Amount = amount - Currency, PlayerName = Name });
				}

                return;
            }


			Currency -= amount;

            if (callback != null)
			{
				callback.RaiseEvent("REMOVE_MONEY", new RemoveMoneyPacket() { PlayerName = Name, Amount = amount, Description = "Paid money" });
			}
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
