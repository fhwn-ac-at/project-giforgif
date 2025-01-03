namespace GameServer.Models
{
	public class FieldEventArgs
	{
		private string messageType;
		private object? data;

		public FieldEventArgs(string messageType, object? data)
		{
			this.messageType = messageType;
			this.data = data;
		}
	}
}