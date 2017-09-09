namespace Dmp.NpgsqlEngineLaucher
{
	public interface IPostgresqlInstallExistChecker
	{
		EngineDirExistCheckResult EngineBinDirCheck(string installSettingsEngineRootPath);
		DataDirExistCheckResult CheckDataDir(string dataPath);
	}
}