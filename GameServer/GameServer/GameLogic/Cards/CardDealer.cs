using GameServer.Models;

namespace GameServer.GameLogic
{
    public class CardDealer
    {
        private Random _random = new Random();
        private Dictionary<int, Card> _chanceCards = new Dictionary<int, Card>();
        private Dictionary<int, Card> _communityCards = new Dictionary<int, Card>();

        public CardDealer()
        {
            _random = new Random();
            // How to load cards into this Dealer?
        }

        public Card DrawChanceCard()
        {
            // Handle use case that there are no cards left

            int index = _random.Next(0, _chanceCards.Count);
            return _chanceCards[1];
            return _chanceCards[index];
        }

        public void AddChanceCard(Card card)
        {
            _chanceCards.Add(card.Id, card);
        }

        public void AddChanceCard(IEnumerable<Card> cards)
        {
            foreach(var card in cards)
            {
                AddChanceCard(card);
            }
        }

        public Card DrawCommunityCard()
        {
            // Handle use case that there are no cards left

            int index = _random.Next(0, _communityCards.Count);

            return _communityCards[index];
        }

        public void AddCommunityCard(Card card)
        {
            _communityCards.Add(card.Id, card);
        }

        public void AddCommunityCard(IEnumerable<Card> cards)
        {
            foreach(var card in cards)
            {
                AddCommunityCard(card);
            }
        }
    }
}
