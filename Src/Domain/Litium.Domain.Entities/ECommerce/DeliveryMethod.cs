using System;
using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Common.Validation.CustomAttribute;
using Litium.Resources;

namespace Litium.Domain.Entities.ECommerce
{
    public class DeliveryMethod : Entity
    {
		[Required (ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof (DomainNotification))]
        [ResourceDisplayName(ResourceKey.DeliveryMethod)]
        public virtual String Name { get; set; }
        
		public virtual String Description { get; set; }

		[PriceValueCompatible (ErrorMessageResourceName = ResourceKey.IncompatiblePriceValue, ErrorMessageResourceType = typeof (DomainNotification))]
        [DataType(DataType.Currency)]
        public virtual decimal Cost { get; set; }

    	public override object ValidationCopy()
    	{
    		return MemberwiseClone();
    	}
    }
}