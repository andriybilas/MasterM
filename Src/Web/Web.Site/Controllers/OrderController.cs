using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Litium.Common;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ECommerce;
using Litium.Domain.Entities.Shop;
using Site.Infrastuctures.Security;
using Site.Infrastuctures.Utility;
using Telerik.Web.Mvc;

namespace Web.Site.Controllers
{
	[Security(Roles = UserRole.Administer)]
	public class OrderController : Controller
    {
        private IEnumerable<Order> GetOrders()
        {
            IEnumerable<Order> orders = Repository.Data.Get<Order>().All().OrderByDescending(x => x.CreateDate);
            return orders;
        }

        public ActionResult Index()
        {
            return View("Order", GetOrders());
        }

        [GridAction]
        public ActionResult GetOrdersAsync()
        {
            return View("Order", GetOrdersGridModel());
        }

        [GridAction]
        public ActionResult GetRorderDetailsAsync (String orderId)
        {
            Guid corderId;
            if(Guid.TryParse(orderId, out corderId))
            {
                IEnumerable<OrderProduct> orderDetails = Repository.Data.Get<Order>(corderId).OrderProducts;
                return View("Order", new GridModel<OrderProduct>(orderDetails));
            }
            return View("Order", new GridModel<OrderProduct>(new List<OrderProduct>()));
        }

        private GridModel<Order> GetOrdersGridModel()
        {
            return new GridModel<Order>(GetOrders());
        }

        [GridAction]
        public ActionResult InsertOrderAsync()
        {
            return GetOrdersAsync();
        }

		[GridAction]
		public ActionResult UpdateOrderAsync( Guid orderId, String orderState, string paymentMethodId, string deliveryMethodId )
        {
			if(String.IsNullOrWhiteSpace(paymentMethodId)||String.IsNullOrWhiteSpace(deliveryMethodId))
				return GetOrdersAsync();

			var order = Repository.Data.Get<Order>(orderId);

			State state;
			Enum.TryParse(orderState, out state);
			order.OrderState = state;

			order.PaymentMethod = Repository.Data.Get<PaymentMethod>(Guid.Parse(paymentMethodId));
			order.DeliveryMethod = Repository.Data.Get<DeliveryMethod>(Guid.Parse(deliveryMethodId));
			ActionHelper.TryExecute(() => Repository.Data.Save(order), ModelState);
			return Content("");
        }

        [GridAction]
        public ActionResult DeleteOrderAsync(Guid orderId)
        {
			Order order = Repository.Data.Get<Order>(orderId);
			Repository.Data.Delete(order);
            return GetOrdersAsync();
        }
  
    }
}
