using FluentNHibernate.Mapping;
using Litium.Domain.Entities.ECommerce;

namespace Litium.Domain.Mappings.DataAccess.ECommerce
{
	class OrderMap : ClassMap<Order>
	{
		public OrderMap()
		{
			Id(x => x.Id);
			Map(x => x.CreateDate).Not.Nullable();
			Map(x => x.LastSynchDate).Nullable();
			Map (x => x.OrderState).Update();
			Map (x => x.OrderSumma).Not.Nullable();
			Map (x => x.OrderTotal).Not.Nullable();
			Map (x => x.Description);
			Map(x => x.OrderNumber);
			References(x => x.Customer).Not.Nullable();
			References(x => x.DeliveryMethod).Not.Nullable();
			References(x => x.PaymentMethod).Nullable();
			HasMany(x => x.OrderProducts).KeyColumn("OrderId").Cascade.AllDeleteOrphan();
		}
	}
}
