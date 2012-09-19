using FluentNHibernate.Mapping;

namespace Litium.Test.Common.Xunit.TestEntities
{
	public sealed class SimpleEventEntityMap : ClassMap<SimpleEventEntity>
	{
		public SimpleEventEntityMap()
		{
			Table("SimpleEntityTable");
			Id(x => x.Id)
				.Column("EntityId")
				.GeneratedBy.Guid();
			Map(x => x.Name)
				.Nullable();
		}
	}
}