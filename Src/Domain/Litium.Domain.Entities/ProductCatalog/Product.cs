using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Litium.Common.CustomSerialization;
using Litium.Common.Entities;
using Litium.Domain.Entities.Metadata;

namespace Litium.Domain.Entities.ProductCatalog
{
	[MetadataType (typeof (ProductMetadata))]
	public class Product : Entity, IImage
	{
	    public virtual String Name { get; set; }

        public virtual String Article { get; set; }

	    public virtual String Description { get; set; }

		public virtual Decimal Price { get; set; }

		public virtual int StockBalance { get; set; }

        [NotSerializable]
        public virtual Category Category { get; set; }

        [NotSerializable]
		public virtual IList<Campaign> Campaigns { get; set; }

        [NotSerializable]
		public virtual bool Published { get; set; }

        [NotSerializable]
        public virtual ProductProperty ProductProperty { get; set; }

        [NotSerializable]
        public virtual bool HasImage { get; set; }

        [NotSerializable]
		public virtual IList<ProductSet> ProductSets { get; set; }

        [NotSerializable]
		public virtual DateTime? CreateDate { get; set; }

        [NotSerializable]
		public virtual DateTime? UpdateDate { get; set; }

        [NotSerializable]
        public virtual DateTime? LastSynchDate { get; set; }

	    public override object ValidationCopy()
		{
			return Clone();
		}
	}
}
