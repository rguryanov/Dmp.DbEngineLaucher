using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Dmp.DbEngineLaucher.Installation;

namespace Dmp.DbEngineLaucher
{
	/// <summary>
	/// install package stream source
	/// </summary>
	public interface IInstallationSource
	{
		Stream Get();
	}

	/// <summary>
	/// engine installer source service provider and description
	/// </summary>
	public interface IInstallerSource
	{
		RuntimeOs[] OsPlatforms { get; }
		RuntimeArchitecture[] Architectures { get; }
		IEngineInstaller GetIntaller();
	}
}
