using System;
using FluentNHibernate.Mapping;
using Litium.Domain.Entities.Media;
using Litium.Domain.Entities.ProductCatalog;

namespace Litium.Domain.Mappings.DataAccess.Media
{
	public sealed class FileMap : ClassMap<File>
	{
		public FileMap()
		{
			Id(x => x.Id);
			Map(x => x.DisplayName).Not.Nullable();
			Map(x => x.Name);
			Map(x => x.ContentType).Not.Nullable();
		    Map(x => x.ResizedTo).Not.Nullable();
			Map(x => x.Size);
			Map(x => x.StoragePath);
			Map(x => x.EntityId);
        }
	}
}