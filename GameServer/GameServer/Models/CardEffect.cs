using GameServer.GameLogic;

namespace GameServer.Models
{
    public class CardEffect
    {
        public bool IsInstant { get; set; }

        public Action<Player> Effect { get; set; }

        public CardEffect(bool isInstant, Action<Player> effect)
        {
            IsInstant = isInstant;
            Effect = effect;
        }

    }
}
