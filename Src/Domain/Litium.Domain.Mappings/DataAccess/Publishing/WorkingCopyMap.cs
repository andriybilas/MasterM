using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.Mappings.DataAccess.Publishing
{
	public sealed class WorkingCopyMap : PageBaseMap<WorkingCopy>
	{
		public WorkingCopyMap()
		{
			Id(x => x.Id);

			References(x => x.Page);
			Map(x => x.PublishDateTime);
			Map(x => x.Status);
			Map(x => x.UrlName)
				.Not.Nullable();
		}
	}
}