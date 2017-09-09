using System;

namespace Dmp.NpgsqlEngineLaucher
{
#if !NETSTANDARD1_3
	[Serializable]
#endif
	public class InitDbException : Exception
	{
		public InitDbException(int exitCode, bool exitByTimeout)
		{
			ExitCode = exitCode;
			ExitByTimeout = exitByTimeout;
		}

		public InitDbException(string message, int exitCode, bool exitByTimeout) : base(message)
		{
			ExitCode = exitCode;
			ExitByTimeout = exitByTimeout;
		}

		public InitDbException(string message, Exception inner, int exitCode, bool exitByTimeout) : base(message, inner)
		{
			ExitCode = exitCode;
			ExitByTimeout = exitByTimeout;
		}

		public int ExitCode { get; private set; }

		public bool ExitByTimeout { get; private set; }
	}
}