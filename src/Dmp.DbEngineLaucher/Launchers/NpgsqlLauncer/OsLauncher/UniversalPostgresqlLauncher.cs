namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// postgresql server launcher
	/// </summary>
	public class UniversalPostgresqlLauncher : PostgresqlLauncher
	{
		public UniversalPostgresqlLauncher(ISettingsProvider settingsProvider, IPostgresqlLauncherServiceProvider pgCmdProvider) : base(settingsProvider, pgCmdProvider)
		{
		}
	}
}
