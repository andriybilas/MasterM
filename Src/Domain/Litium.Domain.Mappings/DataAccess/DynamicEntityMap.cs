using FluentNHibernate.Mapping;
using Litium.Common.Entities;
using Litium.Common.Entities.FieldFramework;
using Litium.Infrastructure.DataAccess.UserTypes;

namespace Litium.Domain.Mappings.DataAccess
{
	public abstract class DynamicEntityMap<T> : ClassMap<T> where T : DynamicEntity
	{
		protected DynamicEntityMap()
		{
			Join("Foundation_Metadata", x => {
			            x.Fetch.Join();
			            x.KeyColumn("Id");
			            x.Map(y => y.Fields, "Metadata").CustomType<JsonUserType<FieldCollection>>(); });
		}
	}
}