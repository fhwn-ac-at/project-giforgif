using GameServer.Data.Models;
using GameServer.Models;
using GameServer.Models.Fields;
using GameServer.Models.Packets;

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

                f.RaiseEvent("GO_TO_JAIL", new GoToJailPacket() { PlayerName = p.Name });
            }))));

            // Move Player to first Railroad
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, 6, false);

                f.RaiseEvent("MOVE_PLAYER", new MovePlayerPacket() { PlayerName = p.Name, FieldId = 6});
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

                f.RaiseEvent("MOVE_PLAYER", new MovePlayerPacket() { PlayerName = p.Name, FieldId = 12 });
            }))));

            // Collect 150
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 150;

                f.RaiseEvent("ADD_MONEY", new AddMoneyPacket() { PlayerName = p.Name, Amount = 150, Description = "Collect 150" });
            }))));

            // Pay 15
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {   
                p.DeductCurrency(15, f);
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

                f.RaiseEvent("MOVE_PLAYER", new MovePlayerPacket() { PlayerName = p.Name, FieldId = closestRailroad.Id });
            }))));

            // Move Back 3
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, p.CurrentPositionFieldId - 3, true);

                f.RaiseEvent("MOVE_PLAYER", new MovePlayerPacket() { PlayerName = p.Name, FieldId = p.CurrentPositionFieldId - 3 });
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

                f.RaiseEvent("MOVE_PLAYER", new MovePlayerPacket() { PlayerName = p.Name, FieldId = closestRailroad.Id });
            }))));

            // Advance to nearest utility and pay the normal rental
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                var closestUtility = p.Board.Groups["utility"].Properties
                .Where(r => r.Id > p.CurrentPositionFieldId)
                .OrderBy(r => r.Id)
                .FirstOrDefault()
                ?? p.Board.Groups["utility"].Properties.OrderBy(r => r.Id).First();

                g.SetPlayerPosition(p, closestUtility.Id, false);

                f.RaiseEvent("MOVE_PLAYER", new MovePlayerPacket() { PlayerName = p.Name, FieldId = closestUtility.Id });
            }))));

            // Move to Field ID 21
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, 21, false);

                f.RaiseEvent("MOVE_PLAYER", new MovePlayerPacket() { PlayerName = p.Name, FieldId = 21 });
            }))));

            // Move to Field ID 39
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                g.SetPlayerPosition(p, 39, false);

                f.RaiseEvent("MOVE_PLAYER", new MovePlayerPacket() { PlayerName = p.Name, FieldId = 39 });
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

                f.RaiseEvent("GO_TO_JAIL", new GoToJailPacket() { PlayerName = p.Name });
            }))));

            // Give 100
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 100;

                f.RaiseEvent("ADD_MONEY", new AddMoneyPacket() { PlayerName = p.Name, Amount = 100, Description = "Collect 100" });
            }))));

            // Give 45
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 45;

                f.RaiseEvent("ADD_MONEY", new AddMoneyPacket() { PlayerName = p.Name, Amount = 45, Description = "Collect 45" });
            }))));

            // Give 100
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 100;

                f.RaiseEvent("ADD_MONEY", new AddMoneyPacket() { PlayerName = p.Name, Amount = 100, Description = "Collect 100" });
            }))));

            // Give 25
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 25;

                f.RaiseEvent("ADD_MONEY", new AddMoneyPacket() { PlayerName = p.Name, Amount = 25, Description = "Collect 25" });
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

                f.RaiseEvent("ADD_MONEY", new AddMoneyPacket() { PlayerName = p.Name, Amount = 10, Description = "Collect 10" });
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

                f.RaiseEvent("ADD_MONEY", new AddMoneyPacket() { PlayerName = p.Name, Amount = 20, Description = "Collect 20" });
            }))));

            // Give 200
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 200;

                f.RaiseEvent("ADD_MONEY", new AddMoneyPacket() { PlayerName = p.Name, Amount = 200, Description = "Collect 200" });
            }))));

            // Give 100
            cards.Add(new Card(cards.Count, new CardEffect(true, new Action<Player, Game, ActionField>((p, g, f) =>
            {
                p.Currency += 100;

                f.RaiseEvent("ADD_MONEY", new AddMoneyPacket() { PlayerName = p.Name, Amount = 100, Description = "Collect 100" });
            }))));

            return cards;
        }
    }
}
