using Dmp.DbEngineLaucher;
using System;
#if NET40 || NET45
using System.Reflection;
using System.Runtime.InteropServices;
#endif
#if NETSTANDARD1_3
using System.Runtime.InteropServices;
#endif

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// resolver of system architecture
	/// </summary>
	public interface IArchitectureResolver
	{
		/// <summary>
		/// resolve system architecture
		/// </summary>
		RuntimeArchitecture GetRuntimeArchitecture();
	}
}
