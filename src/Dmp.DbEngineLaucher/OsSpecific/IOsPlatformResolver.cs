using Dmp.DbEngineLaucher;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// resolver of system os
	/// </summary>
	public interface IOsPlatformResolver
	{
		/// <summary>
		/// resolve system os
		/// </summary>
		RuntimeOs GetOsPlatform();
	}
}
