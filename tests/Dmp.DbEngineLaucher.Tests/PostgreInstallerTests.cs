using System;
using System.Text;
using Dmp.DbEngineLaucher.Installation;
using Xunit;

namespace Dmp.DbEngineLaucher.Tests
{
	public abstract class PostgreInstallerTests : IDisposable
	{
		private readonly string _tempFolder;
		private readonly ITempDirectoryScope _tempScope;
		protected IEngineInstalationResolver EngineInstalationResolver;

		protected PostgreInstallerTests(IEngineInstalationResolver engineInstalationResolver)
		{
#if NETCOREAPP1_1
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

			EngineInstalationResolver = engineInstalationResolver;

			_tempScope = new TempDirectoryProvider().GetTempDirectoryScope();
			_tempFolder = _tempScope.Directory.FullName;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
		public void Dispose()
		{
			_tempScope?.Dispose();
		}

		[Fact]
		[IntergrationTest]
		public void LinuxX86EmbeddedInstallTest()
		{
			var installer = EngineInstalationResolver.GetIntaller(RuntimeOs.Linux, RuntimeArchitecture.X86);
			installer.Install(_tempFolder).Wait();
		}

		[Fact]
		[IntergrationTest]
		public void LinuxX64EmbeddeInstallTest()
		{
			var installer = EngineInstalationResolver.GetIntaller(RuntimeOs.Linux, RuntimeArchitecture.X64);
			installer.Install(_tempFolder).Wait();
		}

		[Fact]
		[IntergrationTest]
		public void MacOsX86EmbeddedInstallTest()
		{
			var installer = EngineInstalationResolver.GetIntaller(RuntimeOs.OSX, RuntimeArchitecture.X86);
			installer.Install(_tempFolder).Wait();
		}

		[Fact]
		[IntergrationTest]
		public void MacOsX64EmbeddedInstallTest()
		{
			var installer = EngineInstalationResolver.GetIntaller(RuntimeOs.OSX, RuntimeArchitecture.X64);
			installer.Install(_tempFolder).Wait();
		}

		[Fact]
		[IntergrationTest]
		public void WindowX86EmbeddedInstallTest()
		{
			var installer = EngineInstalationResolver.GetIntaller(RuntimeOs.Windows, RuntimeArchitecture.X86);
			installer.Install(_tempFolder).Wait();
		}


		[Fact]
		[IntergrationTest]
		public void WindowX64EmbeddedInstallTest()
		{
			var installer = EngineInstalationResolver.GetIntaller(RuntimeOs.Windows, RuntimeArchitecture.X64);
			installer.Install(_tempFolder).Wait();
		}
	}
}