using FluentNHibernate.Mapping;
using Litium.Domain.Entities.Media;

namespace Litium.Domain.Mappings.DataAccess.Media
{
	public sealed class FolderMap : ClassMap<Folder>
	{
		public FolderMap()
		{
			Id(x => x.Id);

			Map(x => x.Name)
				.Not.Nullable();

			References(x => x.Parent);
		}
	}
}