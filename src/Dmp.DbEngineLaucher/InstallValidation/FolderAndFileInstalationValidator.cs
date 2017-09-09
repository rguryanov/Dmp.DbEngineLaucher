using System;
using System.Collections.Generic;
using System.IO;

namespace Dmp.DbEngineLaucher.Installation
{
	public class FolderAndFileInstalationValidator : IInstalationValidator
	{
		IEnumerable<string> _folders;
		IEnumerable<string> _files;
		public FolderAndFileInstalationValidator(IEnumerable<string> folders, IEnumerable<string> files)
		{
			_folders = folders;
			_files = files;
		}

		public InstallValidationResult Validate(string installationPath)
		{
			foreach (var folder in _folders)
			{
				if (string.IsNullOrWhiteSpace(folder))
				{
					throw new NullReferenceException("empty folder in list");
				}

				if (!Directory.Exists(folder))
				{
					var error = string.Format("Folder not found: {0}", folder);
					return new InstallValidationResult(false, error);
				}
			}

			foreach (var file in _files)
			{
				if (string.IsNullOrWhiteSpace(file))
				{
					throw new NullReferenceException("empty file in list");
				}

				if (!File.Exists(file))
				{
					var error = string.Format("file not found: {0}", file);
					return new InstallValidationResult(false, error);
				}
			}
			
			return new InstallValidationResult(true, null);
		}
	}
}
