using System.IO;

namespace Dmp.DbEngineLaucher
{
	/// <summary>
	///  stream based install package stream source
	/// </summary>
	public class InstallationSource: IInstallationSource
	{
		private readonly Stream _stream;

		public InstallationSource(Stream stream)
		{
			_stream = stream;
		}

		public Stream Get()
		{
			return _stream;
		}
	}
}