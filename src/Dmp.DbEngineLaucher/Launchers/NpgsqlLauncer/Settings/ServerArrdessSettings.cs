namespace Dmp.NpgsqlEngineLaucher
{
	public class ServerArrdessSettings
	{
		/// <summary>
		/// server adress (localhost)
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		/// port
		/// </summary>
		public int Port { get; set; }

		public ServerArrdessSettings(string server = null, int port = 5432)
		{
			Server = server;
			Port = port;
		}
	}
}
