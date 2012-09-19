using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Litium.Common;
using Litium.Common.WorkUnit;
using Litium.Domain.Entities.Customers;
using Site.Infrastuctures.ModelHelpers.User;
using Site.Infrastuctures.Security;
using Telerik.Web.Mvc;

namespace Web.Site.Controllers
{
	[Security(Roles = UserRole.Administer)]
	public class UsersController : Controller
	{
        private IEnumerable<UserModel> GetPersons(GridCommand command)
        {
            IEnumerable<UserModel> users = UserModelSource.Source.Get(command);
            return users;
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult GetPersonsAsync(GridCommand command)
        {
            return View("Users", GetPersonsGridModel(command));
        }

        private GridModel<UserModel> GetPersonsGridModel(GridCommand command)
        {
            return new GridModel<UserModel> { Data = GetPersons(command), Total = GetCount() };
        }

        private int GetCount()
        {
            return UserModelSource.Source.Count;
        }

		public ActionResult Index()
		{
            return View("Users", GetPersons(new GridCommand { Page = 1, PageSize = 20 }));
		}

		[GridAction]
		public ActionResult UpdatePersonAsync ()
		{
			var edited = new UserModel();

			if (TryUpdateModel(edited))
			{
			    var current = Repository.Data.Get<Person>(edited.Id);
			    current.CopyFrom(edited);
                Repository.Data.Save(current);
			}

            return GetPersonsAsync(new GridCommand { Page = 1, PageSize = 20 });
		}

        [GridAction]
        public ActionResult InsertPersonAsync ()
        {
            var inserting = new UserModel();

            if (TryUpdateModel(inserting))
            {
                Person person = new Person();
                person.Profile = new PersonProfile();
                person.DeliveryAddress = new Address();
                person.CopyFrom(inserting);

                person.LastLoginDate = DateTime.Now;
                person.LoginName = String.Format("{0} {1}", inserting.FirstName, inserting.LastName );
                person.SetPassword(inserting.Password);
                person.Role =UserRole.Customer;
                Repository.Data.Save(person);
            }

            return GetPersonsAsync(new GridCommand { Page = 1, PageSize = 20 });
        }

        [GridAction]
        public ActionResult DeletePersonAsync(Guid userId)
        {
   
                var person = Repository.Data.Get<Person>(userId);
               // var addressMemeber = Repository.Data.Get<AddressListMember>().Where(x => x.Person.Id == person.Id);
                
                //if(addressMemeber != null)
                //    Repository.Data.Delete(addressMemeber);

                Repository.Data.Delete(person);


            return GetPersonsAsync(new GridCommand { Page = 1, PageSize = 20 });
        }
	}
}
