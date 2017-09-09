using System.Collections.Generic;
using System.Linq;
using Dmp.DbEngineLaucher.Installation;

namespace Dmp.DbEngineLaucher
{
	public class EngineInstalationResolver : IEngineInstalationResolver
	{
		protected IEnumerable<IInstallerSource> InstallSources;
		public EngineInstalationResolver(IEnumerable<IInstallerSource> installSources)
		{
			InstallSources = installSources;
		}

		public IEngineInstaller GetIntaller(RuntimeOs osPlatform, RuntimeArchitecture architecture)
		{
			var installerZipSource = InstallSources.FirstOrDefault(x => x.OsPlatforms.Any(os => os == osPlatform) && x.Architectures.Any(a => a == architecture));
			return installerZipSource?.GetIntaller();
		}
	}
}
