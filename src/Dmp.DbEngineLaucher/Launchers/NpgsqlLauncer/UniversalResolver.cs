using Dmp.NpgsqlEngineLaucher;
using System;
using Dmp.DbEngineLaucher;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// <see cref="IPostgreLauncherResolver"/>
	/// </summary>
	public class UniversalResolver: IPostgreLauncherResolver
	{
		public IPostgresqlLauncher GetLauncher(ISettingsProvider settingProvider, IPostgresqlLauncherServiceProvider pgCmdProvider, RuntimeOs osPlatform, RuntimeArchitecture architecture)
		{
			if (osPlatform == RuntimeOs.Unknown)
			{
				throw new PlatformNotSupportedException("Your OS is not supported");
			}
			return new UniversalPostgresqlLauncher(settingProvider, pgCmdProvider);
		}
	}
}
