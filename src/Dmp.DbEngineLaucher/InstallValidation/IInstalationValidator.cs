namespace Dmp.DbEngineLaucher.Installation
{
	public interface IInstalationValidator
	{
		InstallValidationResult Validate(string installationPath);
	}
}
