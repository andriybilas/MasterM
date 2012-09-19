using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Domain.Entities.Media;
using Litium.Resources;

namespace Litium.Domain.Entities.ProductCatalog
{
	public class ProductSet : Entity, IImage
	{
		[Required (ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof (DomainNotification))]
		[ResourceDisplayName (ResourceKey.ProductSetName)]
		public virtual String Name { get; set; }
		
		[ResourceDisplayName (ResourceKey.ProductSetDescription)]
		public virtual String Description { get; set; }

		public virtual IList<Product> Products { get; set; }

		public virtual Campaign Campaign { get; set; }

        public virtual bool HasImage { get; set; }

		public override object ValidationCopy()
		{
			return Clone();
		}
	}
}