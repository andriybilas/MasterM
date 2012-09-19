using FluentNHibernate.Mapping;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.Mappings.DataAccess.Publishing
{
	public abstract class PageBaseMap<T> : ClassMap<T>
		where T : PageBase
	{
		protected PageBaseMap()
		{
			Component(x => x.Seo, m =>
			                      	{
			                      		m.Map(x => x.ChangeFrequency);
			                      		m.Map(x => x.Description);
			                      		m.Map(x => x.Keywords);
			                      		m.Map(x => x.Priority);
			                      		m.Map(x => x.Title);
			                      	});
			Map(x => x.Name)
				.Not.Nullable();
			References(x => x.PageTemplate)
				.Not.Nullable();
		}
	}
}
