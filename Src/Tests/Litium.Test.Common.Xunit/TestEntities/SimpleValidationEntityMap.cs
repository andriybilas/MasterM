using FluentNHibernate.Mapping;

namespace Litium.Test.Common.Xunit.TestEntities
{
	public sealed class SimpleValidationEntityMap : ClassMap<SimpleValidationEntity>
	{
		public SimpleValidationEntityMap()
		{
			Table("SimpleEntityTable");
			Id(x => x.Id)
				.Column("EntityId")
				.GeneratedBy.Guid();
			Map(x => x.DisplayName).Column("Name");
			Map(x => x.ExtraInfo).Column("Column1");
		}
	}
}
