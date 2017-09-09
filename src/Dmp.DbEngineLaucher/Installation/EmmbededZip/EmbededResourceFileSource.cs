using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Dmp.DbEngineLaucher
{
	/// <summary>
	/// EmbededResource based install package stream source
	/// </summary>
	public class EmbededResourceFileSource : IInstallationSource
	{
		private readonly string _resourceName;
		private readonly Assembly _assembly;

		public EmbededResourceFileSource(Assembly assembly, string resourceName)
		{
			_assembly = assembly;
			_resourceName = resourceName;
		}

		public Stream Get()
		{
			return _assembly.GetManifestResourceStream(_resourceName); ;
		}
	}
}
