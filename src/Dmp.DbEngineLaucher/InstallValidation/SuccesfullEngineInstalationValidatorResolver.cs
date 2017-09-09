namespace Dmp.DbEngineLaucher.Installation
{
	public class SuccesfullEngineInstalationValidatorResolver : IEngineInstalationValidatorResolver
	{
		public IInstalationValidator GetIntallValidator(RuntimeOs osPlatform, RuntimeArchitecture architecture)
		{
			return new SuccessfulInstalationValidator();
		}
	}
}
