using Dmp.NpgsqlEngineLaucher;
using Dmp.DbEngineLaucher;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// postgresql launcher resolver
	/// </summary>
	public interface IPostgreLauncherResolver
	{
		/// <summary>
		/// resolve launcher implementation for os and architecture
		/// </summary>
		IPostgresqlLauncher GetLauncher(ISettingsProvider settingProvider, IPostgresqlLauncherServiceProvider pgCmdProvider, RuntimeOs osPlatform, RuntimeArchitecture architecture);
	}
}
