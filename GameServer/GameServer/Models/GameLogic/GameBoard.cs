using GameServer.Models.GameLogic;

namespace GameServer.Models
{
	public class GameBoard
	{
		private List<IField> _fields { get; set; } = [];
        private List<Card> _cards { get; set; } = [];

		public IField Move(Player player, int steps)
		{
            int currentPositionIndex = _fields.IndexOf(player.CurrentPosition);

            if (currentPositionIndex == -1)
            {
                throw new InvalidOperationException("Player's current position is not on the game board.");
            }

            int totalFields = _fields.Count;
            int newPositionIndex = (currentPositionIndex + steps) % totalFields;

            for (int i = 1; i <= steps; i++)
            {
                int passPositionIndex = (currentPositionIndex + i) % totalFields;
                _fields[passPositionIndex].Pass(player);
            }

            player.CurrentPosition = _fields[newPositionIndex];

            return _fields[newPositionIndex];
        }

        public void AddField(IField field)
        {
            _fields.Add(field);
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }
	}
}
