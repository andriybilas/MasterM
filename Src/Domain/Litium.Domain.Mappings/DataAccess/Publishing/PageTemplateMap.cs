using FluentNHibernate.Mapping;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.Mappings.DataAccess.Publishing
{
	public sealed class PageTemplateMap : ClassMap<PageDisplayTemplate>
	{
		public PageTemplateMap()
		{
			Id(x => x.Id);

			Map(x => x.Name)
				.Not.Nullable();
			Map(x => x.FileName)
				.Not.Nullable();
			Map(x => x.ThumbnailPath)
				.Not.Nullable();

			References(x => x.PageType)
				.Not.Nullable()
				.LazyLoad();
		}
	}
}