using GameServer.GameLogic;
using GameServer.Models.Fields;

namespace GameServer.Models
{
    public class CardEffect
    {
        public bool IsInstant { get; set; }

        public Action<Player, Game, ActionField> Effect { get; set; }

        public CardEffect(bool isInstant, Action<Player, Game, ActionField> effect)
        {
            IsInstant = isInstant;
            Effect = effect;
        }

    }
}
