using FluentNHibernate.Mapping;

namespace Litium.Test.Common.Xunit.TestEntities
{
	public sealed class SimpleCacheEntityMap : ClassMap<SimpleCacheEntity>
	{
		public SimpleCacheEntityMap()
		{
			Table("SimpleEntityTable");
			Id(x => x.Id)
				.Column("EntityId")
				.GeneratedBy.Guid();
			Map(x => x.Name)
				.Nullable();

			//Replaced by automapping convention for all entities
			//Cache.ReadWrite();
		}
	}
}
