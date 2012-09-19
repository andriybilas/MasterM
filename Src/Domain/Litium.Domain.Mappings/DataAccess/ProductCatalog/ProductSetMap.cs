using FluentNHibernate.Mapping;
using Litium.Domain.Entities.ProductCatalog;

namespace Litium.Domain.Mappings.DataAccess.ProductCatalog
{
	public class ProductSetMap : ClassMap<ProductSet>
	{
		public ProductSetMap()
		{
			Id(x => x.Id);
			Map(x => x.Name).Not.Nullable();
			Map(x => x.Description);
			Map(x => x.HasImage);
			References(x => x.Campaign);
			
			HasManyToMany (x => x.Products)
				.ParentKeyColumn ("ProducttSetId")
				.ChildKeyColumn ("ProductId")
				.Table ("ECommerce_ProductToProductSet")
				.Inverse ();

		}
	}
}
