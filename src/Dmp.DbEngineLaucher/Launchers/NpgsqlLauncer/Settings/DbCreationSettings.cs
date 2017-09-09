using System.Collections.Generic;

namespace Dmp.NpgsqlEngineLaucher
{
	/// <summary>
	/// setting for db cluster creation
	/// </summary>
	public class DbCreationSettings
	{
		/// <summary>
		/// username
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// password
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Locale
		/// </summary>
		public string Locale { get; set; }

		/// <summary>
		/// db encoding
		/// </summary>
		public string Encoding { get; set; }

		public IDictionary<string, string> AdditionalParameters { get; set; }

		public DbCreationSettings(string userName, string password, string locale = null, string encoding = "UTF8")
		{
			UserName = userName;
			Password = password;
			Locale = locale;
			Encoding = encoding;
			AdditionalParameters = new Dictionary<string, string>();
		}
	}
}
