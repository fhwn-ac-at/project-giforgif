
using GameServer.Models.Fields;

namespace GameServer.GameLogic.Politics
{
    public class Nurgle : God
    {
        public override int Id => 1;

        private static readonly int RentModifier = 2;

        public new void Activate(GameBoard board, List<Player> players, Random rng)
        {
            base.Activate(board, players, rng);

            var eligibleFiels = board.GetFields().Where(f => f is PropertyField).ToList();

            eligibleFiels.ForEach(f =>
            {
                var field = (PropertyField)f;
                field.GodRentModifier = RentModifier;
            });
        }

        public new void Deactivate(GameBoard board, List<Player> players, Random rng)
        {
            base.Activate(board, players, rng);

            var eligibleFiels = board.GetFields().Where(f => f is PropertyField).ToList();

            eligibleFiels.ForEach(f =>
            {
                var field = (PropertyField)f;
                field.GodRentModifier = 1;
            });
        }
    }
}