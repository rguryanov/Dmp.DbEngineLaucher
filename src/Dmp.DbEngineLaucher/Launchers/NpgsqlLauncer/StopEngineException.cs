using System;

namespace Dmp.NpgsqlEngineLaucher
{
#if !NETSTANDARD1_3	
	[Serializable]
#endif
	public class StopEngineException : Exception
	{
		public StopEngineException(int exitCode)
		{
			ExitCode = exitCode;
		}

		public StopEngineException(string message, int exitCode) : base(message)
		{
			ExitCode = exitCode;
		}

		public StopEngineException(string message, Exception inner, int exitCode) : base(message, inner)
		{
			ExitCode = exitCode;
		}

		public int ExitCode { get; private set; }
	}
}