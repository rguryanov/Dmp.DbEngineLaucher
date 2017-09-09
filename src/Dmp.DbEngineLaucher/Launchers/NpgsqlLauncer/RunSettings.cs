using System.Collections;
using System.Collections.Generic;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// run and stop server settings (for cmd configurations and actions on errors/timeouts)
	/// </summary>
	public class RunSettings
	{
		public bool TryKillPostgreProcessOnStopError { get; private set; }
		public bool TryKillPostgreProcessOnStopTimeout { get; private set; }
		public bool CleanTableSpaceOnStart { get; set; }
		public IDictionary<string, string> StatusPgCltAdditionalParamters { get; set; }
		public IDictionary<string, string> StartPgCltAdditionalParamters { get; set; }
		public IDictionary<string, string> StopPgCltAdditionalParamters { get; set; }

		public RunSettings(bool tryKillPostgreProcessOnStopError, bool tryKillPostgreProcessOnStopTimeout)
		{
			TryKillPostgreProcessOnStopError = tryKillPostgreProcessOnStopError;
			TryKillPostgreProcessOnStopTimeout = tryKillPostgreProcessOnStopTimeout;
			CleanTableSpaceOnStart = false;
			StartPgCltAdditionalParamters = new Dictionary<string, string>();
			StopPgCltAdditionalParamters = new Dictionary<string, string>();
			StatusPgCltAdditionalParamters = new Dictionary<string, string>();
		}
	}
}