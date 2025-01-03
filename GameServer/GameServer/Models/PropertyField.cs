
namespace GameServer.Models
{
	public abstract class PropertyField : IField
	{
        public Player? Owner { get; set; }

        public string Name { get; set; }

        public int BuyingPrice { get; set; }

		public PropertyGroup Group { get; set; } // muss halt noch befüllt werden

		public event EventHandler<FieldEventArgs>? FieldEventOccurred;

		public abstract void LandOn(Player player);

        public abstract void Pass(Player player);

		public void RaiseEvent(string messageType, object? data)
		{
			FieldEventOccurred?.Invoke(this, new FieldEventArgs(messageType, data));
		}
	}
}
