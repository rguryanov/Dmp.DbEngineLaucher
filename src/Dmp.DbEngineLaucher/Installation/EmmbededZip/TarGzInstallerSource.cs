using System.Reflection;
using Dmp.DbEngineLaucher.Installation;

namespace Dmp.DbEngineLaucher
{
	public class TarGzInstallerSource : IInstallerSource
	{
		private readonly IInstallationSource _installationSource;
		public RuntimeOs[] OsPlatforms { get; }
		public RuntimeArchitecture[] Architectures { get; }

		public TarGzInstallerSource(IInstallationSource installationSource, RuntimeOs osPlatform,
			RuntimeArchitecture architecture)
			: this(installationSource, new [] { osPlatform }, new []{ architecture })
		{
			
		}

		public TarGzInstallerSource(IInstallationSource installationSource, RuntimeOs osPlatform,
			RuntimeArchitecture[] architectures)
			: this(installationSource, new[] { osPlatform }, architectures)
		{

		}

		public TarGzInstallerSource(IInstallationSource installationSource, RuntimeOs[] osPlatforms, RuntimeArchitecture[] architectures)
		{
			OsPlatforms = osPlatforms;
			Architectures = architectures;
			_installationSource = installationSource;
		}

		public IEngineInstaller GetIntaller()
		{
			return new FromTarGzEngineInstaller(_installationSource);
		}
	}
}