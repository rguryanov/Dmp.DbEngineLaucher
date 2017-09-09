using System.IO;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// installation settings
	/// engine binaries location and cluster data directory
	/// </summary>
	public class InstallationSettings
	{
		public string InstallPgSqlPath { get; set; }
		public string EngineRootPath => Path.Combine(InstallPgSqlPath, "pgsql");
		public string PgDataPath { get; set; }

		public InstallationSettings(string installPgSqlPath, string pgDataPath)
		{
			InstallPgSqlPath = installPgSqlPath;
			PgDataPath = pgDataPath;
		}
	}
}
