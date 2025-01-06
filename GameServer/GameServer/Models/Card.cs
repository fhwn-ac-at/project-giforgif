using GameServer.GameLogic;

namespace GameServer.Models
{
	public class Card // evtl. unterscheidung zwischen Karten die man besitzen kann und nicht besitzen kann
	{
		public int Id { get; set; }

		public string? Name { get; set; }

		public string? Description { get; set; }

		public CardEffect Effect { get; set; }

		public Card(int id, string name, string description, CardEffect effect)
		{
            Id = id;
            Name = name;
            Description = description;
            Effect = effect;
        }
	}
}
