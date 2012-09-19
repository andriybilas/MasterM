using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Litium.Common;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ECommerce;
using Site.Infrastuctures.Security;
using Site.Infrastuctures.Utility;
using Telerik.Web.Mvc;

namespace Web.Site.Controllers
{
	[Security(Roles = UserRole.Administer)]
    public class DeliveryController : Controller
    {
        public ActionResult Index()
        {
            return View("Delivery");
        }

        [GridAction]
        public ActionResult GetDeliveriesAsync()
        {
            IEnumerable<DeliveryMethod> deliveries = Repository.Data.Get<DeliveryMethod>().All();
            return PartialView("Delivery/DeliveriesView", new GridModel<DeliveryMethod> { Data = deliveries });
        }

        [GridAction]
        public ActionResult InsertDeliveryAsync()
        {
            var inserting = new DeliveryMethod();

            if (TryUpdateModel(inserting))
            {
                Repository.Data.Save(inserting);
            }

            return GetDeliveriesAsync();
        }

        [GridAction]
        public ActionResult UpdateDeliveryAsync()
        {
            var edited = new DeliveryMethod();

            if (TryUpdateModel(edited))
            {
                var current = Repository.Data.Get<DeliveryMethod>(edited.Id);
                current.Name = edited.Name;
                current.Cost = edited.Cost;
                current.Description = edited.Description;
                Repository.Data.Save(current);
            }

            return GetDeliveriesAsync();
        }

        [GridAction]
        public ActionResult DeleteDeliveryAsync(Guid deliveryId)
        {
            var delivery = Repository.Data.Get<DeliveryMethod>(deliveryId);
            ActionHelper.TryExecute(()=> Repository.Data.Delete(delivery), ModelState);
            if(ModelState.IsValid)
                return GetDeliveriesAsync();
            return Content("");
        }

    }
}
