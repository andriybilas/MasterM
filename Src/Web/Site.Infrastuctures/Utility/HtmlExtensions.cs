using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Site.Infrastuctures.Utility
{
	public static class HtmlExtensions
	{
		public static string GetCurrentTheme ( this HtmlHelper instance )
		{
			return instance.ViewContext.HttpContext.Request.QueryString["theme"] ?? "Windows7";
		}
	}

}
