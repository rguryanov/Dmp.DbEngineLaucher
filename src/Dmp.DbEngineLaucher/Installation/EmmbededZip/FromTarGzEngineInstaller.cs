using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Dmp.DbEngineLaucher.Installation;
using ICSharpCode.SharpZipLib.Tar;

namespace Dmp.DbEngineLaucher
{
	class FromTarGzEngineInstaller : IEngineInstaller
	{
		private readonly IInstallationSource _installSource;
		public FromTarGzEngineInstaller(IInstallationSource installSource)
		{
			_installSource = installSource;
		}

		public async Task Install(string path)
		{
			if (Directory.Exists(path))
				Directory.Delete(path, true);
			Directory.CreateDirectory(path);

			try
			{
				await Task.Factory.StartNew(() =>
				{
					using (var targzStream = _installSource.Get())
					{
						using (var tarStream = new GZipStream(targzStream, CompressionMode.Decompress, true))
						{
							using (var zipArchive = TarArchive.CreateInputTarArchive(tarStream))
							{
								zipArchive.ExtractContents(path);
							}
						}
					}
				}, TaskCreationOptions.LongRunning);
			}
			catch (Exception)
			{
				Directory.Delete(path, false);
				throw;
			}
		}
	}
}