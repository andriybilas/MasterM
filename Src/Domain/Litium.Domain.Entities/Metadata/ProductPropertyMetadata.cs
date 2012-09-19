using Litium.Resources;

namespace Litium.Domain.Entities.Metadata
{
	public class ProductPropertyMetadata
	{
		[ResourceDisplayName (ResourceKey.ProductBrend)]
		public virtual string Brend { get; set; }

		[ResourceDisplayName (ResourceKey.ProductCapacity)]
		public virtual string Capacity { get; set; }

		[ResourceDisplayName (ResourceKey.ProductCountry)]
		public virtual string Country { get; set; }

		[ResourceDisplayName (ResourceKey.ProductWeight)]
		public virtual string Weight { get; set; }
	}
}
