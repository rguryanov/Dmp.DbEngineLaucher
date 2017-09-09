using System;
using System.IO;
using System.Text;
using Dmp.DbEngineLaucher.Installation.HttpOfficial;
using Xunit;

namespace Dmp.DbEngineLaucher.Tests
{
	public class PostgreRemoteInstallerTests : PostgreInstallerTests
	{
		public PostgreRemoteInstallerTests() : base(new RemoteHttpSourcesEngineInstalationResolver())
		{

		}
	}
}