using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.Mappings.DataAccess.Publishing
{
	public sealed class PageMap : PageBaseMap<Page>
	{
		public PageMap()
		{
			Id(x => x.Id);

			Map(x => x.EndPublishDateTime);
			Map(x => x.Index);
			Map(x => x.IsInSitemap);
			//Map(x => x.IsMasterPage);
			//Map(x => x.IsTopSharedPage);
			//Map(x => x.IsTranslationOriginalPage);
			//Map(x => x.Locked);
			//References(x => x.LockedBy);
			//References(x => x.MasterPage);
			Map(x => x.MenuStatus);
			//Map(x => x.MovedToTrashcan);
			//References(x => x.MovedToTrashcanBy);
			Map(x => x.Status);
			References(x => x.Parent);
			References(x => x.PageType)
				.Not.Nullable();
			//References(x => x.PublishedBy);
			References(x => x.PageResponsible);
			//Map(x => x.SharesStructure);
			Map(x => x.StartPublishDateTime);
			//References(x => x.TranslationOriginalPage);
			//Map(x => x.TranslationStatus);
			Map(x => x.UrlName);
			//Map(x => x.ProductCatalogItemId);
			//Map(x => x.ProductCatalogLanguageId);
			//Map(x => x.ShowProductGroupRecursively);
			Map(x => x.WebSiteId)
				.Not.Nullable();
		}
	}
}