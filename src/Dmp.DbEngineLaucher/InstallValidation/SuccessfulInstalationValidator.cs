namespace Dmp.DbEngineLaucher.Installation
{
	public class SuccessfulInstalationValidator : IInstalationValidator
	{
		public InstallValidationResult Validate(string installationPath)
		{
			return new InstallValidationResult(true, null);
		}
	}
}
