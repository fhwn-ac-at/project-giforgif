
using GameServer.Models.Fields;
using GameServer.Models.Packets;

namespace GameServer.GameLogic.Politics
{
    public class Nurgle : God
    {
        public override int Id => 1;

        private static readonly int RentModifier = 2;

        public override void Perform(GameBoard board, List<Player> players, Random rng)
        {
            this.Activate(board, players, rng);

            var eligibleFiels = board.GetFields().Where(f => f is PropertyField).ToList();

            eligibleFiels.ForEach(f =>
            {
                var field = (PropertyField)f;
                field.GodRentModifier = RentModifier;
            });

            RaiseEvent("RENT_INCREASE", new RentChangedPacket() { NewMultiplier = RentModifier });
        }

        public override void Revert(GameBoard board, List<Player> players, Random rng)
        {
            this.Deactivate(board, players, rng);

            var eligibleFiels = board.GetFields().Where(f => f is PropertyField).ToList();

            eligibleFiels.ForEach(f =>
            {
                var field = (PropertyField)f;
                field.GodRentModifier = 1;
            });
        }
    }
}