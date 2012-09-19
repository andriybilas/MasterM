using System;
using FluentNHibernate.Testing;
using Litium.Domain.Entities.Publishing;
using Litium.Test.Domain.Xunit.Mappings.Base;
using Xunit;

namespace Litium.Test.Domain.Xunit.Mappings
{
	public class PublishingMappingTest : MappingTestBase
	{

		[Fact]
		public PageDisplayTemplate PageDisplayTemplate()
		{
			return new PersistenceSpecification<PageDisplayTemplate>(Session)
				.CheckProperty(x => x.Name, "Template 66")
				.CheckProperty(x => x.ThumbnailPath, "Thumbnail 77")
				.CheckProperty(x => x.FileName, "Filename 88")
				.CheckProperty(x => x.PageType, PageType())
				.VerifyTheMappings();
		}

		[Fact]
		public PageType PageType()
		{
			//IEnumerable<PageType> pageTypes = new[]
			//                                    {
			//                                        new PageType {Name = "PageType #31", Behaviour = new PageBehaviour()},
			//                                        new PageType {Name = "PageType #32", Behaviour = new PageBehaviour()},
			//                                        new PageType {Name = "PageType #33", Behaviour = new PageBehaviour()}
			//                                    };
			//Repository.Save(pageTypes);
			var uniquePageTypeName = "PageType_" + Guid.NewGuid();
			return new PersistenceSpecification<PageType>(Session)
				.CheckProperty(x => x.Name, uniquePageTypeName)
				.CheckProperty(x => x.Behaviour, new PageBehaviour
				{
					CanBeDeleted = true,
					CanBeInMenu = false,
					CanBeInSiteMap = true,
					CanBeMasterPage = false
					//PageVersionsToKeep = 100,
					//PossibleChildPageTypes = pageTypes.ToSet(),
					//PossibleParentPageTypes = pageTypes.ToSet(),
				})
				.VerifyTheMappings();
		}

		[Fact]
		public Page RegularPage()
		{
			var pageTemplate = PageDisplayTemplate();
			var pageType = PageType();
			var webSite = WebSite();
			return new PersistenceSpecification<Page>(Session)
				.CheckProperty(x => x.Name, "Page 1 name")
				.CheckReference(x => x.PageTemplate, pageTemplate)
				.CheckReference(x => x.PageType, pageType)
				.CheckProperty(x => x.Seo, new SeoProfile
				{
					ChangeFrequency = ChangeFrequency.Monthly,
					Description = "seo-desc",
					Keywords = "seo-key",
					Priority = 0.7M,
					Title = "seo-title"
				})
				.CheckProperty(x => x.UrlName, "page_1_url")
				.CheckProperty(x => x.WebSiteId, webSite.Id)
				.VerifyTheMappings();
		}

		[Fact]
		public WebSite WebSite()
		{
			var common = new CommonMappingTest();
			var currency = common.MapCurrency();
			var language = common.MapLanguage();
			return new PersistenceSpecification<WebSite>(Session)
				.CheckReference(x => x.Currency, currency)
				.CheckProperty(x => x.DomainName, "Domain 12")
				.CheckProperty(x => x.GoogleAnalyticsAccount, "ga script")
				.CheckProperty(x => x.IconPath, "icon")
				.CheckProperty(x => x.IncludeVat, true)
				.CheckProperty(x => x.IsDefault, true)
				.CheckReference(x => x.Language, language)
				.CheckProperty(x => x.Name, "web 12 site name")
				.CheckProperty(x => x.UsePagePermissionsInGui, true)
				.CheckProperty(x => x.UsePageResponsibilityInGui, true)
				.CheckProperty(x => x.FrameworkPath, "framework")
				//startpage
				//toppage
				.VerifyTheMappings();
		}
	}
}