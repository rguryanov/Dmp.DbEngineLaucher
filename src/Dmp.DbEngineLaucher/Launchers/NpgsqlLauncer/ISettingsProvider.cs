using Dmp.NpgsqlEngineLaucher;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// settings provider for launcher implementations
	/// </summary>
	public interface ISettingsProvider
	{

		/// <summary>
		/// settings for server start (host and port and initdb params)
		/// </summary>
		PostgreEngineStartupSettings StartupSettings { get; }

		/// <summary>
		/// installation settings
		/// engine binaries location and cluster data directory
		/// </summary>
		InstallationSettings InstallSettings { get; }

		/// <summary>
		/// run and stop server settings (for cmd configurations and actions on errors/timeouts)
		/// </summary>
		RunSettings RunSettings { get; }

		/// <summary>
		/// Engine Constants - default timeouts and relative paths
		/// </summary>
		PostreSqlEngineConstants EngineConstants { get; }
	}
}
