using System.Linq;
using System.Web.Mvc;

namespace Site.Infrastuctures.Utility
{
	public class WebStoreViewEngine : RazorViewEngine
	{
		public WebStoreViewEngine ()
		{
			var newLocationFormat = new[] { "~/Views/BackOffice/{1}/{0}.cshtml"  };
			ViewLocationFormats = ViewLocationFormats.Union (newLocationFormat).ToArray ();
			//PartialViewLocationFormats = PartialViewLocationFormats.Union (newLocationFormat).ToArray ();
		}
	}
}
