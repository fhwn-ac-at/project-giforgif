
using GameServer.Models.Fields;
using GameServer.Models.Packets;
using GameServer.Models.Packets.Gods.Outgoing;

namespace GameServer.GameLogic.Politics
{
    public class Slaanesh : God
    {
        public override int Id => 2;

        private static readonly int BuyModifier = 2;

        public new void Activate(GameBoard board, List<Player> players, Random rng)
        {
            base.Activate(board, players, rng);

            var eligibleFiels = board.GetFields().Where(f => f is PropertyField).ToList();

            eligibleFiels.ForEach(f =>
            {
                var field = (PropertyField)f;
                field.BuyingPrice = field.BuyingPrice * BuyModifier;
            });

            RaiseEvent("BUYING_INCREASE", new BuyingPriceChangedPacket() { NewMultiplier = BuyModifier });
        }

        public new void Deactivate(GameBoard board, List<Player> players, Random rng)
        {
            base.Activate(board, players, rng);

            var eligibleFiels = board.GetFields().Where(f => f is PropertyField).ToList();

            eligibleFiels.ForEach(f =>
            {
                var field = (PropertyField)f;
                field.BuyingPrice = field.BuyingPrice / BuyModifier;
            });
        }
    }
}