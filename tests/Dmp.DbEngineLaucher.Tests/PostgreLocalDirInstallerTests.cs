using System.IO;
using System.Threading;
using Dmp.DbEngineLaucher.Installation;

namespace Dmp.DbEngineLaucher.Tests
{
	// todo
#if DEBUG
		public
#endif
	class PostgreLocalDirInstallerTests : PostgreInstallerTests
	{
		public PostgreLocalDirInstallerTests() : base(new AllZipSourcesFromDirEngineInstalationResolver(
			Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "binaries"),
			"9.5.5-1"))
		{

		}
	}
}
