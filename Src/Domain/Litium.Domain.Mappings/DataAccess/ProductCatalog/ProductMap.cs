using FluentNHibernate.Mapping;
using Litium.Domain.Entities.ProductCatalog;

namespace Litium.Domain.Mappings.DataAccess.ProductCatalog
{
	public class ProductMap : ClassMap<Product>
	{
		public ProductMap()
		{
			Id(x => x.Id);
			Map(x => x.Name).Not.Nullable();
		    Map(x => x.Article);
			Map(x => x.Description);
			Map(x => x.Price).Not.Nullable();
			Map(x => x.StockBalance).Not.Nullable();
			Map(x => x.Published).Not.Nullable();
			Map(x => x.CreateDate).Nullable();
			Map(x => x.UpdateDate).Nullable();
			Map(x => x.LastSynchDate).Nullable();
			Map(x => x.HasImage);
			
			HasManyToMany (x => x.ProductSets).ParentKeyColumn ("ProductId")
			.ChildKeyColumn ("ProducttSetId").Table("ECommerce_ProductToProductSet");

            HasMany (x => x.Campaigns).KeyColumn ("CampaignId").KeyNullable ();
			References (x => x.Category).Nullable ();
			References(x => x.ProductProperty).Nullable().Cascade.All();
		}
	}
}
