using GameServer.GameLogic;

namespace GameServer.Models
{
	public class Card // evtl. unterscheidung zwischen Karten die man besitzen kann und nicht besitzen kann
	{
		public int Id { get; set; }

		public CardEffect Effect { get; set; }

		public Card(int id, CardEffect effect)
		{
            Id = id;
            Effect = effect;
        }
	}
}
