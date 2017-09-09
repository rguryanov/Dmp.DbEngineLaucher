namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// service provider for luncher implementations
	/// </summary>
	public interface IPostgresqlLauncherServiceProvider
	{
		ICommandLinesFactory GetCommandLinesFactory();
		IPostgresqlInstallExistChecker GetInstallationStatusChecker();
	}
}