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
	/// <see cref="IArchitectureResolver"/>
	/// </summary>
	public class OsArchitectureResolver : IArchitectureResolver
	{
		public RuntimeArchitecture GetRuntimeArchitecture()
		{
#if NETSTANDARD1_3

			var currentArch = RuntimeInformation.OSArchitecture;
			switch (currentArch)
			{
				case Architecture.X86:
					return RuntimeArchitecture.X86;
				case Architecture.X64:
					return RuntimeArchitecture.X64;
				case Architecture.Arm:
					return RuntimeArchitecture.Arm;
				case Architecture.Arm64:
					return RuntimeArchitecture.Arm64;
				default:
					throw new NotSupportedException("Current os architecture not supported");
			}
#else
			var arch = GetProcessorArchitecture();

			var isArm = false;
#if NET45
			if (arch == ProcessorArchitecture.Arm)
			{
				isArm = true;
			}
#endif

			return isArm 
				? Environment.Is64BitOperatingSystem ? RuntimeArchitecture.Arm64 : RuntimeArchitecture.Arm 
				: Environment.Is64BitOperatingSystem ? RuntimeArchitecture.X64 : RuntimeArchitecture.X86; 
#endif
		}

#if NET40 || NET45
		[DllImport("kernel32.dll")]
		private static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

		private const int PROCESSOR_ARCHITECTURE_AMD64 = 9;
		private const int PROCESSOR_ARCHITECTURE_IA64 = 6;
		private const int PROCESSOR_ARCHITECTURE_INTEL = 0;

		[StructLayout(LayoutKind.Sequential)]
		private struct SYSTEM_INFO
		{
			public short wProcessorArchitecture;
			public short wReserved;
			public int dwPageSize;
			public IntPtr lpMinimumApplicationAddress;
			public IntPtr lpMaximumApplicationAddress;
			public IntPtr dwActiveProcessorMask;
			public int dwNumberOfProcessors;
			public int dwProcessorType;
			public int dwAllocationGranularity;
			public short wProcessorLevel;
			public short wProcessorRevision;
		}

		public static ProcessorArchitecture GetProcessorArchitecture()
		{
			SYSTEM_INFO si = new SYSTEM_INFO();
			GetNativeSystemInfo(ref si);
			switch (si.wProcessorArchitecture)
			{
				case PROCESSOR_ARCHITECTURE_AMD64:
					return ProcessorArchitecture.Amd64;

				case PROCESSOR_ARCHITECTURE_IA64:
					return ProcessorArchitecture.IA64;

				case PROCESSOR_ARCHITECTURE_INTEL:
					return ProcessorArchitecture.X86;

				default:
					return ProcessorArchitecture.None; // that's weird :-)
			}
		}
#endif

	}
}
