using System;

namespace Dmp.NpgsqlEngineLaucher
{
#if !NETSTANDARD1_3
	[Serializable]
#endif
	public class StartEngineException : Exception
	{
		public StartEngineException(int exitCode)
		{
			ExitCode = exitCode;
		}

		public StartEngineException(string message, int exitCode) : base(message)
		{
			ExitCode = exitCode;
		}

		public StartEngineException(string message, Exception inner, int exitCode) : base(message, inner)
		{
			ExitCode = exitCode;
		}

		public int ExitCode { get; private set; }
	}
}