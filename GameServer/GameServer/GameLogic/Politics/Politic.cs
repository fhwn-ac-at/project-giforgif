using GameServer.Models.Packets;

namespace GameServer.GameLogic.Politics
{
    public class Politic
    {
        private static readonly Random Rng = new();

        private GameBoard _board;
        private List<Player> _players;
        private List<God> _gods;

        private static readonly int _chanceToGetGod = 10;
        private static readonly int _godDuration = 1;

        private int _currentGodDuration = 0;
        private God? _currentGod;

        public Politic(GameBoard board, List<Player> players, EventHandler<Packet> callback)
        {
            _board = board;
            _players = players;

            _gods = new List<God>()
            {
                new Khorne(),
                new Nurgle(),
                new Slaanesh(),
                new Tzeentch(),
                new Sigmar()
            };

            foreach (var god in _gods)
            {
                god.FieldEventOccurred += callback;
            }
        }

        public void RollForGod()
        {
            if (Rng.Next(0, 1 + _chanceToGetGod) == 0)
            {
                God god = _gods[Rng.Next(0, _gods.Count)];

                _currentGodDuration = _godDuration;
                _currentGod = god;

                god.Activate(_board, _players, Rng);
            }
        }

        public void Update()
        {
            if (_currentGodDuration > 0)
            {
                _currentGodDuration--;
                if (_currentGodDuration == 0 && _currentGod != null)
                {
                    _currentGod.Deactivate(_board, _players, Rng);

                    _currentGod = null;
                }
            }
        }

        public void TurnEnd()
        {
            if (_currentGod != null && _currentGodDuration != 0)
            {
                // There is a god still active, do nothing
                Update();
                return;
            }

            RollForGod();
        }
    }
}
