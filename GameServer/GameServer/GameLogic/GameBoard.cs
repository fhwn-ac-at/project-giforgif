using GameServer.Models;
using GameServer.Models.Fields;

namespace GameServer.GameLogic
{
	public class GameBoard
	{
		//private List<IField> _fields { get; set; } = [];
		public Dictionary<string, PropertyGroup> Groups { get; set; } = new Dictionary<string, PropertyGroup>();

		private Dictionary<int, IField> _fields { get; set; } = new Dictionary<int, IField>();
		
        private CardDealer _cardDealer = new CardDealer();

		public void AddField(IField field)
		{
			field.Id = _fields.Count + 1;
			_fields.Add(field.Id, field);
		}

		public IField? GetFieldById(int id)
		{
            if (_fields.ContainsKey(id) == false)
                throw new System.Exception($"Field with id {id} not found");

            return _fields[id];
        }

		public void AddCard(Card card)
		{
			_cardDealer.Add(card);
		}

		public void AddCards(IEnumerable<Card> cards)
		{
            _cardDealer.Add(cards);
        }

		public Card DrawCard()
		{
            return _cardDealer.DrawCard();
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
