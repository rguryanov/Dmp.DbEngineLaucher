using Dmp.DatabaseEngineLaucher;
using Dmp.DbEngineLaucher.Installation;
using System;
using System.Threading.Tasks;
using Dmp.DbEngineLaucher.OsSpecific;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// npg sql server database service
	/// use for start, stop initdb of postgresql engine
	/// </summary>
	public class PostgreSqlDataBaseEngineLauncher : IDatabaseService
	{
		private volatile bool _started;
		private readonly ISettingsProvider _settingsProvider;		
		private readonly IEngineInstalationResolver _installResolver;
		private readonly IRuntimePlatformResolver _runtimePlatformResolver;
		private readonly IPostgreLauncherResolver _launcerResolver;

		public IPostgresqlLauncherServiceProvider LauncherServiceProvider { get; set; }

		public PostgreSqlDataBaseEngineLauncher(ISettingsProvider settingsProvider,
			IEngineInstalationResolver installResolver, IRuntimePlatformResolver runtimePlatformResolver = null,
			IPostgreLauncherResolver launcerResolver = null)
		{
			_settingsProvider = settingsProvider;
			_installResolver = installResolver;
			LauncherServiceProvider = new PostgresqlLauncherServiceProvider(_settingsProvider);
			_runtimePlatformResolver = runtimePlatformResolver ?? new RuntimePlatformResolver();
			_launcerResolver = launcerResolver ?? new UniversalResolver();
		}

		public PostgreSqlDataBaseEngineLauncher(InstallationSettings installationSettings,
			IEngineInstalationResolver installResolver)
			: this(new SimpleSettingsProvider(installationSettings), installResolver)
		{
			
		}

		/// <summary>
		/// start server (tring install server binaries, if no was founded)
		/// </summary>
		public async Task StartAsync()
		{
			if (_started)
			{
				return;
			}

			if (_settingsProvider.StartupSettings.ExternalServer)
				return;

			var launcher = Getlauncher();
			var status = await launcher.GetStatusAsync();
			if (status.EngineIsInstalled)
			{
				if (!status.HasMastertDb)
				{
					await launcher.InitDbAsync();
				}

				// start if was stoped
				if (!status.EngineProcessStarted)
				{
					await launcher.StartEngineAsync();
					_started = true;
				}
			}
			else
			{
				var arch = _runtimePlatformResolver.GetRuntimeArchitecture();
				var os = _runtimePlatformResolver.GetOsPlatform();
				var intaller = _installResolver.GetIntaller(os, arch);
				await intaller.Install(_settingsProvider.InstallSettings.InstallPgSqlPath);
				await StartAsync();
			}
		}

		/// <summary>
		/// stop server
		/// </summary>
		public async Task StopAsync()
		{
			if (_settingsProvider.StartupSettings.ExternalServer)
			{
				return;
			}

			var launcher = Getlauncher();
			await launcher.StopEngineAsync();
			_started = false;
		}

		private IPostgresqlLauncher Getlauncher()
		{
			var arch = _runtimePlatformResolver.GetRuntimeArchitecture();
			var os = _runtimePlatformResolver.GetOsPlatform();
			var pltLauncher = _launcerResolver.GetLauncher(_settingsProvider, LauncherServiceProvider, os, arch);
			if (pltLauncher != null)
			{
				return pltLauncher;
			}

			throw new PlatformNotSupportedException("Your OS is not supported");
		}
	}
}
