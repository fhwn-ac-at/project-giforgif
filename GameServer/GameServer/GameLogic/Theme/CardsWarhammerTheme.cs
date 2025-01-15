using GameServer.Models;
using GameServer.Models.Fields;

namespace GameServer.GameLogic.Theme
{
    public class CardsWarhammerTheme : ICardTheme
    {
        private CardDealer _cardDealer;

        public CardsWarhammerTheme(CardDealer dealer)
        {
            _cardDealer = dealer;
        }

        public void LoadChanceCards()
        {
            _cardDealer.AddChanceCard(loadChanceCards());
        }

        public void LoadCommunityCards()
        {
            _cardDealer.AddCommunityCard(loadCommunityCards());
        }

        private IEnumerable<Card> loadChanceCards()
        {
            var cards = new List<Card>();

            // Go to Jail
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, 11, true);
                p.RoundsLeftInJail = 3;
            }))));

            // Move Player to first Railroad
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, 6, false);
            }))));

            // Pay each Player 50
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                foreach(var player in g.Players.Where(player => !player.IsBankrupt && player != p))
                {
                    p.TransferCurrency(player, 50, f);
                }
            }))));

            // Move Player to first Field after Jail
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, 12, false);
            }))));

            // Collect 150
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 150;
            }))));

            // Pay 15
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.DeductCurrency(50, f);
            }))));

            // Advance to nearest Railroad and pay the normal rental
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                var closestRailroad = p.Board.Groups["station"].Properties
                .Where(r => r.Id > p.CurrentPositionFieldId)
                .OrderBy(r => r.Id)
                .FirstOrDefault()
                ?? p.Board.Groups["station"].Properties.OrderBy(r => r.Id).First();

                g.SetPlayerPosition(p, closestRailroad.Id, false);
            }))));

            // Move Back 3
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, p.CurrentPositionFieldId - 3, true);
            }))));

            // Advance to nearest Railroad and pay the normal rental
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                var closestRailroad = p.Board.Groups["station"].Properties
                .Where(r => r.Id > p.CurrentPositionFieldId)
                .OrderBy(r => r.Id)
                .FirstOrDefault()
                ?? p.Board.Groups["station"].Properties.OrderBy(r => r.Id).First();

                g.SetPlayerPosition(p, closestRailroad.Id, false);
            }))));

            // Advance to nearest utility and pay the normal rental
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                var closestRailroad = p.Board.Groups["utility"].Properties
                .Where(r => r.Id > p.CurrentPositionFieldId)
                .OrderBy(r => r.Id)
                .FirstOrDefault()
                ?? p.Board.Groups["utility"].Properties.OrderBy(r => r.Id).First();

                g.SetPlayerPosition(p, closestRailroad.Id, false);
            }))));

            // Move to Field ID 21
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, 21, false);
            }))));

            // Move to Field ID 39
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, 39, false);
            }))));

            // Pay 25 for each house and 100 for each hotel
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                var fields = p.Board.GetPropertyFieldsOf(p);
                var count = 0;
                foreach (var field in fields)
                {
                    if (field is Site site)
                    {
                        count += site.Housecount < 5
                        ? site.Housecount * 25
                        : 100 + (4 * 25);
                    }
                }

                p.DeductCurrency(count, f);
            }))));

            return cards;
        }

        private IEnumerable<Card> loadCommunityCards()
        {
            var cards = new List<Card>();

            // Pay 40 for each house and 115 for each hotel
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                var fields = p.Board.GetPropertyFieldsOf(p);
                var count = 0;
                foreach (var field in fields)
                {
                    if (field is Site site)
                    {
                        count += site.Housecount < 5
                        ? site.Housecount * 40
                        : 115 + (4 * 40);
                    }
                }

                p.DeductCurrency(count, f);
            }))));

            // Go to Jail
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, 11, true);
                p.RoundsLeftInJail = 3;
            }))));

            // Give 100
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 100;
            }))));

            // Give 45
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 45;
            }))));

            // Give 100
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 100;
            }))));

            // Give 25
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 25;
            }))));

            // Pay 150
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.DeductCurrency(150, f);
            }))));

            // Give 10
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 10;
            }))));

            // Collect 50 from every Player
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                foreach (var player in g.Players.Where(player => !player.IsBankrupt && player != p))
                {
                    player.TransferCurrency(p, 50, f);
                }
            }))));

            // Pay 50
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.DeductCurrency(50, f);
            }))));

            // Pay 100
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.DeductCurrency(100, f);
            }))));

            // Give 20
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 20;
            }))));

            // Give 200
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 200;
            }))));

            // Give 100
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 100;
            }))));

            return cards;
        }
    }
}
