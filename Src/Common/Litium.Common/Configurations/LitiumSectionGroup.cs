using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Litium.Common.Configurations
{
	public class LitiumSectionGroup : ConfigurationSectionGroup
	{
		private readonly static LitiumSectionGroup _instance;
		private DataSection _dataSection;
        private GlobalizationSection _globalizationSection;
		private PluginsSection _pluginSection;

		static LitiumSectionGroup()
		{
			Configuration config = null;

			var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.StartsWith("System.Web,"));
			if (assembly != null)
			{
				var type = assembly.GetType("System.Web.Configuration.WebConfigurationManager");
				var hostingType = assembly.GetType("System.Web.Hosting.HostingEnvironment");
				if (type != null && hostingType != null)
				{
					var applicationPath = hostingType.GetProperty("ApplicationVirtualPath").GetValue(null, null);
					if (applicationPath != null)
					{
						config = (Configuration)type.InvokeMember("OpenWebConfiguration", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, new[] { applicationPath });
					}
				}
			}

			if (config == null)
			{
				config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			}
			_instance = config.GetSectionGroup("litium") as LitiumSectionGroup;
		}

		public static LitiumSectionGroup Instance
		{
			get { return _instance; }
		}

		public DataSection Data
		{
			get { return _dataSection ?? (_dataSection = Sections["data"] as DataSection); }
		}

        public GlobalizationSection Globalization
        {
            get { return _globalizationSection ?? (_globalizationSection = Sections["globalization"] as GlobalizationSection); }
        }

		public PluginsSection Plugin
		{
			get { return _pluginSection ?? (_pluginSection = Sections["plugin"] as PluginsSection ?? new PluginsSection()); }
		}
	}
}
