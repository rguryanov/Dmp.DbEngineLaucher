using System;

namespace Dmp.DbEngineLaucher.LockManager
{
	/// <summary>
	/// lock manager based on mutex class
	/// </summary>
	public class MutexLockManager : ILockManager
	{
		/// <summary>
		/// get lock on by path of instalation
		/// </summary>
		/// <param name="enginePath">path for server instalation</param>
		/// <exception cref="System.TimeoutException">can be thrown</exception>
		/// <returns>IDisposable</returns>
		public IDisposable AcquireStartStopLock(string enginePath, int timeout)
		{
			return new SingleGlobalInstance(enginePath, timeout);
		}
	}
}
