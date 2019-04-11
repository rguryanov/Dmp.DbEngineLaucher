using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Dmp.DbEngineLaucher.Installation;

namespace Dmp.DbEngineLaucher
{
	class FromZipEngineInstaller : IEngineInstaller
	{
		private readonly IInstallationSource _installSource;
		private readonly ITempDirectoryProvider _tempDirectoryProvider;

		public FromZipEngineInstaller(IInstallationSource installSource, ITempDirectoryProvider tempDirectoryProvider)
		{
			_installSource = installSource;
			_tempDirectoryProvider = tempDirectoryProvider;
		}

		public async Task Install(string path)
		{
			if (Directory.Exists(path))
				Directory.Delete(path, true);
			Directory.CreateDirectory(path);

			using (var zipStream = _installSource.Get())
			{
				await UnZip(path, zipStream);
			}
		}

		protected async Task UnZip(string path, Stream zipStream)
		{
#if NET40
			// create temp dir
			using (var tempScope = _tempDirectoryProvider.GetTempDirectoryScope())
			{
				string fileName = Path.ChangeExtension(Path.Combine(tempScope.Directory.FullName, "Dmp" + Path.GetRandomFileName()), "zip");
				using (var fileStream = File.Open(fileName, FileMode.CreateNew))
				{
					await zipStream.CopyToAsync(fileStream);
				}
				await Task.Factory.StartNew(() => Utils.UnZip(fileName, path), TaskCreationOptions.LongRunning);
			}

#else
			await Task.Factory.StartNew(() =>
			{
				using (var zipArchive = new ZipArchive(zipStream))
				{
					zipArchive.ExtractToDirectory(path);
				}
			}, TaskCreationOptions.LongRunning);
#endif
		}
	}
}
