

using GameServer.Models.Packets;

namespace GameServer.GameLogic.Politics
{
    public class Sigmar : God
    {
        public override int Id => 4;

        private static readonly int BonusCurrency = 200;

        public new void Deactivate(GameBoard board, List<Player> players, Random rng)
        {
            return;
        }

        public override void Perform(GameBoard board, List<Player> players, Random rng)
        {
            this.Activate(board, players, rng);

            foreach (var player in players.Where(p => !p.IsBankrupt))
            {
                player.Currency += BonusCurrency;

                RaiseEvent("PlayerCurrencyUpdate", new AddMoneyPacket() { PlayerName = player.Name, Amount = BonusCurrency, Description = "The imperator has blessed you." });
            }
        }
    }
}