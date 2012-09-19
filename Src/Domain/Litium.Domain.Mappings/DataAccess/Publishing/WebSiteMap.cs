using FluentNHibernate.Mapping;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.Mappings.DataAccess.Publishing
{
	public sealed class WebSiteMap : ClassMap<WebSite>
	{
		public WebSiteMap()
		{
			Id(x => x.Id);

			Map(x => x.Name)
				.Not.Nullable();
			References(x => x.Language)
				.Not.Nullable();
			Map(x => x.DomainName)
				.Not.Nullable();
			Map(x => x.GoogleAnalyticsAccount);
			Map(x => x.IsDefault);
			Map(x => x.UsePageResponsibilityInGui)
				.Column("IsPageResponsibilityActive");
			References(x => x.StartPage);
			Map(x => x.UsePagePermissionsInGui)
				.Column("IsPagePermissionStepActive");
			References(x => x.Currency)
				.Not.Nullable();
			Map(x => x.IncludeVat);
			Map(x => x.IconPath);
			Map(x => x.FrameworkPath);
		}
	}
}