using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Litium.Common.Validation.CustomAttribute;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;

namespace Litium.Domain.Entities.Metadata
{

	public class ProductMetadata
	{
		[Required (ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof (DomainNotification))]
		[ResourceDisplayName (ResourceKey.ProductName)]
		public String Name { get; set; }

		[ResourceDisplayName (ResourceKey.Description)]
		public String Description { get; set; }

		[PriceValueCompatible (ErrorMessageResourceName = ResourceKey.IncompatiblePriceValue, ErrorMessageResourceType = typeof (DomainNotification))]
		[ResourceDisplayName (ResourceKey.Price)]
		public Decimal Price { get; set; }

		[CountValueCompatible (ErrorMessageResourceName = ResourceKey.IncompatibleCountValue, ErrorMessageResourceType = typeof (DomainNotification))]
		[ResourceDisplayName (ResourceKey.StockBalance)]
		public int StockBalance { get; set; }

		[ResourceDisplayName (ResourceKey.CategoryName)]
		[DisplayFormat(NullDisplayText = "Uncategorized")]
		public Category Category { get; set; }

		[ResourceDisplayName (ResourceKey.Published)]
		public bool Published { get; set; }

		[SQLDateValid (ErrorMessageResourceName = ResourceKey.SqlDateValide, ErrorMessageResourceType = typeof (DomainNotification))]
		[ResourceDisplayName (ResourceKey.CreateDate)]
		public DateTime CreateDate { get; set; }

		[SQLDateValid (ErrorMessageResourceName = ResourceKey.SqlDateValide, ErrorMessageResourceType = typeof (DomainNotification))]
		[ResourceDisplayName (ResourceKey.UpdateDate)]
		public DateTime UpdateDate { get; set; }

        public virtual DateTime LastSynchDate { get; set; }

        public virtual IList<Campaign> Campaigns { get; set; }

        public virtual bool HasImage { get; set; }

        public virtual IList<ProductSet> ProductSets { get; set; }
	}
}
