namespace GameServer.Models
{
	public class FieldEventArgs
	{
		public string MessageType { get; set; }
		public object? Data { get; set; }

		public FieldEventArgs(string messageType, object? data)
		{
			this.MessageType = messageType;
			this.Data = data;
		}
	}
}