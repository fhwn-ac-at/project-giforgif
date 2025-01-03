namespace GameServer.Models
{
    public interface IField
    {
        // When player lands on the field
        public void LandOn(Player player);

        // When player passes the field
        public void Pass(Player player);
    }
}