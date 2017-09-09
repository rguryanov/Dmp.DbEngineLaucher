using Dmp.DbEngineLaucher;

namespace Dmp.NpgsqlEngineLaucher
{
	public class PostreSqlEngineConstants
	{
		public int StatusProcessWaitTimeout { get; set; }
		public int StartEngineProcessWaitTimeout { get; set; }
		public int StopEngineProcessWaitTimeout { get; set; }
		public int InitDbWaitTimeout { get; set; }
		public string PostgreCtlPath { get; set; }
		public string InitDbPath { get; set; }
		public string PgTblspcPath { get; set; }

		public static PostreSqlEngineConstants GetDefault()
		{
			return new PostreSqlEngineConstants
			{
				StatusProcessWaitTimeout = 60000,
				StartEngineProcessWaitTimeout = 60000,
				StopEngineProcessWaitTimeout = 60000,
				InitDbWaitTimeout = 60000,
				PostgreCtlPath = @"bin\pg_ctl",
				InitDbPath = @"bin\initdb",
				PgTblspcPath = "pg_tblspc",
			};
		}
	}
}
