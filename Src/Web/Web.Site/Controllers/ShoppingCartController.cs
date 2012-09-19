using System;
using System.Configuration;
using System.Net.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using Litium.Domain.Entities.Customers;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.Order;
using Site.Infrastuctures.Security;
using Site.Infrastuctures.Utility;
using Web.Site.Extensions;

namespace Web.Site.Controllers
{
    public class ShoppingCartController : Controller
    {
		[Security(Roles = UserRole.Customer | UserRole.Administer)]
        public ActionResult Index()
        {
			return View(CartHelper.Source.CalculateOrder());
        }

        [HttpPost]
        public ActionResult UpdateShoppingCartOrderRows ()
        {
            return PartialView("PublicSite/ShoppingCartOrder", CartHelper.Source.CalculateOrder());
        }

        [HttpPost]
        public ActionResult AddProductToCart(CartModel model)
        {
            CartHelper.Source.AddItemToCart(model);
            return PartialView("PublicSite/MiniCart", CartHelper.Source.GetCartItems());
        }

        [HttpPost]
        public ActionResult RemoveCartItem(Guid productId)
        {
            CartHelper.Source.RemoveCartItem(productId);
            return PartialView("PublicSite/MiniCart", CartHelper.Source.GetCartItems());
        }

        [Security(Roles = UserRole.Customer | UserRole.Administer)]
        public ActionResult SubmitOrder (OrderModel orderModel)
        {
            ActionHelper.TryExecute(() => CartHelper.Source.SaveOrder(), ModelState);
            if (ModelState.IsValid)
            {
            	SendEmail();
                HttpContext.Session[WebStroreResource.CART] = null;
                return View("OrderComplete");
            }

        	return View("Index", CartHelper.Source.CalculateOrder());
        }

    	private void SendEmail()
    	{
    		var user = WebStoreSecurity.GetLoggedInUser();
			var order = System.Web.HttpContext.Current.Session[WebStroreResource.CART] as OrderModel;
			if(user != null)
    		{
				var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

				WebMail.SmtpPort = smtpSection.Network.Port;
				WebMail.SmtpServer = smtpSection.Network.Host;

				WebMail.UserName = smtpSection.Network.UserName;
				WebMail.Password = smtpSection.Network.Password;

				var messageBody = PartialView("PublicSite/MailOrder", order).Capture(ControllerContext);

				WebMail.Send(user.Profile.Email, WebStroreResource.Subject,
					messageBody, smtpSection.From, null, null, true); 
    		}
    	}

    	public ActionResult OrderComplete ()
        {
			return View("OrderComplete");
        }

        [Security(Roles = UserRole.Customer | UserRole.Administer)]
        public void DeliveryMethodChanged (Guid deliveryId)
        {
            CartHelper.Source.SetDeliveryMethod(deliveryId);
        }
    }
}
