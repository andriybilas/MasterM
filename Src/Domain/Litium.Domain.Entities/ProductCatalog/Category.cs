using System;
using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Domain.Entities.Metadata;

namespace Litium.Domain.Entities.ProductCatalog
{
	[MetadataType (typeof (CategoryMetadata))]
	public class Category : Entity, IImage
	{
		public virtual String Name { get; set; }

		public virtual String Description { get; set; }

		public virtual Category Parent { get; set; }

        public virtual bool HasImage { get; set; }

        public override object Clone ()
        {
            return new Category { Name = Name, HasImage = HasImage, Description = Description };
        }

		public override object ValidationCopy()
		{
			return new Category { Name = Name, HasImage = HasImage, Description = Description };
		}
	}
}