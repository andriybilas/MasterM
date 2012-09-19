using FluentNHibernate.Mapping;
using Litium.Domain.Entities.ECommerce;

namespace Litium.Domain.Mappings.DataAccess.ECommerce
{
    class PaymentMethodMap : ClassMap<PaymentMethod>
    {
        public PaymentMethodMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Description);
            Map(x => x.Cost);
        }
    }
}
