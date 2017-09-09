using System.Reflection;
using Dmp.DbEngineLaucher.Installation;

namespace Dmp.DbEngineLaucher
{
	public class ZipInstallerSource : IInstallerSource
	{
		private readonly IInstallationSource _installationSource;
		public RuntimeOs[] OsPlatforms { get; }
		public RuntimeArchitecture[] Architectures { get; }

		public ZipInstallerSource(IInstallationSource installationSource, RuntimeOs osPlatform,
			RuntimeArchitecture architecture)
			: this(installationSource, new [] { osPlatform }, new []{ architecture })
		{

		}

		public ZipInstallerSource(IInstallationSource installationSource, RuntimeOs osPlatform,
			RuntimeArchitecture[] architectures)
			: this(installationSource, new[] { osPlatform }, architectures)
		{

		}

		public ZipInstallerSource(IInstallationSource installationSource, RuntimeOs[] osPlatforms, RuntimeArchitecture[] architectures)
		{
			OsPlatforms = osPlatforms;
			Architectures = architectures;
			_installationSource = installationSource;
		}

		public IEngineInstaller GetIntaller()
		{
			return new FromZipEngineInstaller(_installationSource);
		}
	}
}