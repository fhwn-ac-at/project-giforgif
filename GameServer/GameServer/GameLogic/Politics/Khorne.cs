
using GameServer.Models.Packets;

namespace GameServer.GameLogic.Politics
{
    public class Khorne : God
    {
        public override int Id => 0;

        public new void Activate(GameBoard board, List<Player> players, Random rng)
        {
            base.Activate(board, players, rng);

            foreach (var player in players.Where(p => !p.IsBankrupt))
            {
                // Everyone back to Go
                player.CurrentPositionFieldId = 1;

                this.RaiseEvent("MovePlayer", new MovePlayerPacket() { FieldId = 1, PlayerName = player.Name});
            }
        }

        public new void Deactivate(GameBoard board, List<Player> players, Random rng)
        {
            return;
        }
    }
}