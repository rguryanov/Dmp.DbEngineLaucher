using System;
using System.IO;

namespace Dmp.DbEngineLaucher.Installation
{
	/// <summary>
	/// installator resolver based on saved locally packages
	/// 
	/// dir must contain one or more file from list
	/// postgresql-9.5.5-1-linux-binaries.tar.gz - for x86 linux
	/// postgresql-9.5.5-1-linux-x64-binaries.tar.gz - for x64 linux
	/// postgresql-9.5.5-1-osx-binaries.zip - for x64 osx
	/// postgresql-9.5.5-1-windows-binaries.zip - for x86 windows
	/// postgresql-9.5.5-1-windows-x64-binaries.zip - for x64 windows
	/// </summary>
	public class AllZipSourcesFromDirEngineInstalationResolver : IEngineInstalationResolver
	{
		private readonly string _dir;
		private readonly string _version;

		public AllZipSourcesFromDirEngineInstalationResolver(string dir, string version)
		{
			_dir = dir;
			_version = version;
		}

		protected virtual string GetFilePath(RuntimeOs osPlatform, RuntimeArchitecture architecture)
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

			return Path.Combine(_dir, "postgresql-" + _version + "-" + platformStr + "-" +
			                          (archStr != null ? archStr + "-" : "") +
			                          "binaries" + extention);
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
			var filePath = GetFilePath(osPlatform, architecture);
			return GetLocalInstallerFacory(osPlatform, architecture)(new InstallationSource(File.OpenRead(filePath)))
				.GetIntaller();
		}
	}
}
