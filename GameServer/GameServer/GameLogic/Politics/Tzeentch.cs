

using GameServer.Models.Fields;
using GameServer.Models.Packets;

namespace GameServer.GameLogic.Politics
{
    public class Tzeentch : God
    {
        public override int Id => 3;

        public new void Deactivate(GameBoard board, List<Player> players, Random rng)
        {
            return;
        }

        public override void Perform(GameBoard board, List<Player> players, Random rng)
        {
            this.Activate(board, players, rng);

            var eligibleFields = board.GetFields().Where(p => p is PropertyField field && field.Owner == null);

            if (eligibleFields.Count() == 0)
            {
                // There is no empty property
                return;
            }

            var randomField = (PropertyField)eligibleFields.ToList()[rng.Next(eligibleFields.Count())];
            var randomPlayer = players[rng.Next(players.Where(p => !p.IsBankrupt).Count())];
            randomField.Owner = randomPlayer;

            RaiseEvent("GotProperty", new BoughtFieldPacket() { FieldId = randomField.Id, PlayerName = randomPlayer.Name, ReducedBy = 0 });
        }
    }
}