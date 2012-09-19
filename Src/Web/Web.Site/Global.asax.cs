using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Litium.Common;
using Litium.Common.Configurations;
using Litium.Common.Lifecycle;
using Litium.Common.Setup;
using Litium.Domain.Entities.Customers;
using Site.Infrastuctures.Security;
using Site.Infrastuctures.Utility;

namespace Web.Site
{

    public class MvcApplication : System.Web.HttpApplication
    {
    	public MvcApplication()
    	{
			BeginRequest += (s, e) => ConversationHelper.Open();
			EndRequest += (s, e) => ConversationHelper.Close();
    	}

    	public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
			filters.Add(new SecurityAttribute());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Public", action = "Index", id = UrlParameter.Optional } // Parameter defaults 
				);

            //routes.MapRoute(
            //    "Category", // Route name
            //    "{controller}/{action}/{name}/{id}", // URL with parameters
            //    new { controller = "Public", action = "Category", name = "Default", id = Guid.Empty } // Parameter defaults 
            //    );
        }

        protected void Application_Start()
        {
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize(); 
			LitiumSetup.Setup (LitiumSectionGroup.Instance.Plugin.SolutionAssemblies.Distinct ());
	        RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
			ValueProviderFactories.Factories.Add (new JsonValueProviderFactory ());
            AreaRegistration.RegisterAllAreas();
			//Thread.CurrentThread.CurrentCulture = new CultureInfo("uk-UA");
			//Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk-UA");
        }

		protected virtual void Application_End ( object sender, EventArgs e )
		{
			LitiumSetup.Release ();
		}

		protected void Application_AuthenticateRequest( object sender, EventArgs e )
		{
			// If the user is logged-in, make sure his cache details are still 
			// available, otherwise redirect to login page.
            if (Request.IsAuthenticated && WebStoreSecurity.GetLoggedInUser() == null)
			{
				FormsAuthentication.SignOut();
			}
		}
    }
}