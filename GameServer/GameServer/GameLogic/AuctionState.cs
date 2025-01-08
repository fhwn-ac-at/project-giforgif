namespace GameServer.GameLogic
{
	public class AuctionState
	{
		public int FieldId { get; set; }
		public Player HighestBidder { get; set; }
		public int HighestBid { get; set; }
		private Timer _timer;
		private readonly int _timeout = 10000;

		public event Action<AuctionState> AuctionEnded;

		public AuctionState(int FieldId)
		{
			FieldId = FieldId;
		}

		public bool PlaceBid(Player player, int bidAmount)
		{
			lock (this)
			{
				if (bidAmount <= HighestBid)
					return false;

				if (!player.CanAfford(bidAmount))
					return false;

				HighestBid = bidAmount;
				HighestBidder = player;

				RestartTimer();

				return true;
			}
		}

		private void RestartTimer()
		{
			lock (this)
			{
				_timer.Change(_timeout, Timeout.Infinite);
                Console.WriteLine("Timer got renewed");
            }
		}

		public void StartTimer()
		{
			_timer = new Timer(OnAuctionTimeout, null, _timeout, Timeout.Infinite);
            Console.WriteLine("Timer started!");
        }

		private void OnAuctionTimeout(object? state)
		{
			lock (this)
			{
				_timer.Dispose();
				AuctionEnded?.Invoke(this);
                Console.WriteLine("Auction timeouted");
            }
		}
	}
}
