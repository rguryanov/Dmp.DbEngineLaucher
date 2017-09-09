using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dmp.DatabaseEngineLaucher;
using Dmp.DbEngineLaucher.Installation;
using Dmp.DbEngineLaucher.Installation.HttpOfficial;
using Dmp.DbEngineLaucher.OsSpecific;
using Dmp.NpgsqlEngineLaucher;

namespace Dmp.DbEngineLaucher.ConsoleExampleApp.Net40
{
	class Program
	{
		static void Main(string[] args)
		{
			var tempFolder = Path.Combine(Path.GetTempPath(), "Dmp" + Path.GetRandomFileName());
			var enginePath = Path.Combine(tempFolder, "engine");
			var userDb = Path.Combine(tempFolder, "userdb");

			try
			{
				IDatabaseService dataBaseEngineService = new PostgreSqlDataBaseEngineLauncher(
					new SimpleSettingsProvider(new InstallationSettings(enginePath, userDb),
						new PostgreEngineStartupSettings(new ServerArrdessSettings("localhost", 29952),
							new DbCreationSettings("postgres", "postgres")),
						new RunSettings(true, true)), new RemoteHttpSourcesEngineInstalationResolver(), new RuntimePlatformResolver(),
					new UniversalResolver());

				dataBaseEngineService.StartAsync().Wait();

				dataBaseEngineService.StopAsync().Wait();
			}
			finally
			{
				Directory.Delete(tempFolder, true);
			}
		}
	}
}
