namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// <see cref="IPostgresqlLauncherServiceProvider"/>
	/// </summary>
	public class PostgresqlLauncherServiceProvider : IPostgresqlLauncherServiceProvider
	{
		private readonly ISettingsProvider _settingsProvider;
		public PostgresqlLauncherServiceProvider(ISettingsProvider settingsProvider)
		{
			_settingsProvider = settingsProvider;
		}

		public ICommandLinesFactory GetCommandLinesFactory()
		{
			return new DefaultCommandLinesFactory(_settingsProvider);
		}

		public IPostgresqlInstallExistChecker GetInstallationStatusChecker()
		{
			return new PostgresqlInstallExistChecker();
		}
	}
}