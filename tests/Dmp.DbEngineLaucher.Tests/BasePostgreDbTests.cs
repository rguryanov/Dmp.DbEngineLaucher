using System;
using System.IO;
using System.Text;
using System.Threading;
using Dmp.DatabaseEngineLaucher;
using Dmp.DbEngineLaucher.Installation;
using Dmp.DbEngineLaucher.OsSpecific;
using Dmp.NpgsqlEngineLaucher;
using Xunit;

namespace Dmp.DbEngineLaucher.Tests
{
	class IntergrationTestAttribute : Attribute
	{
		
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
#if DEBUG
	public
#endif
		class BasePostgreDbTests : IDisposable
	{
		private readonly string _tempFolder;
		private readonly string _enginePath;
		private readonly string _userDb;
		private readonly ITempDirectoryScope _tempScope;
		private readonly string _installPackPath;
		private readonly string _installPackVersion;

		public BasePostgreDbTests()
		{
			_installPackPath = Path.Combine(
				new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "binaries");
			_installPackVersion = "9.5.5-1";
			_tempScope = new TempDirectoryProvider().GetTempDirectoryScope();
			_tempFolder = _tempScope.Directory.FullName;

			_enginePath = Path.Combine(_tempFolder, "engine");
			_userDb = Path.Combine(_tempFolder, "userdb");

#if NETCOREAPP1_1
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
		public void Dispose()
		{
			_tempScope?.Dispose();
		}

		[Fact]
		[IntergrationTest]
		public void SimpleDbStartStop()
		{
			PostgreSqlDataBaseEngineLauncher dataBaseEngineService = new PostgreSqlDataBaseEngineLauncher(new SimpleSettingsProvider(new InstallationSettings(_enginePath, _userDb), new PostgreEngineStartupSettings(new ServerArrdessSettings("localhost", 29952), new DbCreationSettings("postgre", "postgre")), new RunSettings(true, true)), new AllZipSourcesFromDirEngineInstalationResolver(_installPackPath, _installPackVersion));

			dataBaseEngineService.StartAsync().Wait();
			dataBaseEngineService.StopAsync().Wait();
		}

		[Fact]
		[IntergrationTest]
		public void MultipleDbStartStop()
		{
			PostgreSqlDataBaseEngineLauncher dataBaseEngineService = new PostgreSqlDataBaseEngineLauncher(new SimpleSettingsProvider(new InstallationSettings(_enginePath, _userDb), new PostgreEngineStartupSettings(new ServerArrdessSettings("localhost", 29952), new DbCreationSettings("postgre", "postgre")), new RunSettings(true, true)), new AllZipSourcesFromDirEngineInstalationResolver(_installPackPath, _installPackVersion));

			var count = 10;
			for (int i = 0; i < count; i++)
			{
				dataBaseEngineService.StartAsync().Wait();
				dataBaseEngineService.StopAsync().Wait();
			}
		}
	}
}
