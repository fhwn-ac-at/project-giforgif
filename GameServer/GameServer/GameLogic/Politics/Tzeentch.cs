

using GameServer.Models.Packets;

namespace GameServer.GameLogic.Politics
{
    public class Tzeentch : God
    {
        public override int Id => 3;

        public new void Activate(GameBoard board, List<Player> players, Random rng)
        {
            base.Activate(board, players, rng);

            var eligibleFields = board.GetPropertyFieldsOf(null);

            if (eligibleFields.Count == 0)
            {
                // There is no empty property
                return;
            }

            var randomField = eligibleFields[rng.Next(eligibleFields.Count)];
            var randomPlayer = players[rng.Next(players.Where(p => !p.IsBankrupt).Count())];
            randomField.Owner = randomPlayer;

            RaiseEvent("GotProperty", new BoughtFieldPacket() { FieldId = randomField.Id, PlayerName = randomPlayer.Name, ReducedBy = 0 });
        }

        public new void Deactivate(GameBoard board, List<Player> players, Random rng)
        {
            return;
        }
    }
}