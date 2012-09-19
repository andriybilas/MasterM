using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Litium.Domain.Entities.ECommerce;

namespace Litium.Domain.Mappings.DataAccess.ECommerce
{
    class DeliveryMethodMap : ClassMap<DeliveryMethod>
    {
        public DeliveryMethodMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Description);
            Map(x => x.Cost);
        }
    }
}
