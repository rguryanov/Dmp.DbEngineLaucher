using System.Threading.Tasks;

namespace Dmp.DbEngineLaucher.Installation
{
	/// <summary>
	/// server binaries installer
	/// </summary>
	public interface IEngineInstaller
	{
		/// <summary>
		/// Install database server engine in folder by path dirPath
		/// </summary>
		/// <param name="dirPath">directory path (remote directories not supported)</param>
		Task Install(string dirPath);
	}
}
