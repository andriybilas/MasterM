using FluentNHibernate.Mapping;
using Litium.Domain.Entities.ECommerce;

namespace Litium.Domain.Mappings.DataAccess.ECommerce
{
    class OrderProductMap : ClassMap<OrderProduct>
    {
        public OrderProductMap()
        {
            Id(x => x.Id);
            Map(x => x.ProductName).Not.Nullable();
            Map(x => x.Count).Not.Nullable();
            Map(x => x.Price).Not.Nullable();
            Map(x => x.CampaignPrice);
            Map(x => x.Summa).Not.Nullable();
            References(x => x.Product);
        }
    }
}
