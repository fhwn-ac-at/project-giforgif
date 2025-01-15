
using GameServer.Models.Fields;
using GameServer.Models.Packets;

namespace GameServer.GameLogic.Politics
{
    public class Slaanesh : God
    {
        public override int Id => 2;

        private static readonly int BuyModifier = 2;

        public override void Perform(GameBoard board, List<Player> players, Random rng)
        {
            this.Activate(board, players, rng);

            var eligibleFiels = board.GetFields().Where(f => f is PropertyField).ToList();

            eligibleFiels.ForEach(f =>
            {
                var field = (PropertyField)f;
                field.BuyingPrice = field.BuyingPrice * BuyModifier;
            });

            RaiseEvent("BUYING_INCREASE", new BuyingPriceChangedPacket() { NewMultiplier = BuyModifier });
        }

        public override void Revert(GameBoard board, List<Player> players, Random rng)
        {
            this.Deactivate(board, players, rng);

            var eligibleFiels = board.GetFields().Where(f => f is PropertyField).ToList();

            eligibleFiels.ForEach(f =>
            {
                var field = (PropertyField)f;
                field.BuyingPrice = field.BuyingPrice / BuyModifier;
            });
        }
    }
}