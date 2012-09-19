using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using Litium.Common;
using Litium.Common.Configurations;
using Litium.Domain.Entities.Customers;
using Litium.Resources;
using Site.Infrastuctures.Security.Model;

namespace Site.Infrastuctures.Security
{
	public class WebStoreSecurity
	{
		private static WebStoreSecurity _security;

		public static WebStoreSecurity Service
		{
			get { return _security ?? (_security = new WebStoreSecurity()); }
		}

        public static Person GetLoggedInUser ()
        {
            var cockieContent = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cockieContent != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cockieContent.Value);
                String userId = ticket.UserData;
				return Repository.Data.Get<Person>(Guid.Parse(userId)) ?? TryGetAdminSettings(userId);
            }
            return null;
        }

		private static Person TryGetAdminSettings(string userId)
		{
			var userConfig = (SystemUserConfig)ConfigurationManager.GetSection("litium/accountSettings/systemUser");
			if (Guid.Parse(userConfig.UserId) == Guid.Parse(userId))
			{
				return new Person {
						Role = UserRole.Administer,
						LoginName = userConfig.LogIn };
			}
			return null;
		}

		public bool ValidateUser( string username, string password)
		{
			Person user = Repository.Data.Get<Person>().Where(x => x.LoginName
				.Equals(username, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().Value;

			if (user == null)
				return false;

			if (user.Validate(password) && user.Active)
			{
				var authenticationTicket = new FormsAuthenticationTicket
                    (1, username, DateTime.Now, DateTime.Now.AddMinutes(30), true, user.Id.ToString());

				string cookieContents = FormsAuthentication.Encrypt(authenticationTicket);

				var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents) 
                {
					Expires = authenticationTicket.Expiration,
					Path = FormsAuthentication.FormsCookiePath
				};

				HttpContext.Current.Response.Cookies.Add(cookie);
				
				return true;
			}
			return false;
		}
	}
}
