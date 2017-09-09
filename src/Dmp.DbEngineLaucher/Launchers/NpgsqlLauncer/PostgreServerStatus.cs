namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// Status of current PostgreServer process and instalation
	/// </summary>
	public class PostgreServerStatus
	{
		/// <summary>
		/// engine installed
		/// </summary>
		public bool EngineIsInstalled { get; set; }

		/// <summary>
		/// has master db
		/// </summary>
		public bool HasMastertDb { get; set; }

		///// <summary>
		///// instalation is valid (check if folders has all needed files)
		///// </summary>
		//public bool IsValidEngineInstallation { get; set; }

		/// <summary>
		/// server process started
		/// </summary>
		public bool EngineProcessStarted { get; set; }
	}
}
