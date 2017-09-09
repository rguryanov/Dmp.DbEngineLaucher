using Dmp.NpgsqlEngineLaucher;

namespace Dmp.DbEngineLaucher.OsSpecific
{

	public class RuntimePlatformResolver : IRuntimePlatformResolver
	{
		private readonly IArchitectureResolver _archResolver;
		private readonly IOsPlatformResolver _osResolver;

		public RuntimePlatformResolver()
		{
			_archResolver = new OsArchitectureResolver();
			_osResolver = new OsPlatformResolver();
		}

		public RuntimeOs GetOsPlatform()
		{
			return _osResolver.GetOsPlatform();
		}

		public RuntimeArchitecture GetRuntimeArchitecture()
		{
			return _archResolver.GetRuntimeArchitecture();
		}
	}
}
