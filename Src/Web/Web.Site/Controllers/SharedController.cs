using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Litium.Common;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.Media;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers;
using Site.Infrastuctures.ModelHelpers.File;
using Site.Infrastuctures.ModelHelpers.Product;
using Site.Infrastuctures.Security;
using Web.Site.Models;

namespace Web.Site.Controllers
{
	public class SharedController : Controller
	{
        private readonly ProductModelService _productService = new ProductModelService();
        public ActionResult Index()
        {
            IEnumerable<Category> categories = Repository.Data.Get<Category>()
                .Where(x => x.Parent == null).All().OrderBy(x => x.Name);

			var model = categories.Select(x => new CategoryModel {
				Name = x.Name,
				Id = x.Id,
				HasChild = ChildExist(x),
			}).OrderBy(x => x.Name);

			return PartialView("PublicSite/CategoryTree", model);
        }

		private bool ChildExist( Category category )
		{
			IEnumerable<Category> categories =
				Repository.Data.Get<Category>().Where(x => x.Parent.Id == category.Id).All();

			return categories.Count() > 0;
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult GetCategories(string instanceName)
		{
			List<Category> categories = Repository.Data.Get<Category>().All().ToList();
			List<CategoryModel> categoriesModel =
				(from entity in categories let model = new CategoryModel() select model.Copy(entity)).ToList();

			if (instanceName.Equals("new-category", StringComparison.InvariantCultureIgnoreCase))
			{
				categoriesModel.Insert(0, new CategoryModel
					{
						Id = null,
						Name = StoreResourceStrings.TopCategory
					});
			}
			else if (instanceName.Equals("filter-productset", StringComparison.InvariantCultureIgnoreCase))
			{
				categoriesModel.Insert(0, new CategoryModel
					{
						Id = Guid.Empty,
						Name = StoreResourceStrings.All
					});
				categoriesModel.Insert(1, new CategoryModel
					{
						Id = Guid.Parse(StoreResourceStrings.UncategorizedId),
						Name = StoreResourceStrings.Uncategorized
					});
			}

			return new JsonResult
			{
				Data = new SelectList(categoriesModel.ToList(), "Id", "Name")
			};
		}

		public JsonResult GetEntityImage( string entityId, string entityType )
		{
			Guid productEntityId;
			String imageUrl = String.Empty;

			if (!Guid.TryParse (entityId, out productEntityId) && productEntityId == Guid.Empty)
                return new JsonResult { Data = String.Empty };

            EntityType type;  
            if(!EntityType.TryParse(entityType, true, out type) )
                return new JsonResult { Data = String.Empty };
		    
            imageUrl = ImageUploadHelper.Helper.GetImageUrl(productEntityId, type);
            return new JsonResult { Data = imageUrl };
		}

		[Security(Roles = UserRole.Administer)]
		public ActionResult UploadImageAsync(IEnumerable<HttpPostedFileBase> attachments, string entityId, string entityType)
		{
		    Guid entityIdGuid;
		    Guid.TryParse(entityId, out entityIdGuid);

            if(entityIdGuid == Guid.Empty)
            {
                ModelState.AddModelError("entityId", WebStroreResource.EnityGuidEmptyException);
                return Content(WebStroreResource.EnityGuidEmptyException);
            }

            EntityType type;  
            if(EntityType.TryParse(entityType, true, out type) )
            {
                switch (type)
                {
                    case EntityType.Product:
                        {
                            return Content(ImageUploadHelper.Helper.UploadProductImageAsync<Product>(attachments, entityId, ResizedVersion.To100x125));
                        }
                    case EntityType.Category:
                        {
                            return Content(ImageUploadHelper.Helper.UploadProductImageAsync<Category>(attachments, entityId, ResizedVersion.To620x195));
                        }
                    case EntityType.Campaign:
                        {
                            return Content(ImageUploadHelper.Helper.UploadProductImageAsync<Campaign>(attachments, entityId, ResizedVersion.To620x195));
                        }
                    case EntityType.ProductSet:
                        {
                            return Content(ImageUploadHelper.Helper.UploadProductImageAsync<ProductSet>(attachments, entityId, ResizedVersion.To620x195));
                        }
                }
            }
            	
            return Content(String.Empty);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void FilterByProduct(String produtSearch, String CategoryDropDown)
        {
            ProductModelService.Service.SearchCriteria(produtSearch);
            Guid categoryId;
            Guid.TryParse(CategoryDropDown, out categoryId);
            ProductModelService.Service.SearchCriteria(FilterBy.Category, categoryId);
        }
	}
}
