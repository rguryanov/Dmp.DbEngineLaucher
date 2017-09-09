using System;
using System.Threading;

namespace Dmp.DbEngineLaucher.LockManager
{
	/// <summary>
	/// single instance based on mutex
	/// can throw TimeoutException
	/// </summary>
	public class SingleGlobalInstance : IDisposable
	{
		public bool HasHandle = false;
		Mutex _mutex;

		private void InitMutex(string name)
		{
			string id = name;
			string mutexId = string.Format("Global\\{{{0}}}", name);

			_mutex = new Mutex(false, mutexId);
		}

		public SingleGlobalInstance(string name, int timeOut = -1)
		{
			InitMutex(name);
			try
			{
				if (timeOut < 0)
					HasHandle = _mutex.WaitOne(Timeout.Infinite);
				else
					HasHandle = _mutex.WaitOne(timeOut);

				if (HasHandle == false)
					throw new TimeoutException("Timeout waiting for exclusive access on SingleInstance");
			}
			catch (AbandonedMutexException)
			{
				HasHandle = true;
			}
		}
		
		public void Dispose()
		{
			if (_mutex != null)
			{
				if (HasHandle)
					_mutex.ReleaseMutex();
				_mutex.Dispose();
			}
		}
	}
}
