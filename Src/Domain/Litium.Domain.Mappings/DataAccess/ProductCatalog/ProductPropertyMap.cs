using FluentNHibernate.Mapping;
using Litium.Domain.Entities.ECommerce;
using Litium.Domain.Entities.ProductCatalog;

namespace Litium.Domain.Mappings.DataAccess.ProductCatalog
{
	public class ProductPropertyMap : ClassMap<ProductProperty>
	{
		public ProductPropertyMap()
		{
			Id(x => x.Id);
			Map(x => x.Brend);
			Map(x => x.Capacity);
			Map(x => x.Country);
			Map(x => x.Weight);
		}
	}
}
