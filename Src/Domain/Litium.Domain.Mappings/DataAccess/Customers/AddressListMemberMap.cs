using FluentNHibernate.Mapping;
using Litium.Domain.Entities.Customers;

namespace Litium.Domain.Mappings.DataAccess.Customers
{
    class AddressListMemberMap : ClassMap<AddressListMember>
    {
        public AddressListMemberMap()
        {
            Id(x => x.Id);
            Map(x => x.Email);
            Map(x => x.Subscribed);
            Map(x => x.SubscribedDateTime);
            References(x => x.Person).Cascade.All();
        }
    }
}
