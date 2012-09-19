using System.Web.Mvc;

namespace Web.Site.Extensions
{
    public static class ActionResultExtensions
	{
		public static string Capture( this ActionResult result, ControllerContext controllerContext )
		{
			using (var it = new ResponseCapture(controllerContext.RequestContext.HttpContext.Response))
			{
				result.ExecuteResult(controllerContext);
				return it.ToString();
			}
		}
	}

}