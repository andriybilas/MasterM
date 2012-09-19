using System;
using System.Globalization;
using Litium.Common;
using Litium.Common.Validation;
using Litium.Domain.Entities;
using Litium.Domain.Entities.Publishing;
using Litium.Test.Common.Xunit.Base;
using Xunit;

namespace Litium.Test.Domain.Xunit.Validation
{

    public class PublishingValidationTest :  ConversationalTestBase
    {
        private WebSite _webSite;

        public PublishingValidationTest()
        {
            CreateWebSite();
        }

        private void CreateWebSite()
        {
            _webSite = Repository.Data.Get<WebSite>().FirstOrDefault().Value;

            if (_webSite != null)
                return;

            var currency = new Currency { Code = "Rub", TextFormat = "hernia" };
            Repository.Data.Save(currency);
            var language = new Language { Culture = CultureInfo.CurrentCulture, IsDefault = true };
            Repository.Data.Save(language);

            var webSite = new WebSite {
                Currency = currency,
                Language = language,
                DomainName = "Domain",
                Name = "Name" };

            Repository.Data.Save(webSite);
            _webSite = webSite;
        }

        private PageType CreatePageType()
        {
            var pageType =
            new PageType
            {
                Behaviour = new PageBehaviour(),
                Name = "PageType - " + Guid.NewGuid().ToString(),
                PageTypeCategory = PageTypeCategories.Regular
            };
            Repository.Data.Save(pageType);
            return pageType;
        }

        private Page CreatePage()
        {
            var page = new Page
            {
                MenuStatus = MenuStatus.NotInMenu,
                PageType = CreatePageType(),
                WebSiteId = _webSite.Id,
                Name = "Page - " + Guid.NewGuid().ToString(),
                PageTemplate = CreatePageTemplate(),
                Seo = new SeoProfile(),
                UrlName = "www.litium.home.aspx"
            };
            Repository.Data.Save(page);
            return page;
        }

        private PageDisplayTemplate CreatePageTemplate()
        {
            var pageDsiplayTemplate = new PageDisplayTemplate
            {
                FileName = "FileName - " + Guid.NewGuid().ToString(),
                Name = "PageDisplayTemplate - " + Guid.NewGuid().ToString(),
                PageType = CreatePageType(),
                ThumbnailPath = "C:\temp"
            };
            Repository.Data.Save(pageDsiplayTemplate);
            return pageDsiplayTemplate;
        }

        [Fact]
        public void PageInsertTest()
        {
            var page = new Page {
                    Name = String.Empty,
                    WebSiteId = _webSite.Id,
                    MenuStatus = (MenuStatus) 5 };

            Assert.Throws<ValidationArgumentException>(() => Repository.Data.Save(page));
        }

        [Fact]
        public void PageDisplayTemplateInsertTest ()
        {
            var pageDisplayTemplate = new PageDisplayTemplate();

            try
            {
                Repository.Data.Save(pageDisplayTemplate);
            }
            catch (ValidationArgumentException ex)
            {
                Assert.True(ex.ValidationResult.Count == 4);
            }
        }

        [Fact]
        public void PageTypeInsertTest ()
        {
            var pageType = new PageType();

            try
            {
                Repository.Data.Save(pageType);
            }
            catch (ValidationArgumentException ex)
            {
                Assert.True(ex.ValidationResult.Count == 2);
            }
        }

        [Fact]
        public void WebSiteInsertTest ()
        {
            var pageType = new WebSite();

            try
            {
                Repository.Data.Save(pageType);
            }
            catch (ValidationArgumentException ex)
            {
                Assert.True(ex.ValidationResult.Count == 4);
            }
        }

        [Fact]
        public void WorkingCopyInsertTest ()
        {
            var workingCopy = new WorkingCopy();

            try
            {
                Repository.Data.Save(workingCopy);
            }
            catch (ValidationArgumentException ex)
            {
                Assert.True(ex.ValidationResult.Count == 5);
            }
        }
        
        [Fact]
        public void WebSiteDeleteHandlerTest()
        {
            _webSite.IsDefault = true;
            Repository.Data.Save(_webSite);
            Assert.Throws<ValidationArgumentException>(() => Repository.Data.Delete(_webSite));
        }

        [Fact]
        public void CreateIdenticalCurrencyTest()
        {
            string currencyCode = "Currency - " + Guid.NewGuid().ToString();
            var currency1 = new Currency { Code = currencyCode };
            var currency2 = currency1.Clone();
            Repository.Data.Save(currency1);
            Assert.Throws<ValidationArgumentException>(() => Repository.Data.Save(currency2));
        }

        [Fact]
        public void LanguageDefaultRuleDeleteTest()
        {
            var language = Repository.Data.Get<Language>().Where(x => x.IsDefault).FirstOrDefault().Value 
                ?? new Language {Culture = CultureInfo.CurrentCulture, IsDefault = true };

            Assert.Throws<ValidationArgumentException>(() => Repository.Data.Delete(language));
        }

        [Fact]
        public void LanguageUniqueRuleTest()
        {
            var language1 = Repository.Data.Get<Language>().Where(x => x.IsDefault).FirstOrDefault().Value
                ?? new Language { Culture = CultureInfo.CurrentCulture, IsDefault = true };

            var language2 = language1.Clone();
            Assert.Throws<ValidationArgumentException>(() => Repository.Data.Save(language2));
        }

        [Fact]
        public void PageTypeUniqueRuleTest ()
        {
            PageType pageType1 = Repository.Data.Get<PageType>().FirstOrDefault().Value ??
                                CreatePageType();
            var pageType2 = (PageType)pageType1.Clone();
            Assert.Throws<ValidationArgumentException>(() => Repository.Data.Save(pageType2));
        }

        [Fact]
        public void PageUrlUniqueRuleTest ()
        {
            Page page1 = Repository.Data.Get<Page>().FirstOrDefault().Value ??
                        CreatePage();
            var page2 = (Page)page1.Clone();
            Assert.Throws<ValidationArgumentException>(() => Repository.Data.Save(page2));
        }

        [Fact]
        public void CheckForSameWebSiteTest()
        {
            var webSite1 = (WebSite) _webSite.Clone();
            try
            {
                Repository.Data.Save(webSite1);
            }
            catch (ValidationArgumentException ex)
            {
                Assert.True(ex.ValidationResult.Count == 2);
            }
        }
    }
}
