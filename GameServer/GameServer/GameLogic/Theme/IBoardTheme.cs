using GameServer.Models.Packets;

namespace GameServer.GameLogic.Theme
{
    public interface IBoardTheme
    {
        public void LoadBoardTheme(EventHandler<Packet> OnFieldEventOccurred);
    }
}
