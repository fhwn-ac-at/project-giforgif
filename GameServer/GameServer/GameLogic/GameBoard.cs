using GameServer.Models;
using GameServer.Models.Fields;

namespace GameServer.GameLogic
{
	public class GameBoard
	{
		//private List<IField> _fields { get; set; } = [];

		private Dictionary<int, IField> _fields { get; set; } = [];
		private List<Card> _cards { get; set; } = [];

		public IField Move(IFieldVisitor fieldVisitor, Player player, int steps)
		{
			int currentPositionIndex = player.CurrentPositionFieldId;

			if (currentPositionIndex == -1)
			{
				throw new InvalidOperationException("Player's current position is not on the game board.");
			}

			int totalFields = _fields.Count;
			int newPositionIndex = (currentPositionIndex + steps) % totalFields;

			for (int i = 1; i <= steps; i++)
			{
				int passPositionIndex = (currentPositionIndex + i) % totalFields;
				_fields[passPositionIndex].Accept(fieldVisitor, player, false);
			}

			player.CurrentPositionFieldId = _fields[newPositionIndex].Id;

			return _fields[newPositionIndex];
		}

		public void AddField(IField field)
		{
			_fields.Add(field.Id, field);
		}

		public IField? GetFieldById(int id)
		{
			return _fields.TryGetValue(id, out var field) ? field : null;
		}

		public void AddCard(Card card)
		{
			_cards.Add(card);
		}

		public List<PropertyField>? GetPropertyFieldsOf(Player player)
		{
			if (_fields.Count == 0)
				return [];

			return _fields.Values.OfType<PropertyField>().Where(f => f.Owner == player).ToList();
		}

		//public PropertyField? GetPropertyByName(string name)
		//{
		//	return _fields.OfType<PropertyField>().Where(p => p.Name == name).FirstOrDefault();
		//}
	}
}
