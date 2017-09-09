using System;

namespace Dmp.DbEngineLaucher.LockManager
{
	/// <summary>
	/// interface for system mutex based locking
	/// </summary>
	public interface ILockManager
	{
		/// <summary>
		/// Acquire start and stop server lock for server instalation with path enginePath
		/// </summary>
		/// <param name="enginePath">server instalation path, use as key for locking</param>
		/// <param name="timeout">timeout in milliseconds, use -1 for infinity</param>
		/// <returns></returns>
		IDisposable AcquireStartStopLock(string enginePath, int timeout);
	}
}
