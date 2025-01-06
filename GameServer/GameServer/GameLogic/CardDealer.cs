using GameServer.Models;

namespace GameServer.GameLogic
{
    public class CardDealer
    {
        private Random _random = new Random();
        private Dictionary<int, Card> _cards = new Dictionary<int, Card>();

        public CardDealer()
        {
            _random = new Random();
            // How to load cards into this Dealer?
        }

        public Card DrawCard()
        {
            int index = _random.Next(0, _cards.Count);

            _cards.Remove(index);

            return _cards[index];
        }

        public void Add(Card card)
        {
            _cards.Add(card.Id, card);
        }

        public void Add(IEnumerable<Card> cards)
        {
            foreach(var card in cards)
            {
                Add(card);
            }
        }
    }
}
