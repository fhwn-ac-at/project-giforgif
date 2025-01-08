using GameServer.GameLogic;

namespace GameServer.Models.Fields
{
    public class Tax : ActionField
    {
        public int Amount { get; set; }

        public override void Accept(IFieldVisitor visitor, Player player, bool isLanding)
        {
            visitor.Visit(this, player, isLanding);
        }
    }
}
