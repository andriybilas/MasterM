using System.Configuration;

namespace Litium.Common.Configurations
{
	/// <summary>
    /// Globalization section: configuration of Litium.Common.Resources.
	/// </summary>
    public class GlobalizationSection : ConfigurationSection
	{
		private const string _defaultResourceFolder = "defaultResourceFolder";
        private const string _defaultResourceWebStrings = "defaultResourceWebStrings";
        private const string _defaultResourceCulture = "defaultResourceCulture";
		
		/// <summary>
        /// Default resource folder. Path offset where *.resx files are stored. 
		/// </summary>
        [ConfigurationProperty(_defaultResourceFolder, DefaultValue = @"App_Data/Resources/", IsRequired = false)]
		public string DefaultResourceFolder
		{
			get
			{
				return (string)this[_defaultResourceFolder];
			}
			set
			{
                this[_defaultResourceFolder] = value;
			}
		}

        /// <summary>
        /// Default resource for web strings. Default *.resx file for web strings.
        /// </summary>
        [ConfigurationProperty(_defaultResourceWebStrings, DefaultValue = "WebStrings", IsRequired = false)]
        public string DefaultResourceWebStrings
        {
            get
            {
                return (string)this[_defaultResourceWebStrings];
            }
            set
            {
                this[_defaultResourceWebStrings] = value;
            }
        }

        /// <summary>
        /// Fall-back culture. Default resource *.resx with no culture info in the file name based on this culture. 
        /// </summary>
        [ConfigurationProperty(_defaultResourceCulture, DefaultValue = "en-us", IsRequired = false)]
        public string DefaultResourceCulture
        {
            get
            {
                return (string)this[_defaultResourceCulture];
            }
            set
            {
                this[_defaultResourceCulture] = value;
            }
        }
	}
}
