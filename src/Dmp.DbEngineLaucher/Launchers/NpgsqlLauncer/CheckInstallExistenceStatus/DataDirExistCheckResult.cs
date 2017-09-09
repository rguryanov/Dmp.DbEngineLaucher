namespace Dmp.NpgsqlEngineLaucher
{
	public class DataDirExistCheckResult
	{
		public bool HasMasterDb { get; set; }

		public DataDirExistCheckResult(bool hasMasterDb)
		{
			HasMasterDb = hasMasterDb;
		}
	}
}