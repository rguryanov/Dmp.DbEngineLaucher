namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// <see cref="ISettingsProvider"/>
	/// </summary>
	public class SimpleSettingsProvider: ISettingsProvider
	{
		public SimpleSettingsProvider(InstallationSettings installSettings, PostgreEngineStartupSettings startupSettings = null, RunSettings runSettings = null)
		{
			InstallSettings = installSettings;
			StartupSettings = startupSettings ?? new PostgreEngineStartupSettings();
			RunSettings = runSettings ?? new RunSettings(true, true);
			EngineConstants = PostreSqlEngineConstants.GetDefault();
		}

		/// <summary>
		/// <see cref="ISettingsProvider"/>
		/// </summary>
		public PostgreEngineStartupSettings StartupSettings { get; }

		/// <summary>
		/// <see cref="ISettingsProvider"/>
		/// </summary>
		public InstallationSettings InstallSettings { get; }

		/// <summary>
		/// <see cref="ISettingsProvider"/>
		/// </summary>
		public RunSettings RunSettings { get; }

		/// <summary>
		/// <see cref="ISettingsProvider"/>
		/// </summary>
		public PostreSqlEngineConstants EngineConstants { get; }
	}
}
