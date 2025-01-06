using GameServer.GameLogic;

namespace GameServer.Models
{
	public class Card // evtl. unterscheidung zwischen Karten die man besitzen kann und nicht besitzen kann
	{
		public int Id { get; set; }

		public string? Name { get; set; }

		public string? Description { get; set; }

		public Action<Player>? Effect { get; set; }
	}
}
