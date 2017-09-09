using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dmp.DbEngineLaucher
{
	public interface ITempDirectoryScope : IDisposable
	{
		DirectoryInfo Directory { get; }
	}

	public class TempDirectoryScope: ITempDirectoryScope, IDisposable
	{
		private DirectoryInfo _directory;
		public DirectoryInfo Directory => _directory ?? (_directory = GetTempDirectory());

		private DirectoryInfo GetTempDirectory()
		{
			return System.IO.Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Dmp" + Path.GetRandomFileName()));
		}

		public void Dispose()
		{
			if (_directory != null)
			{
				const int tryingCOunt = 5;
				for (var i = 1; i <= tryingCOunt; i++)
				{
					try
					{
						System.IO.Directory.Delete(Directory.FullName, true);
					}
					catch (DirectoryNotFoundException)
					{
						return;  // good!
					}
					catch (Exception)
					{ 
						// do not want add dependency on another assembly and Thread won't be implemented for uwp before netstandard 2.0
#if NETSTANDARD1_3
						Task.Delay(50).Wait();
#else
						Thread.Sleep(50);
#endif
						continue;
					}
					return;
				}
			}
		}
	}

	public interface ITempDirectoryProvider
	{
		ITempDirectoryScope GetTempDirectoryScope();
	}

    public class TempDirectoryProvider: ITempDirectoryProvider
	{
	    public ITempDirectoryScope GetTempDirectoryScope()
	    {
		    return new TempDirectoryScope();
	    }
    }
}
