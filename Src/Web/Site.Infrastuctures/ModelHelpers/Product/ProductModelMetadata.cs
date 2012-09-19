using System;
using System.ComponentModel.DataAnnotations;
using Litium.Common.Validation.CustomAttribute;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;

namespace Site.Infrastuctures.ModelHelpers.Product
{
    public class ProductModelMetadata
    {
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.ProductName)]
        public String Name { get; set; }

        [PriceValueCompatible(ErrorMessageResourceName = ResourceKey.IncompatiblePriceValue, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.Price)]
        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }

        [DataType(DataType.Currency)]
        public Decimal CampaignPrice { get; set; }

        [CountValueCompatible(ErrorMessageResourceName = ResourceKey.IncompatibleCountValue, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.StockBalance)]
        [DataType("Number")]
        public int StockBalance { get; set; }

        [ResourceDisplayName(ResourceKey.CategoryName)]
        [DisplayFormat(NullDisplayText = "Uncategorized")]
        public Category Category { get; set; }

        [ResourceDisplayName(ResourceKey.CreateDate)]
        [DataType(DataType.Date)]
        public DateTime? CreateDate { get; set; }
    }
}