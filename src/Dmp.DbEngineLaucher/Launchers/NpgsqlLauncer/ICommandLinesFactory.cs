namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// factory for cmd commands
	/// for pg_ctl and initdb utils
	/// </summary>
	public interface ICommandLinesFactory
	{
		string GetStatus(string dataPath);
		string StartPgClt(string dataPath);
		string StopPgClt(string dataPath);
		string InitDb(string dataPath, string pwdPath);
	}
}