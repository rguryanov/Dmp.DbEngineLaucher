using System.Threading.Tasks;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// postgresql server launcher contract
	/// </summary>
	public interface IPostgresqlLauncher
	{
		/// <summary>
		/// get postgresql status
		/// </summary>
		Task<PostgreServerStatus> GetStatusAsync();

		/// <summary>
		/// init db cluster
		/// </summary>
		Task InitDbAsync();

		/// <summary>
		/// start server
		/// </summary>
		Task StartEngineAsync();

		/// <summary>
		/// stop server
		/// </summary>
		Task StopEngineAsync();
	}
}
