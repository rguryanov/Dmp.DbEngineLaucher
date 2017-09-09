using System.Collections.Generic;
using System.Text;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// <see cref="ICommandLinesFactory"/>
	/// </summary>
	public class DefaultCommandLinesFactory : ICommandLinesFactory
	{
		private readonly ISettingsProvider _settingsProvider;

		public DefaultCommandLinesFactory(ISettingsProvider settingsProvider)
		{
			_settingsProvider = settingsProvider;
		}

		public string GetStatus(string dataPath)
		{
			return $"status -D \"{dataPath}\"{GetCommandLineParams(_settingsProvider.RunSettings.StatusPgCltAdditionalParamters)}";
		}

		public string StartPgClt(string dataPath)
		{
			var host = _settingsProvider.StartupSettings.ServerAddressSettings.Server;
			var addressSpec =
				$"-o \"{(host != null ? $"-h {host} " : "")}-p {_settingsProvider.StartupSettings.ServerAddressSettings.Port}\"";

			return $"start -w -D \"{dataPath}\" {addressSpec}{GetCommandLineParams(_settingsProvider.RunSettings.StartPgCltAdditionalParamters)}";
		}

		public string StopPgClt(string dataPath)
		{
			return $"stop -D \"{dataPath}\" -m f{GetCommandLineParams(_settingsProvider.RunSettings.StopPgCltAdditionalParamters)}";
		}

		public string InitDb(string dataPath, string pwdPath)
		{
			var encoding = _settingsProvider.StartupSettings.PostgreDbSettings.Encoding;
			var commandLineArgs =
				$"-D \"{dataPath}\"{(encoding != null ? $" -E {encoding}" : "")} -U \"{_settingsProvider.StartupSettings.PostgreDbSettings.UserName}\" --pwfile=\"{pwdPath}\" {(string.IsNullOrEmpty(_settingsProvider.StartupSettings.PostgreDbSettings.Locale) ? "" : "--locale " + _settingsProvider.StartupSettings.PostgreDbSettings.Locale)}";

			commandLineArgs += GetCommandLineParams(_settingsProvider.StartupSettings.PostgreDbSettings.AdditionalParameters);
			return commandLineArgs;
		}

		private string GetCommandLineParams(IDictionary<string, string> parameters)
		{
			var sb = new StringBuilder();
			foreach (var additionalParameter in parameters)
			{
				sb.AppendFormat(" {0} {1}", additionalParameter.Key, additionalParameter.Value);
			}

			return sb.ToString();
		}
	}
}