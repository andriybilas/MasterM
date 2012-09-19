using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Infrastuctures.ModelHelpers.Order;

namespace Web.Site.Controllers
{
    public class CartController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public ActionResult RemoveCartItem (Guid productId)
		{
			CartHelper.Source.RemoveCartItem(productId);
			return PartialView("PublicSite/MiniCart", CartHelper.Source.GetCartItems());
		}

		[HttpPost]
		public ActionResult AddProductToCart( CartModel model )
		{
			CartHelper.Source.AddItemToCart(model);
			return PartialView("PublicSite/MiniCart", CartHelper.Source.GetCartItems());
		}

    }
}
