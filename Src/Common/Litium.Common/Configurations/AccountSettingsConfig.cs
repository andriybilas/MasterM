using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Litium.Common.Configurations
{
	public class AccountSettingsConfig : ConfigurationSectionGroup
	{
		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static AccountSettingsConfig Instance
		{
			get
			{
				return (AccountSettingsConfig)ConfigurationManager.GetSection("litium/accountSettings");
			}
		}

		/// <summary>
		/// True if sessions are timed out in admin.
		/// </summary>
		public bool EnableSessionTimeoutsInGui { get; internal set; }

		/// <summary>
		/// System user config.
		/// </summary>
		public SystemUserConfig SystemUser 
		{ 
			get 
			{
				return (SystemUserConfig)ConfigurationManager.GetSection("litium/accountSettings/systemUser");
			}
		}
	}
}
