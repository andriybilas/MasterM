using Litium.Domain.Entities.ECommerce;
using Litium.Domain.Entities.ProductCatalog;

namespace Litium.Domain.Mappings.DataAccess.ProductCatalog
{
	public class CampaignMap : DynamicEntityMap<Campaign>
	{
		public CampaignMap()
		{
			Id(x => x.Id);
			Map(x => x.Name).Not.Nullable();
			Map(x => x.Description);
			Map(x => x.Active).Not.Nullable();
			Map(x => x.StartDate).Nullable();
			Map(x => x.EndDate).Nullable();
		    Map(x => x.HasImage);
		}
	}
}
