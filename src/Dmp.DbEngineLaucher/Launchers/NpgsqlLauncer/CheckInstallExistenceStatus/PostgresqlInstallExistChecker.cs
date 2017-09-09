using System.IO;

namespace Dmp.NpgsqlEngineLaucher
{
	class PostgresqlInstallExistChecker : IPostgresqlInstallExistChecker
	{
		public EngineDirExistCheckResult EngineBinDirCheck(string installEngineRootPath)
		{
			return new EngineDirExistCheckResult(Directory.Exists(installEngineRootPath));
		}

		public DataDirExistCheckResult CheckDataDir(string dataPath)
		{
			string[] masterDbFolders = null;
			if (Directory.Exists(dataPath))
			{
				masterDbFolders = Directory.GetDirectories(dataPath);
			}
			return new DataDirExistCheckResult(masterDbFolders != null);
		}
	}
}