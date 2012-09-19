using System.ComponentModel.DataAnnotations;

namespace Litium.Resources
{
	public class ResourceNullFormatAttribute : DisplayFormatAttribute
	{
		public ResourceNullFormatAttribute( string resourceKey ) : base()
		{
			base.NullDisplayText = GetMessageFromResource(resourceKey);
		}

		private static string GetMessageFromResource( string key )
		{
			return StoreResourceStrings.Get(key);
		}
	}
}