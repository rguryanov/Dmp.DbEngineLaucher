using System;
using Dmp.DbEngineLaucher;

#if NETSTANDARD1_3
using System.Runtime.InteropServices;
#endif

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// <see cref="IOsPlatformResolver"/>
	/// </summary>
	public class OsPlatformResolver: IOsPlatformResolver
	{
		public RuntimeOs GetOsPlatform()
		{
#if NETSTANDARD1_3

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					return RuntimeOs.Windows;
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					return RuntimeOs.OSX;
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
					return RuntimeOs.Linux;

			return RuntimeOs.Unknown;
#else
			PlatformID platformID = Environment.OSVersion.Platform;
			if ((int)platformID == 4 || (int)platformID == 128)
			{
				return RuntimeOs.Linux;
			}

			switch (platformID)
			{
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.Win32NT:
					return RuntimeOs.Windows;
				case PlatformID.WinCE:
					return RuntimeOs.Unknown;
				case PlatformID.Unix:
					return RuntimeOs.Linux;
				case PlatformID.Xbox:
					// not checked
					return RuntimeOs.Linux;
				case PlatformID.MacOSX:
					return RuntimeOs.OSX;
				default:
					return RuntimeOs.Unknown;
			}

#endif
		}
	}
}
