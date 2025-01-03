namespace GameServer.Models
{
    public class PropertyField : IField
    {
        public Player? Owner { get; set; }
        public int HouseCount { get; set; }
        public IProperty Property { get; set; }

        public void LandOn(Player player)
        {
            if (Owner == null)
            {

            }
        }

        public void Pass(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
