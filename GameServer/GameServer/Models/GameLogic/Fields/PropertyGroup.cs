namespace GameServer.Models
{
    public class PropertyGroup
    {
        public string GroupID { get; set; }

        public PropertyField[] Properties { get; set; }

        public int AmountOfWOwnedProperties(Player player)
        {
           return Properties.Select(p => p.Owner == player).Count();
        }
    }
}
