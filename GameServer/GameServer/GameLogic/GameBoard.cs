using GameServer.Models;
using GameServer.Models.Fields;

namespace GameServer.GameLogic
{
	public class GameBoard
	{
		//private List<IField> _fields { get; set; } = [];

		private Dictionary<int, IField> _fields { get; set; } = [];
		
        private CardDealer _cardDealer = new CardDealer();

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
			_cardDealer.Add(card);
		}

		public void AddCards(IEnumerable<Card> cards)
		{
            _cardDealer.Add(cards);
        }

		public List<PropertyField>? GetPropertyFieldsOf(Player player)
		{
			if (_fields.Count == 0)
				return [];

			return _fields.Values.OfType<PropertyField>().Where(f => f.Owner == player).ToList();
		}

        public int GetFieldCount()
        {
			return _fields.Count();
        }

        //public PropertyField? GetPropertyByName(string name)
        //{
        //	return _fields.OfType<PropertyField>().Where(p => p.Name == name).FirstOrDefault();
        //}
    }
}
