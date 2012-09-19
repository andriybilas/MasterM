using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Litium.Common;
using Litium.Domain.Entities.ECommerce;
using Litium.Resources;
using LOrder = Litium.Domain.Entities.ECommerce.Order;

namespace Site.Infrastuctures.ModelHelpers.Order
{
    public class OrderHelper
    {
        private static OrderHelper _instance;

        public static OrderHelper Source
        {
            get { return _instance ?? (_instance = new OrderHelper()); }
        }

        public IEnumerable<SelectListItem> GetPaymentMethods()
        {
            IEnumerable<PaymentMethod> paymentMethods = Repository.Data.Get<PaymentMethod>().All();
            return paymentMethods.Select(method => new SelectListItem { Value = method.Id.ToString(), Text = method.Name }).ToList();
        }

        public IEnumerable<SelectListItem> GetDeliveryMethods()
        {
            IEnumerable<DeliveryMethod> paymentMethods = Repository.Data.Get<DeliveryMethod>().All();
            return paymentMethods.Select(method => new SelectListItem { Value = method.Id.ToString(), Text = method.Name }).ToList();
        }

        public IEnumerable<SelectListItem> GetOrderStates()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "0", Text = WebStroreResource.Canceled });
            list.Add(new SelectListItem { Value = "1", Text = WebStroreResource.Created });
            list.Add(new SelectListItem { Value = "2", Text = WebStroreResource.Paid });
            list.Add(new SelectListItem { Value = "3", Text = WebStroreResource.Delivered });
            return list;
        }
    }
}
