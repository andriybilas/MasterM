using FluentNHibernate.Mapping;
using Litium.Domain.Entities;

namespace Litium.Domain.Mappings.DataAccess
{
	public sealed class CurrencyMap : ClassMap<Currency>
	{
		public CurrencyMap()
		{
			Id(x => x.Id);

			Map(x => x.Code)
				.Not.Nullable();
			Map(x => x.TextFormat);
		}
	}
}