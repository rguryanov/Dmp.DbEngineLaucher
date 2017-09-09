namespace Dmp.NpgsqlEngineLaucher
{
	public class EngineDirExistCheckResult
	{
		public bool EngineIsInstalled { get; set; }

		public EngineDirExistCheckResult(bool engineIsInstalled)
		{
			EngineIsInstalled = engineIsInstalled;
		}
	}
}