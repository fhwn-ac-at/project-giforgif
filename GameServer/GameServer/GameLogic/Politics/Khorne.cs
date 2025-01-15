
using GameServer.Models.Packets;

namespace GameServer.GameLogic.Politics
{
    public class Khorne : God
    {
        public override int Id => 0;

        public new void Deactivate(GameBoard board, List<Player> players, Random rng)
        {
            return;
        }

        public override void Perform(GameBoard board, List<Player> players, Random rng)
        {
            this.Activate(board, players, rng);

            foreach (var player in players.Where(p => !p.IsBankrupt))
            {
                // Everyone back to Go
                player.CurrentPositionFieldId = 1;

                RaiseEvent("MovePlayer", new MovePlayerPacket() { FieldId = 1, PlayerName = player.Name });
            }
        }
    }
}