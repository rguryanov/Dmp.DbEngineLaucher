namespace Dmp.DbEngineLaucher.Installation
{
	/// <summary>
	/// resolver for engine installator
	/// </summary>
	public interface IEngineInstalationResolver
	{
		/// <summary>
		/// resolve engine installator
		/// </summary>
		IEngineInstaller GetIntaller(RuntimeOs osPlatform, RuntimeArchitecture architecture);
	}
}
