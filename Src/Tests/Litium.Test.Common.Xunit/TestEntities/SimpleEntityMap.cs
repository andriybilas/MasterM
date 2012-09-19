using FluentNHibernate.Mapping;
using Litium.Infrastructure.DataAccess.UserTypes;

namespace Litium.Test.Common.Xunit.TestEntities
{
	public sealed class SimpleEntityMap : ClassMap<SimpleEntity>
	{
		public SimpleEntityMap()
		{
			Table("SimpleEntityTable");
			Id(x => x.Id)
				.Column("EntityId")
				.GeneratedBy.Guid();
			Map(x => x.Name)
				.Nullable();
			Map(x => x.Column1)
				.Nullable();
			Map(x => x.Column2)
				.Nullable();
			Map(x => x.Column3)
				.Nullable();
			Map(x => x.Column4)
				.Nullable();
			Map(x => x.Culture)
				.CustomType<CultureInfoType>()
				.Nullable();
		}
	}
}