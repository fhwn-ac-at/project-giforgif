using GameServer.GameLogic;

namespace GameServer.Models.Fields
{
    public class PropertyGroup
    {
        //public string GroupID { get; set; }

        public PropertyField[] Properties { get; set; } = [];

        public int AmountOfWOwnedProperties(Player player)
        {
           return Properties.Select(p => p.Owner == player).Count();
        }

        public bool AllPropertiesOwnedBy(Player player)
        {
            return Properties.All(p => p.Owner == player);
        }
    }
}
