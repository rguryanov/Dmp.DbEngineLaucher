using System;

namespace Dmp.DbEngineLaucher.Installation
{
	public interface IEngineInstalationValidatorResolver
	{
		IInstalationValidator GetIntallValidator(RuntimeOs osPlatform, RuntimeArchitecture architecture);
	}
}
