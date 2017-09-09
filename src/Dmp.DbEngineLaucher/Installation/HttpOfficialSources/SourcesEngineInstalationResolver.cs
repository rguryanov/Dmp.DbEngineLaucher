using System;
using System.Reflection;
using System.Collections.Generic;
using Dmp.DbEngineLaucher;
using Dmp.DbEngineLaucher.Installation;

namespace Dmp.DbEngineLaucher.Installation.HttpOfficial
{
	public class RemoteHttpSourcesEngineInstalationResolver : IEngineInstalationResolver
	{
		protected string UrlEndPoint;
		protected string Version;
		private readonly int? _webRequestTimeout;

		public RemoteHttpSourcesEngineInstalationResolver(string urlEndPoint = "https://get.enterprisedb.com/postgresql/", string version = "9.6.2-3", int? webRequestTimeout = null)
		{
			// example of url
			// https://get.enterprisedb.com/postgresql/postgresql-9.6.2-3-linux-x64-binaries.tar.gz
			UrlEndPoint = urlEndPoint;
			Version = version;
			_webRequestTimeout = webRequestTimeout;
		}

		protected virtual string GetUrl(RuntimeOs osPlatform, RuntimeArchitecture architecture)
		{
			string platformStr;
			string archStr;
			string extention;
			switch (osPlatform)
			{
				case RuntimeOs.Linux:
					platformStr = "linux";
					archStr = architecture == RuntimeArchitecture.X64 || architecture == RuntimeArchitecture.Arm64 ? "x64" : null;
					extention = ".tar.gz";
					break;
				case RuntimeOs.OSX:
					platformStr = "osx";
					archStr = null;
					extention = ".zip";
					break;
				case RuntimeOs.Windows:
					platformStr = "windows";
					archStr = architecture == RuntimeArchitecture.X64 || architecture == RuntimeArchitecture.Arm64 ? "x64" : null;
					extention = ".zip";
					break;
				default:
					throw new NotSupportedException();
			}

			return UrlEndPoint + "postgresql-" + Version + "-" + platformStr + "-" + (archStr != null ?  archStr + "-" : "")  +
			       "binaries" + extention;
		}

		protected virtual Func<IInstallationSource, IInstallerSource> GetLocalInstallerFacory(RuntimeOs osPlatform, RuntimeArchitecture architecture)
		{
			switch (osPlatform)
			{
				case RuntimeOs.Linux:
					return installationSource => new TarGzInstallerSource(installationSource, osPlatform, architecture);
				case RuntimeOs.OSX:
				case RuntimeOs.Windows:
					return installationSource => new ZipInstallerSource(installationSource, osPlatform, architecture);
				default:
					throw new NotSupportedException();
			}
		}

		public IEngineInstaller GetIntaller(RuntimeOs osPlatform, RuntimeArchitecture architecture)
		{
			var url = GetUrl(osPlatform, architecture);
			return new RemoteInstaller(url, GetLocalInstallerFacory(osPlatform, architecture), _webRequestTimeout);
		}
	}
}
