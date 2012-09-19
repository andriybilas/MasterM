using FluentNHibernate.Mapping;
using Litium.Domain.Entities.ProductCatalog;

namespace Litium.Domain.Mappings.DataAccess.ProductCatalog
{
	public class CategoryMap : ClassMap<Category>
	{
		public CategoryMap()
		{
			Id(x => x.Id);
			Map(x => x.Name).Not.Nullable();
			Map(x => x.Description);
			Map(x => x.HasImage);
			References(x => x.Parent);
        }
	}
}
