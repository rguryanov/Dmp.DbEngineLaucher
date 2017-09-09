using System.Threading.Tasks;

namespace Dmp.DatabaseEngineLaucher
{
	/// <summary>
	/// Database service
	/// </summary>
	public interface IDatabaseService
	{
		/// <summary>
		/// start server (many intermediate operations which accessible in implementations)
		/// </summary>
		Task StartAsync();

		/// <summary>
		/// stop server (many intermediate operations which accessible in implementations)
		/// </summary>
		Task StopAsync();
	}
}
