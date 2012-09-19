using System;
using System.ComponentModel.DataAnnotations;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;

namespace Litium.Domain.Entities.Metadata
{
	public class CategoryMetadata
	{
		[Required (ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof (DomainNotification))]
		[ResourceDisplayName (ResourceKey.CategoryName)]
		public virtual String Name { get; set; }

		[ResourceDisplayName (ResourceKey.Description)]
		public virtual String Description { get; set; }

		[ResourceDisplayName (ResourceKey.CategoryParent)]
		public virtual Category Parent { get; set; }
	}
}
