using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using Dmp.DbEngineLaucher;
using System.Linq;
using System.Text;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// base class for postgresql launchers
	/// </summary>
	public abstract class PostgresqlLauncher: IPostgresqlLauncher
	{
		protected ISettingsProvider SettingsProvider { get; set; }

		protected PostreSqlEngineConstants EngineConstants { get; }

		protected ICommandLinesFactory CommandLinesFactory { get; set; }
		protected IPostgresqlInstallExistChecker InstallExistChecker { get; set; }

		protected PostgresqlLauncher(ISettingsProvider servicesProvider, IPostgresqlLauncherServiceProvider pgCmdProvider)
		{
			SettingsProvider = servicesProvider;
			EngineConstants = SettingsProvider.EngineConstants;
			CommandLinesFactory = pgCmdProvider.GetCommandLinesFactory();
			InstallExistChecker = pgCmdProvider.GetInstallationStatusChecker();
		}

		/// <summary>
		/// get status of postgres.
		/// </summary>
		/// <returns></returns>
		public async Task<PostgreServerStatus> GetStatusAsync()
		{
			PostgreServerStatus result = new PostgreServerStatus
			{
				EngineIsInstalled = InstallExistChecker
					.EngineBinDirCheck(SettingsProvider.InstallSettings.EngineRootPath)
					.EngineIsInstalled,

				HasMastertDb = InstallExistChecker
					.CheckDataDir(SettingsProvider.InstallSettings.PgDataPath)
					.HasMasterDb,

				EngineProcessStarted = await GetEngineStateAsync(),
			};

			return result;
		}

		/// <summary>
		/// check if engine already started
		/// using pg_ctl util
		/// </summary>
		public virtual async Task<bool> GetEngineStateAsync()
		{
			string dataPath = SettingsProvider.InstallSettings.PgDataPath;
			string path = Path.Combine(SettingsProvider.InstallSettings.EngineRootPath, EngineConstants.PostgreCtlPath);
			string commandLineArgs = CommandLinesFactory.GetStatus(dataPath);
			
			if (!File.Exists(path))
			{
				return false;
			}

			ProcessStartInfo psi = new ProcessStartInfo(path, commandLineArgs)
			{
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardInput = true,
				RedirectStandardError = true
			};

			Process pgCltStatus = new Process()
			{
				StartInfo = psi
			};

			bool result = false;
			try
			{
				await pgCltStatus.StartAndWaitForExitAsync(EngineConstants.StatusProcessWaitTimeout);

				if (pgCltStatus.HasExited == false)
				{
					pgCltStatus.Kill();
				}

				if (pgCltStatus.ExitCode == 0)
				{
					result = true;
				}
			}
			catch (Exception e)
			{
				result = false;
				throw new Exception("Check engine state resulted in error", e);
			}

			return result;
		}

		/// <summary>
		/// start postgresql server
		/// using pg_ctl util
		/// </summary>
		public async Task StartEngineAsync()
		{
			string path = Path.Combine(SettingsProvider.InstallSettings.EngineRootPath, EngineConstants.PostgreCtlPath);
			string dataPath = Path.Combine(SettingsProvider.InstallSettings.PgDataPath,
				SettingsProvider.InstallSettings.PgDataPath);

			if (SettingsProvider.RunSettings.CleanTableSpaceOnStart)
			{
				Debug.WriteLine("clean tablespace");
				try
				{
					CleanPgTblspc();
				}
				catch (Exception e)
				{
					throw new Exception("error while deleting tablespace's files", e);
				}
			}

			string commandLineArgs = CommandLinesFactory.StartPgClt(dataPath);

			ProcessStartInfo psi = new ProcessStartInfo(path, commandLineArgs)
			{
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardInput = true,
				RedirectStandardError = true,
			};

			Process pgCltStart = new Process
			{
				StartInfo = psi
			};
			
			await pgCltStart.StartAndWaitForExitAsync(EngineConstants.StartEngineProcessWaitTimeout);
			if (pgCltStart.HasExited == false)
			{
				pgCltStart.Kill();
				// 1460 - This operation returned because the timeout period expired.
				throw new StartEngineException("Postgre engine can not be started. Exceeded timeout on startup", 1460);
			}

			if (pgCltStart.ExitCode != 0)
			{
				var error = await pgCltStart.StandardError.ReadToEndAsync();
				throw new StartEngineException(error, pgCltStart.ExitCode);
			}
		}

		/// <summary>
		/// stop postgresql server
		/// using pg_ctl util
		/// </summary>
		public async Task StopEngineAsync()
		{
			string dataPath = SettingsProvider.InstallSettings.PgDataPath;
			string path = Path.Combine(SettingsProvider.InstallSettings.EngineRootPath, EngineConstants.PostgreCtlPath);
			// -w not needed, shotdown has that as default
			string commandLineArgs = CommandLinesFactory.StopPgClt(dataPath);

			Debug.WriteLine(path + " " + commandLineArgs);


			ProcessStartInfo psi = new ProcessStartInfo(path, commandLineArgs)
			{
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardInput = true,
				RedirectStandardError = true
			};

			Process pgCltStop = new Process
			{
				StartInfo = psi
			};

			await pgCltStop.StartAndWaitForExitAsync(EngineConstants.StopEngineProcessWaitTimeout);

			pgCltStop.Refresh();

			var exitCode = pgCltStop.ExitCode;

			bool killPostgresOnTimeout = false;
			bool killPostgresOnError = pgCltStop.HasExited && exitCode != 0;

			if (!pgCltStop.HasExited)
			{
				pgCltStop.Kill();
				killPostgresOnTimeout = true;
			}

			if (SettingsProvider.RunSettings.TryKillPostgreProcessOnStopTimeout && killPostgresOnTimeout)
			{
				await KillEngineProcess(dataPath);
			}
			else if (SettingsProvider.RunSettings.TryKillPostgreProcessOnStopError && killPostgresOnError)
			{
				await KillEngineProcess(dataPath);
			}
			else
			{
				if (exitCode != 0)
				{
					var error = await pgCltStop.StandardError.ReadToEndAsync();
					throw new StopEngineException(error, exitCode);
				}
			}

			Debug.WriteLine("pg_ctl stop done");
		}

		private async Task KillEngineProcess(string dataPath)
		{
			Debug.WriteLine("try kill process");
			string fileWithPid = Path.Combine(dataPath, "postmaster.pid");
			try
			{
				if (File.Exists(fileWithPid))
				{

					var timeout = 100;
#if NET40
					await TaskEx.Delay(timeout);
#else
					await Task.Delay(timeout);
#endif
					string pidS;
					using (var reader = File.OpenText(fileWithPid))
					{
						pidS = await reader.ReadLineAsync();
					}

					if (int.TryParse(pidS, out int pid))
					{
						Process postgreProcessByPid = Process.GetProcesses().FirstOrDefault(x => x.Id == pid);

						if (postgreProcessByPid != null)
						{
							Debug.WriteLine("Kill process pid {0}", pid.ToString());
							postgreProcessByPid.Kill();
						}
						else
						{
							// todo ???
							Debug.WriteLine("Process pid {0} not found", pid.ToString());
						}
					}
				}
			}
			catch (Exception e)
			{
				throw new StopEngineException("error when trying kill engine process", e, 1);
			}
		}

		/// <summary>
		/// init db cluster for postgresql server
		/// using initdb util
		/// </summary>
		public async Task InitDbAsync()
		{
			string dataPath = SettingsProvider.InstallSettings.PgDataPath;
			string initFilePath = Path.Combine(SettingsProvider.InstallSettings.EngineRootPath, EngineConstants.InitDbPath);

			if (!Directory.Exists(dataPath))
			{
				Directory.CreateDirectory(dataPath);
			}

			if (Directory.GetFileSystemEntries(dataPath).Length != 0)
			{
				throw new ArgumentException(string.Format("Directory for userDb must be empty. Directory {0} is not empty",dataPath));
			}

			string pwdPath = Path.Combine(SettingsProvider.InstallSettings.InstallPgSqlPath, "Dmp" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".txt");
			
			try
			{
				File.WriteAllText(pwdPath, SettingsProvider.StartupSettings.PostgreDbSettings.Password);

				string commandLineArgs = CommandLinesFactory.InitDb(dataPath, pwdPath);

				Debug.WriteLine(initFilePath + " " + commandLineArgs);

				ProcessStartInfo psi = new ProcessStartInfo(initFilePath, commandLineArgs)
				{
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardInput = true,
					RedirectStandardError = true,
				};

				Process intDbProc = new Process
				{
					StartInfo = psi
				};

				await intDbProc.StartAndWaitForExitAsync(EngineConstants.StopEngineProcessWaitTimeout);

				bool exitByTimeout = false;
				if (!intDbProc.HasExited)
				{
					exitByTimeout = true;
					intDbProc.Kill();
				}

				Debug.WriteLine("initdb ExitCode " + intDbProc.ExitCode);
				if (intDbProc.ExitCode != 0)
				{
					var error = await intDbProc.StandardError.ReadToEndAsync();
					throw new InitDbException(error, intDbProc.ExitCode, exitByTimeout);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("initdb error: {0}", ex);
				throw;
			}
			finally
			{
				if (File.Exists(pwdPath))
				{
					File.Delete(pwdPath);
				}
			}

			Debug.WriteLine("Init finished");
		}

		/// <summary>
		/// clean table space
		/// </summary>
		public virtual void CleanPgTblspc()
		{
			// tablespace dir.
			string path = Path.Combine(SettingsProvider.InstallSettings.PgDataPath, EngineConstants.PgTblspcPath);

			if (Directory.Exists(path))
			{
				string[] dirs = Directory.GetDirectories(path);

				foreach (string dir in dirs)
				{
					bool dirEmpty;

					try
					{
						// can throw IOException.
						Directory.GetFileSystemEntries(dir);
						dirEmpty = false;
					}
					catch (IOException)
					{
						dirEmpty = true;
					}

					if (dirEmpty)
					{
						Directory.Delete(dir);
					}
				}
			}
		}
	}
}
