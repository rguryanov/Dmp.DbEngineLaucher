using System;
using System.IO;
using System.Net;
#if !NET40
using System.Net.Http;
#endif
using System.Threading.Tasks;

namespace Dmp.DbEngineLaucher.Installation.HttpOfficial
{
#if NET40 || NET45
	[Serializable]
#endif
	public class RemoteSourceForbidenException : Exception
	{
		public RemoteSourceForbidenException()
		{
		}

		public RemoteSourceForbidenException(string message) : base(message)
		{
		}

		public RemoteSourceForbidenException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	/// <summary>
	/// installer from remote source
	/// (download remote sources and install)
	/// </summary>
	public class RemoteInstaller: IEngineInstaller
	{
		private readonly string _url;
		private readonly Func<IInstallationSource, IInstallerSource> _localInstallFactory;
		private readonly int? _webRequestTimeout;
		private readonly int _bufferSize = 4086;

		public RemoteInstaller(string url, Func<IInstallationSource, IInstallerSource> localInstallFactory, int? webRequestTimeout)
		{
			_url = url;
			_localInstallFactory = localInstallFactory;
			_webRequestTimeout = webRequestTimeout;
		}

		private async Task DownloadAndCopyToStream(Stream tempStream)
		{
#if NET40

			try
			{
				var webRequest = WebRequest.Create(_url);
				if(_webRequestTimeout != null){
					webRequest.Timeout = _webRequestTimeout.Value;
				}
				webRequest.Method = "GET";

				using (var response = await webRequest.GetResponseAsync())
				{
					using (var installStream = response.GetResponseStream())
					{
						await installStream.CopyToAsync(tempStream, _bufferSize);
					}
				}
			}
			catch (WebException we)
			{
				throw new RemoteSourceForbidenException($"download from {_url} failed - {we.Message}", we);
			}

#else
			using (var httpClient = new HttpClient())
			{
				if (_webRequestTimeout != null)
				{
					httpClient.Timeout = TimeSpan.FromMilliseconds(_webRequestTimeout.Value);
				}
				var response = httpClient.GetAsync(_url);
				if (!response.Result.IsSuccessStatusCode)
				{
					throw new RemoteSourceForbidenException($"{_url} not success statuscod - {response.Result.StatusCode}");
				}

				try
				{
					using (var installStream = await response.Result.Content.ReadAsStreamAsync())
					{
						await installStream.CopyToAsync(tempStream, _bufferSize);
					}
				}
				catch (Exception ex)
				{
					throw new RemoteSourceForbidenException($"download from {_url} failed - {ex.Message}", ex);
				}
			}
#endif
		}

		public async Task Install(string dirPath)
		{
			using (var tempScope = new TempDirectoryProvider().GetTempDirectoryScope())
			{
				string fileName = Path.Combine(tempScope.Directory.FullName, "Dmp" + Path.GetRandomFileName());
				using (var fileStream = File.Open(fileName, FileMode.CreateNew, FileAccess.ReadWrite))
				{
					await DownloadAndCopyToStream(fileStream);
					await fileStream.FlushAsync();
					fileStream.Position = 0;
					var localInstaller = _localInstallFactory(new InstallationSource(fileStream)).GetIntaller();
					await localInstaller.Install(dirPath);
				}
			}
		}
	}
}