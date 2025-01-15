using GameServer.Models.Packets;

namespace GameServer.GameLogic.Politics
{
    public abstract class God
    {
        public abstract int Id { get; }

        public event EventHandler<Packet>? FieldEventOccurred;

        public void Activate(GameBoard board, List<Player> players, Random rng)
        {
            RaiseEvent("NEW_GOD", new PoliticActivatedPacket() { PoliticId = Id });
        }

        public void Deactivate(GameBoard board, List<Player> players, Random rng)
        {
            RaiseEvent("POLITIC_RESET", new PoliticResetPacket());
        }

        public abstract void Perform(GameBoard board, List<Player> players, Random rng);
        public abstract void Revert(GameBoard board, List<Player> players, Random rng);

        protected void RaiseEvent(string messageType, Packet data)
        {
            FieldEventOccurred?.Invoke(this, data);
        }
    }
}
