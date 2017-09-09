using System;
using System.Collections.Generic;
using System.Text;

namespace Dmp.DbEngineLaucher
{
	/// <summary>
	/// Supported operating systems.
	/// </summary>
	public enum RuntimeOs
	{
		Unknown,
		/// <summary>
		/// Linux operating systems.
		/// </summary>
		Linux,

		/// <summary>
		/// Versions of Windows.
		/// </summary>
		Windows,

		/// <summary>
		/// Unknown operating system.
		/// </summary>
		OSX,
	}
}
