using System;
using System.Configuration;

namespace Litium.Common.Configurations
{
	public class SystemUserConfig : ConfigurationSection
	{
		/// <summary>
		/// System user is enabled.
		/// </summary>
		[ConfigurationProperty("enabled", DefaultValue = "false", IsRequired = false)]
		public bool Enabled { get { return (bool) this["enabled"]; } }


		[ConfigurationProperty("userId", DefaultValue = "false", IsRequired = false)]
		public String UserId { get { return (String)this["userId"]; } }

		/// <summary>
		/// System user password.
		/// </summary>
		[ConfigurationProperty("password", IsRequired = false)]
		public string Password { get { return (string)this["password"]; } }

		/// <summary>
		/// System login.
		/// </summary>
		[ConfigurationProperty("login", IsRequired = false)]
		public string LogIn { get { return (string)this["login"]; } }
	}
}
