using System;
using System.ComponentModel.DataAnnotations;
using Litium.Common.CustomSerialization;
using Litium.Common.Entities;
using Litium.Common.Validation.CustomAttribute;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;

namespace Litium.Domain.Entities.ECommerce
{
    [MetadataType(typeof(OrderProductMetadata))]
    public class OrderProduct : Entity
    {
        [NotSerializable]
		public virtual decimal CampaignPrice { get; set; }

        public virtual decimal Price { get; set; }

		public virtual int Count { get; set; }
        
        [NotSerializable]
		public virtual Product Product { get; set; }

        [NotSerializable]
        public virtual String ProductName { get; set; }

        public virtual decimal Summa { get; set; }

    	public override object ValidationCopy()
    	{
    		return Clone();
    	}
    }

    public class OrderProductMetadata
    {
        [PriceValueCompatible(ErrorMessageResourceName = ResourceKey.IncompatiblePriceValue, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.CampaignPrice)]
        [DataType(DataType.Currency)]
        public virtual decimal CampaignPrice { get; set; }

        [PriceValueCompatible(ErrorMessageResourceName = ResourceKey.IncompatiblePriceValue, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.Price)]
        [DataType(DataType.Currency)]
        public virtual decimal Price { get; set; }

        [CountValueCompatible(ErrorMessageResourceName = ResourceKey.IncompatibleCountValue, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.Count)]
        public virtual int Count { get; set; }

        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual Product Product { get; set; }

        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.ProductName)]
        public virtual String ProductName { get; set; }

        [PriceValueCompatible(ErrorMessageResourceName = ResourceKey.IncompatiblePriceValue, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.Summa)]
        [DataType(DataType.Currency)]
        public virtual decimal Summa { get; set; }

    }
}