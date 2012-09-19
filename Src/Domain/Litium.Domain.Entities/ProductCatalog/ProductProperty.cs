using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Domain.Entities.Metadata;

namespace Litium.Domain.Entities.ProductCatalog
{
	[MetadataType(typeof(ProductPropertyMetadata))]
	public class ProductProperty : Entity
	{
		public ProductProperty()
		{
			Brend = string.Empty;
			Capacity = string.Empty;
			Country = string.Empty;
			Weight = string.Empty;
		}

		public virtual string Brend { get; set; }

		public virtual string Capacity { get; set; }

		public virtual string Country { get; set; }

		public virtual string Weight { get; set; }

		public override object ValidationCopy()
		{
			return Clone();
		}
	}
}