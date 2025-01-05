using GameServer.Models;
using GameServer.Models.Fields;
using GameServer.Models.Packets;
using System.Net.Sockets;

namespace GameServer.GameLogic
{
    public class Game
    {
        private static Random rng = new Random();
        private GameBoard? _board;
        private Thread? _startGameCounter;
        private int counterId = 0;
        private IFieldVisitor? _fieldVisitor;

        public event EventHandler<Game>? OnGameStarted;
        public bool Started { get; set; }
        private bool _counterStarted = false;
        public Player? CurrentMover;
        //public Action<Packet> Callback { get; set; }
		public event EventHandler<Packet>? FieldEventOccurred;
		public List<Player> Players { get; set; } = [];

        public void Setup()
        {
            _fieldVisitor = new DefaultThemeVisitor();

            if (Players.Count < 2)
            {
                throw new InvalidOperationException("Not enough players registered to start the game");
            }

            RandomizePlayerOrder();

            CurrentMover = Players.First();
			_board = new GameBoard();

			foreach (Player player in Players)
            {
                player.Currency = 1500;
                player.Board = _board;
            }

            // fill all of the fields and cards 
            var field = new Site()
            {
                Name = "GO",
                BuyingPrice = 100,
                RentPrices = new int[6] { 100, 200, 300, 400, 500, 600 },
                Housecount = 0,
                Group = null
            };

            field.FieldEventOccurred += OnFieldEventOccurred;

            _board.AddField(field);

            _board.AddCard(new Card() { Name = "Get Out Of Jail Free!" });
        }

        public void StartCounter()
        {
            if (_counterStarted)
            {
                return;
            }
            
            _counterStarted = true;
            _startGameCounter = new Thread(() =>
            {
                int myId = counterId;
                Thread.Sleep(10 * 1000);

                if (counterId != myId)
                {
                    return;
                }

                Started = true;
                OnGameStarted?.Invoke(this, this);
            });

            _startGameCounter.Start();
        }

        public void StopCounter()
        {
            if (!_counterStarted)
            {
                return;
            }

            counterId++;
            _counterStarted = false;
        }

        private void OnFieldEventOccurred(object? sender, Packet e)
        {
            //Callback(e);
			FieldEventOccurred?.Invoke(this, e);
		}

        private void RandomizePlayerOrder()
        {
            Players = Players.OrderBy(a => rng.Next()).ToList();
        }

        public int RollDice()
        {
			Random random = new Random();
			int rolled = random.Next(1, 7) + random.Next(1, 7);

            return rolled;
		}

		public void MovePlayer(Player player, int rolled)
		{
			if (_board == null)
			{
				throw new ArgumentNullException("The instance of the gameBoard must not be null");
			}

			IField newPosition = _board.Move(_fieldVisitor, player, rolled);

            if (newPosition.GetType() == typeof(Utility)) 
            {
                Utility utility = (Utility)newPosition;
                utility.RolledDice = rolled;
                utility.Accept(_fieldVisitor, player, true);
                return;
            }

            newPosition.Accept(_fieldVisitor, player, true);
		}
	}
}
