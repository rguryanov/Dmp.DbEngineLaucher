using System.Reflection;
using Dmp.DbEngineLaucher.Installation;

namespace Dmp.DbEngineLaucher
{
	public class ZipInstallerSource : IInstallerSource
	{
		private readonly IInstallationSource _installationSource;
		private readonly ITempDirectoryProvider _tempDirectoryProvider;

		public RuntimeOs[] OsPlatforms { get; }
		public RuntimeArchitecture[] Architectures { get; }

		public ZipInstallerSource(IInstallationSource installationSource, ITempDirectoryProvider tempDirectoryProvider, RuntimeOs osPlatform,
			RuntimeArchitecture architecture)
			: this(installationSource, tempDirectoryProvider, new [] { osPlatform }, new []{ architecture })
		{

		}

		public ZipInstallerSource(IInstallationSource installationSource, ITempDirectoryProvider tempDirectoryProvider, RuntimeOs osPlatform,
			RuntimeArchitecture[] architectures)
			: this(installationSource, tempDirectoryProvider, new[] { osPlatform }, architectures)
		{

		}

		public ZipInstallerSource(IInstallationSource installationSource, ITempDirectoryProvider tempDirectoryProvider, RuntimeOs[] osPlatforms, RuntimeArchitecture[] architectures)
		{
			OsPlatforms = osPlatforms;
			Architectures = architectures;
			_installationSource = installationSource;
			_tempDirectoryProvider = tempDirectoryProvider;
		}

		public IEngineInstaller GetIntaller()
		{
			return new FromZipEngineInstaller(_installationSource, _tempDirectoryProvider);
		}
	}
}