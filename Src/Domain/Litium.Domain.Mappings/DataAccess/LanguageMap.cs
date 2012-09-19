using FluentNHibernate.Mapping;
using Litium.Domain.Entities;

namespace Litium.Domain.Mappings.DataAccess
{
	public sealed class LanguageMap : ClassMap<Language>
	{
		public LanguageMap()
		{
			Id(x => x.Id);

			Map(x => x.Culture)
				.Not.Nullable();
			Map(x => x.IsDefault);
		}
	}
}