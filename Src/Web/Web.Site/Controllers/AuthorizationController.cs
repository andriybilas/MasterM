using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Litium.Common;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ECommerce;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.User;
using Site.Infrastuctures.Security;
using Site.Infrastuctures.Security.Model;
using Site.Infrastuctures.Utility;

namespace Web.Site.Controllers
{
    public class AuthorizationController : Controller
    {
        private static Guid Parse(string stringId)
        {
            Guid entityId;
            Guid.TryParse(stringId, out entityId);
            return entityId;
        }

        public ActionResult Index()
        {
            return View();
        }
		
        public ActionResult RegistrationStep1( SimpleRegistartionUserModel model )
		{
			if (ModelState.IsValid)
			{
				Person person = new Person();
				person.LoginName = model.LoginName;
				person.Active = true;
				person.LastLoginDate = DateTime.Now;
				person.Role = UserRole.Customer;
				person.SetPassword(model.Password);
				ActionHelper.TryExecute(() => Repository.Data.Save(person), ModelState);

				if (ModelState.IsValid)
				{
					model.Id = person.Id;
					WebStoreSecurity.Service.ValidateUser(model.LoginName, model.Password);
					return View("RegistrationStep2", new CompleteRegistartionUserModel { Id = model.Id, LoginName = model.LoginName });
				}
			}
			return View("Index");
		}

        [Security(Roles = UserRole.Customer | UserRole.Administer)]
		public ActionResult RegistrationStep2( CompleteRegistartionUserModel model )
		{
			if (ModelState.IsValid)
			{
				var person = Repository.Data.Get<Person>(model.Id);

				person.Profile = new PersonProfile();
				person.Profile.FirstName = model.Profile.FirstName;
				person.Profile.LastName = model.Profile.LastName;
				person.Profile.MiddleName = model.Profile.MiddleName;
				person.Profile.Phone = model.Profile.Phone;
				person.Profile.PhoneMobile = model.Profile.PhoneMobile;
				person.Profile.Email = model.Profile.Email;

				person.DeliveryAddress = new Address();
				person.DeliveryAddress.Address1 = model.DeliveryAddress.Address1;
				person.DeliveryAddress.City = model.DeliveryAddress.City;

				ActionHelper.TryExecute(() => Repository.Data.Save(person), ModelState);

				if (ModelState.IsValid)
				{
					return View();
				}
			}
			return new EmptyResult();
		}

		public ActionResult Authenticate( LogOnModel model )
		{
			if (ModelState.IsValid)
			{
				if (!WebStoreSecurity.Service.ValidateUser(model.LoginName, model.Password))
				{
					ModelState.AddModelError("LoginFailed", WebStroreResource.LoginFailed);
                    return View("Index");	    
				}
			}
            return Redirect("/Public/Index");
		}

        [Security(Roles = UserRole.Customer | UserRole.Administer)]
        public ActionResult UserProfile ()
        {
            var person = WebStoreSecurity.GetLoggedInUser();
            if (person != null)
            {
                return View("UserProfile", new UserProfileModel(person));
            }
            return View("Index");
        }

        [Security(Roles = UserRole.Customer | UserRole.Administer)]
        public ActionResult UpdateUserProfile (UserProfileModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.Id == Guid.Empty)
                    return View("UserProfile");

                var person = Repository.Data.Get<Person>(user.Id);
                person.CopyFrom(user);
                ActionHelper.TryExecute(() => Repository.Data.Save(person), ModelState);
				if (ModelState.IsValid)
					ViewBag.Message = WebStroreResource.UpdateProfileSuccessful;
            }
            
            return View("UserProfile");
        }

        [Security(Roles = UserRole.Customer | UserRole.Administer)]
        public ActionResult SaveUpdatePassword (UserChangePasswordModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.Id == Guid.Empty)
                    return View("UserProfile");

                var person = Repository.Data.Get<Person>(user.Id);
                if (person.Validate(user.OldPassword))
                {
                    person.SetPassword(user.Password);
                    ActionHelper.TryExecute(() => Repository.Data.Save(person), ModelState);
                }
                else
                {
                    ModelState.AddModelError("LoginFailed", WebStroreResource.LoginFailed);
                }                

            }
        	ViewBag.Message = WebStroreResource.PasswordChangedSuccessful;
            return View("ChangePassword");
        }

        [Security(Roles = UserRole.Customer | UserRole.Administer)]
        public ActionResult UserOrderHistory()
        {
            Person user = WebStoreSecurity.GetLoggedInUser();
            IEnumerable<Order> orders = Repository.Data.Get<Order>().Where(x => x.Customer.Id == user.Id).All();
            return View(orders);
        }

        [HttpPost]
        public ActionResult GetOrderDetails (String orderId)
        {
            Guid entityId = Parse(orderId);
            if (entityId != Guid.Empty)
                return PartialView("PublicSite/OrderHistoryDetails", Repository.Data.Get<Order>(entityId).OrderProducts);
            return Content(String.Empty);
        }

        public ActionResult ChangePassword ()
        {
            Person user = WebStoreSecurity.GetLoggedInUser();
            return View(new UserChangePasswordModel(user));
        }
    }
}
