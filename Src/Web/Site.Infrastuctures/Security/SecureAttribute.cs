using System;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Litium.Common;
using Litium.Domain.Entities.Customers;
using Litium.Resources;

namespace Site.Infrastuctures.Security
{
	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Method)]
	public class SecurityAttribute : AuthorizeAttribute
    {
        // the "new" must be used here because we are hiding
        // the Roles property on the underlying class
        public new UserRole Roles;
        
		protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

			Person user = WebStoreSecurity.GetLoggedInUser();

		    if (user == null || (Roles != 0 && ((Roles & user.Role) != user.Role)))
		        return false;

		    return true;
        }

	    public override void OnAuthorization( AuthorizationContext filterContext )
		{
			if (filterContext.Result is HttpUnauthorizedResult)
			{
				filterContext.Result = new RedirectToRouteResult(
					new RouteValueDictionary
						{
							{"controller", "Authorization"},
							{"action", "Index"},
							{"returnUrl", filterContext.HttpContext.Request.RawUrl}
						}
					);
			}

			bool skipAuthorization = !filterContext.ActionDescriptor.IsDefined(typeof(SecurityAttribute), true)
		   && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SecurityAttribute), true);

			if (!skipAuthorization)
			{
				base.OnAuthorization(filterContext);
			}
		}
    }
}

