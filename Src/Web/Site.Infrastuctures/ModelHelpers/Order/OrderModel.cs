using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ECommerce;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.User;

namespace Site.Infrastuctures.ModelHelpers.Order
{
	public class OrderModel
	{
		public OrderModel()
		{
			OrderRows = new List<CartModel>();
            DeliveryMethod = new DeliveryMethod();
		}

		public Address DeliveryAddress { get; set; }

		[DataType(DataType.Date)]
		public DateTime CreateDate { get; set; }

		[ResourceDisplayName(ResourceKey.OrderNumber)]
		public String OrderNumber { get; set; }

		public UserModel Customer { get; set; }
		
        public DeliveryMethod DeliveryMethod { get; set; }

		[ResourceDisplayName(ResourceKey.DeliveryDescription)]
		public String DeliveryDescription { get; set; }

		public IList<CartModel> OrderRows { get; set; }

		[ResourceDisplayName(ResourceKey.DeliveryPayment)]
        [DataType(DataType.Currency)]
        public Decimal DeliveryPayment { get { return DeliveryMethod.Cost; } }

		[ResourceDisplayName(ResourceKey.OrderSumma)]
        [DataType(DataType.Currency)]
        public Decimal OrderSumma { get { return OrderRows.Sum(x => x.Summa); } }

		[ResourceDisplayName(ResourceKey.OrderTotal)]
        [DataType(DataType.Currency)]
		public Decimal OrderTotal { get; set; }

        [ResourceDisplayName(ResourceKey.PersonalDiscount)]
        [DataType(DataType.Currency)]
	    public Decimal PersonalDiscount { get; set; }

	    public void CalculateOrder()
	    {
			OrderTotal = OrderSumma + DeliveryPayment - PersonalDiscount;
		}
	}
}

