using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Litium.Common.Configurations;
using Litium.Resources;
using Site.Infrastuctures.Security.Model;

namespace Web.Site.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View("Admin");
        }

    	public ActionResult LogIn(LogOnModel  model)
    	{
			if (ModelState.IsValid)
			{
				var userConfig = (SystemUserConfig)ConfigurationManager.GetSection("litium/accountSettings/systemUser");

				if (userConfig.LogIn.Equals(model.LoginName, StringComparison.InvariantCultureIgnoreCase) &&
					userConfig.Password.Equals(model.Password))
				{

					AuthenticateUserAdmin(model.LoginName, Guid.Parse(userConfig.UserId));
					return RedirectToAction("Index", "Users");
				}
			}

			ModelState.AddModelError("LoginFailed", WebStroreResource.LoginFailed);
			return Index();
    	}

    	private void AuthenticateUserAdmin(String username, Guid userId)
    	{
			var authenticationTicket = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddMinutes(30), true, userId.ToString());
			string cookieContents = FormsAuthentication.Encrypt(authenticationTicket);

			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents) 
			{
				Expires = authenticationTicket.Expiration,
				Path = FormsAuthentication.FormsCookiePath
			};

			HttpContext.Response.Cookies.Add(cookie);
    	}
    }
}
