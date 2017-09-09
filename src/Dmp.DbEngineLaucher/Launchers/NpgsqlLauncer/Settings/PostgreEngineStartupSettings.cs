namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// settings for server start
	/// </summary>
	public class PostgreEngineStartupSettings
	{
		/// <summary>
		/// set true if you use sql engine already installed and running
		/// </summary>
		public bool ExternalServer { get; set; }

		/// <summary>
		/// setting for server host and port
		/// </summary>
		public ServerArrdessSettings ServerAddressSettings { get; set; }

		/// <summary>
		/// setting for db cluster creation
		/// </summary>
		public DbCreationSettings PostgreDbSettings { get; set; }

		public PostgreEngineStartupSettings(ServerArrdessSettings serverAddressSettings = null, DbCreationSettings postgreDbSettings = null, bool externalServer = false)
		{
			ExternalServer = externalServer;
			ServerAddressSettings = serverAddressSettings ?? new ServerArrdessSettings();
			PostgreDbSettings = postgreDbSettings ?? new DbCreationSettings("postgres", "postgres");
		}
	}
}
