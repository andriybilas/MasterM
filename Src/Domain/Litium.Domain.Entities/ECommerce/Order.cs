using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Litium.Common.CustomSerialization;
using Litium.Common.Entities;
using Litium.Common.Validation.CustomAttribute;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.Shop;
using Litium.Resources;

namespace Litium.Domain.Entities.ECommerce
{
    [MetadataType(typeof(OrderMetadata))]
	public class Order : Entity
    {
        public Order()
		{
			OrderProducts = new List<OrderProduct>();
		}

		public virtual String OrderNumber { get; set; }

		public virtual DateTime CreateDate { get; set; }

        [NotSerializable]
        public virtual DateTime? LastSynchDate { get; set; }

        [ReferenceRequared(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [NotSerializable]
        public virtual DeliveryMethod DeliveryMethod { get; set; }
        
        [NotSerializable]
		public virtual IList<OrderProduct> OrderProducts { get; set; }

        [NotSerializable]
        public virtual PaymentMethod PaymentMethod { get; set; }

        [ReferenceRequared(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [NotSerializable]
		public virtual Person Customer { get; set; }

        [EnumTypeCompatible(ErrorMessageResourceName = ResourceKey.EnumFormat, ErrorMessageResourceType = typeof(DomainNotification))]
		public virtual State OrderState { get; set; }

        [PriceValueCompatible(ErrorMessageResourceName = ResourceKey.IncompatiblePriceValue, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual Decimal OrderSumma { get; set; }

        [PriceValueCompatible(ErrorMessageResourceName = ResourceKey.IncompatiblePriceValue, ErrorMessageResourceType = typeof(DomainNotification))]
		public virtual Decimal OrderTotal { get; set; }

        [NotSerializable]
		public virtual String Description { get; set; }

		public override object Clone ()
		{
			var clone = (Order)base.Clone ();
			if (DeliveryMethod != null)
				clone.DeliveryMethod = (DeliveryMethod)DeliveryMethod.Clone ();
			if (PaymentMethod != null)
				clone.PaymentMethod = (PaymentMethod)PaymentMethod.Clone ();
			if (Customer != null)
				clone.Customer = (Person)Customer.Clone ();

			foreach (OrderProduct orderProduct in OrderProducts)
			{
				clone.OrderProducts.Add ((OrderProduct) orderProduct.Clone());	
			}

			return clone;
		}

    	public override object ValidationCopy()
    	{
			var clone = (Order)base.Clone();
			if (DeliveryMethod != null)
				clone.DeliveryMethod = (DeliveryMethod)DeliveryMethod.Clone();
			if (PaymentMethod != null)
				clone.PaymentMethod = (PaymentMethod)PaymentMethod.Clone();
			if (Customer != null)
				clone.Customer = (Person) Customer.Clone();

    		clone.OrderProducts = OrderProducts;

			return clone;
    	}
	}

    public class OrderMetadata
    {
        [ResourceDisplayName(ResourceKey.CreateDate)]
        [SQLDateValid(ErrorMessageResourceName = ResourceKey.SqlDateValide, ErrorMessageResourceType = typeof(DomainNotification))]
        [DataType(DataType.Date)]
        public virtual DateTime CreateDate { get; set; }

		[ResourceDisplayName(ResourceKey.OrderNumber)]
		[ResourceNullFormat(ResourceKey.NullOrderNumberText)]
		public virtual String OrderNumber { get; set; }

        [ReferenceRequared(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [UIHint("DeliveryMethodEnumerator")]
        [ResourceDisplayName(ResourceKey.DeliveryMethod)]
        public virtual DeliveryMethod DeliveryMethod { get; set; }

        public virtual IList<OrderProduct> OrderProducts { get; set; }

        [UIHint("PaymentMethodEnumerator")]
        [ResourceDisplayName(ResourceKey.PaymentMethod)]
        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual Person Customer { get; set; }

        [ResourceDisplayName(ResourceKey.OrderState)]
        [UIHint("OrderStateEnumerator")]
        public virtual State OrderState { get; set; }

        [ResourceDisplayName(ResourceKey.Summa)]
        [DataType(DataType.Currency)]
        public virtual decimal OrderSumma { get; set; }

        [ResourceDisplayName(ResourceKey.OrderTotal)]
        [DataType(DataType.Currency)]
        public virtual Decimal OrderTotal { get; set; }
    }
}
