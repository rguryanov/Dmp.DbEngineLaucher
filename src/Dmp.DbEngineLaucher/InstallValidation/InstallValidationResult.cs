namespace Dmp.DbEngineLaucher.Installation
{
	public class InstallValidationResult
	{
		public bool IsValid { get; set; }
		public string Error { get; set; }

		public InstallValidationResult(bool isValid, string error)
		{
			this.IsValid = isValid;
			this.Error = error;
		}
	}
}
