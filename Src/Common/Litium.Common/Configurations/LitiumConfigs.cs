namespace Litium.Common.Configurations
{
	public static class LitiumConfigs
	{
		public static DataSection Data
		{
			get { return LitiumSectionGroup.Instance.Data; }
		}

        public static GlobalizationSection Globalization
        {
            get { return LitiumSectionGroup.Instance.Globalization; }
        }

        public static PluginsSection Plugin
        {
            get { return LitiumSectionGroup.Instance.Plugin; }
        }
	}
}
