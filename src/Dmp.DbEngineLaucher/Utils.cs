using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Dmp.DbEngineLaucher
{
    public static class Utils
    {
		public static Assembly GetAssemblyForType<T>()
		{
			return GetAssemblyForType(typeof(T));
		}

		public static Assembly GetAssemblyForType(Type type)
		{
#if NETSTANDARD1_3
			var assembly = type.GetTypeInfo().Assembly;
#elif NET40 || NET45
			var assembly = type.Assembly;
#endif
			return assembly;
		}

		/// <summary>
		/// Waits asynchronously for the process to exit.
		/// </summary>
		/// <param name="process">The process to wait for cancellation.</param>
		/// <param name="cancellationToken">A cancellation token. If invoked, the task will return 
		/// immediately as canceled.</param>
		/// <returns>A Task representing waiting for the process to end.</returns>
		public static Task StartAndWaitForExitAsync(this Process process,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var tcs = new TaskCompletionSource<object>();
			process.EnableRaisingEvents = true;
			process.Exited += (sender, args) =>
			{
				tcs.TrySetResult(null);
			};
			if (cancellationToken != default(CancellationToken))
				cancellationToken.Register(tcs.SetCanceled);

			process.Start();

			return tcs.Task;
		}

	    /// <summary>
	    /// Waits asynchronously for the process to exit.
	    /// </summary>
	    /// <param name="process">The process to wait for cancellation.</param>
	    /// <param name="waitTimeout"></param>
	    /// <returns>A Task representing waiting for the process to end.</returns>
	    public static Task StartAndWaitForExitAsync(this Process process,
			TimeSpan waitTimeout)
		{
			using (CancellationTokenSource source = new CancellationTokenSource())
			{
				source.CancelAfter(waitTimeout);
				return process.StartAndWaitForExitAsync(source.Token);
			}
		}

	    /// <summary>
	    /// Waits asynchronously for the process to exit.
	    /// </summary>
	    /// <param name="process">The process to wait for cancellation.</param>
	    /// <param name="waitTimeout"></param>
	    /// <returns>A Task representing waiting for the process to end.</returns>
	    public static Task StartAndWaitForExitAsync(this Process process,
			int waitTimeout)
		{
			using (CancellationTokenSource source = new CancellationTokenSource())
			{
				source.CancelAfter(waitTimeout);
				return process.StartAndWaitForExitAsync(source.Token);
			}
		}

#if NET40
		public static void UnZip(string zipFile, string folderPath)
		{
			if (!File.Exists(zipFile))
				throw new FileNotFoundException();

			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			if (Path.GetExtension(zipFile) != ".zip")
			{
				throw new NotSupportedException("filename must have .zip extension");
			}

			//Shell32.Shell objShell = new Shell32.Shell();
			Type t = Type.GetTypeFromProgID("Shell.Application");
			dynamic objShell = Activator.CreateInstance(t);
			var destinationFolder = objShell.NameSpace(folderPath);
			var sourceFile = objShell.NameSpace(zipFile);

			foreach (var file in sourceFile.Items())
			{
				destinationFolder.CopyHere(file, 4 | 16);
			}
		}
#endif
	}
}
