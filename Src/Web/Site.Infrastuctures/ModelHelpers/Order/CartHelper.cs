using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Litium.Common;
using Litium.Common.WorkUnit;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ECommerce;
using Litium.Domain.Entities.Shop;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.Campaign;
using Site.Infrastuctures.ModelHelpers.User;
using Site.Infrastuctures.Security;
using LCampaign = Litium.Domain.Entities.ProductCatalog.Campaign;

namespace Site.Infrastuctures.ModelHelpers.Order
{
	public class CartHelper
	{
	    private static CartHelper _source;
		public static CartHelper Source { get { return _source ?? (_source = new CartHelper()); } }

        public OrderModel GetCartItems()
		{
			var order = HttpContext.Current.Session[WebStroreResource.CART] as OrderModel;
            if(order == null)
                return new OrderModel();
            return order;
		}

		public void AddItemToCart (CartModel model)
		{
			var order = HttpContext.Current.Session[WebStroreResource.CART] as OrderModel ?? new OrderModel();

		    if (order.OrderRows.Contains(model, new CartComparer()))
			{
				CartModel current = order.OrderRows.FirstOrDefault(x => x.ProductId == model.ProductId);
				order.OrderRows.Add(CombineCartItems(current, model));
				order.OrderRows.Remove(current);
			}
			else
			{
				order.OrderRows.Add(model.CalculateSumma());
			}

			HttpContext.Current.Session[WebStroreResource.CART] = order;
		}

		private CartModel CombineCartItems(CartModel current, CartModel model)
		{
			return new CartModel {
				ProductId = model.ProductId,
				CampaignPrice = model.CampaignPrice,
				Price = model.Price,
				ProductName = model.ProductName,
				Count = model.Count + current.Count,
				Summa = model.CalculateSumma().Summa + current.CalculateSumma().Summa };
		}

		public void RemoveCartItem(Guid pid)
		{
			var order = HttpContext.Current.Session[WebStroreResource.CART] as OrderModel;
			CartModel cart = order.OrderRows.FirstOrDefault(x => x.ProductId == pid);
			order.OrderRows.Remove(cart);
			HttpContext.Current.Session[WebStroreResource.CART] = order;
		}

		public OrderModel CalculateOrder()
		{
			var order = HttpContext.Current.Session[WebStroreResource.CART] as OrderModel;

            if(order == null)
                return new OrderModel();

			var user = WebStoreSecurity.GetLoggedInUser();

			if (String.IsNullOrWhiteSpace(order.OrderNumber))
			{
				order.CreateDate = DateTime.Now;
				order.OrderNumber = GenerateOrderNumber(order.CreateDate);
			}

            if (user != null)
            {
                ApplyPersonalCampaign(order, user);

                if (user.DeliveryAddress != null)
                    order.DeliveryAddress = user.DeliveryAddress;

				order.Customer = new UserModel(user);
            }

            order.CalculateOrder();
			return order;
		}

		private string GenerateOrderNumber(DateTime createDate)
		{
			return String.Format("{0}{1}{2}{3}{4}", createDate.Year, 
				createDate.Month, createDate.Day, createDate.Hour, createDate.Minute);
		}

		private void ApplyPersonalCampaign(OrderModel model, Person user)
	    {
	        IEnumerable<LCampaign> campains = Repository.Data.Get<LCampaign>().All().Where(x => x.Active);
	        int personalDiscountPercent = 0;
            foreach (var campaign in campains.Where(x => x.Active))
	        {
	            var campaignType = (CampaignType) campaign.Metadata.CampaignType;
                if( campaignType == CampaignType.PersonalCampaign )
                {
                    PersonalCampaign personalCampaign = campaign.Metadata.Data;
                    int tempResult = personalCampaign.GetPersonalDiscountPercent(model, user);
                    if (tempResult >= personalDiscountPercent)
                        personalDiscountPercent = tempResult;
                }
	        }

	        model.PersonalDiscount = model.OrderSumma*personalDiscountPercent / 100;
	    }

        public void SaveOrder()
        {
            var orderModel = HttpContext.Current.Session[WebStroreResource.CART] as OrderModel;
            var user = WebStoreSecurity.GetLoggedInUser();

            var order = new Litium.Domain.Entities.ECommerce.Order
            {
                CreateDate = orderModel.CreateDate, 
				OrderNumber =  orderModel.OrderNumber,
                Customer = user, 
                OrderState = State.Created,
                DeliveryMethod = orderModel.DeliveryMethod,
                Description = orderModel.DeliveryDescription,
                OrderSumma = orderModel.OrderSumma,
                OrderTotal = orderModel.OrderTotal
            };

            var products = new List<OrderProduct>();

            foreach (var cartItem in orderModel.OrderRows)
            {
                var orderRow = new OrderProduct();
                orderRow.CopyFromModel(cartItem);
                products.Add(orderRow);
            }

            using (var uow = new UnitOfWork())
            {
                Repository.Data.Save(products);
                order.OrderProducts = products;
                Repository.Data.Save(order);
                uow.Commit();
            }
        }

	    private void ThrowOutOfStockException(OrderProduct orderRow)
	    {
	        if (orderRow.Count > orderRow.Product.StockBalance)
                throw new OutOfStockException(orderRow.Product.Name, WebStroreResource.OutOfStock);
	    }

	    public IEnumerable<DeliveryMethod> GetDeliveries()
	    {
	        return Repository.Data.Get<DeliveryMethod>().All();
	    }

	    public void SetDeliveryMethod(Guid deliveryId)
	    {
            var orderModel = HttpContext.Current.Session[WebStroreResource.CART] as OrderModel;
	        DeliveryMethod method = Repository.Data.Get<DeliveryMethod>().Where(x => x.Id == deliveryId).FirstOrDefault().Value;
	        orderModel.DeliveryMethod = method;
	        HttpContext.Current.Session[WebStroreResource.CART] = orderModel;
	    }
	}
}

