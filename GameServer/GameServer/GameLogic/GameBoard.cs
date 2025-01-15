using GameServer.Models;
using GameServer.Models.Fields;

namespace GameServer.GameLogic
{
	public class GameBoard
	{
		//private List<IField> _fields { get; set; } = [];
		public Dictionary<string, PropertyGroup> Groups { get; set; } = new Dictionary<string, PropertyGroup>();

		private Dictionary<int, IField> _fields { get; set; } = new Dictionary<int, IField>();
		
        public CardDealer Dealer { get; set; } = new CardDealer();

		public void AddField(IField field)
		{
			field.Id = _fields.Count + 1;
			_fields.Add(field.Id, field);
		}

		public IField GetFieldById(int id)
		{
            if (_fields.ContainsKey(id) == false)
                throw new System.Exception($"Field with id {id} not found");

            return _fields[id];
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

		public IEnumerable<IField> GetFields()
		{
			   return _fields.Values;
		}
    }
}
