using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Litium.Common;
using Litium.Domain.Entities.ProductCatalog;
using Site.Infrastuctures.ModelHelpers;
using Site.Infrastuctures.ModelHelpers.File;
using Site.Infrastuctures.ModelHelpers.Product;

namespace Web.Site.Controllers
{
    public class PublicController : Controller
    {
        private readonly ProductBagService _productBagService = new ProductBagService();
        private readonly ProductModelService _productService = new ProductModelService();
        
        public ActionResult Index ()
        {
            ProductBagModel model = _productBagService.GetNextProductsBag();
            return View("PublicCampaign", model);
        }

        [HttpPost]
        public ActionResult LoadProductsByCategory (string categoryId, string brand, int? page)
        {
            var entityId = Parse(categoryId);

            ProductBagModel products = _productService.GetProducts(
                new CategoryCommandDescriptor {EnityId = entityId, Page = page ?? 0 , PageSize = 12});

            return PartialView("PublicSite/ProductRepeater", products);
        }

		[HttpPost]
		public ActionResult LoadProductsByBrand (string categoryId, string brand, int? page)
		{
			var entityId = Parse(categoryId);

            ProductBagModel products = _productService.GetProducts(
				new BrandCommandDescriptor { EnityId = entityId, Page = page ?? 0, PageSize = 12, Brand = brand });

			return PartialView("PublicSite/ProductRepeater", products);
		}

		[HttpPost]
        public ActionResult SearchProduct(string searchKeyword, int? page)
		{
            ProductBagModel products = _productService.GetProducts(
                new SearchCommandDescriptor { KeyWord = searchKeyword, Page = page ?? 0, PageSize = 12 });
		    
            ViewBag.SearchKeyword = searchKeyword;

			return View("SearchProduct", products);
		}


        [HttpPost]
        public ActionResult AppendSearchProduct(string searchKeyword, int? page)
        {
            ProductBagModel products = _productService.GetProducts(
                new SearchCommandDescriptor { KeyWord = searchKeyword, Page = page ?? 0, PageSize = 12 });

            return PartialView("PublicSite/ProductRepeater", products);
        }


    	private static Guid Parse(string categoryId)
    	{
    		Guid entityId;
    		Guid.TryParse(categoryId, out entityId);
    		return entityId;
    	}

		[HttpPost]
    	public ActionResult GetCategoryImage (String categoryId)
		{
			var entityId = Parse(categoryId);
			if (entityId == Guid.Empty)
				return Content(String.Empty);
    		return Content ( GetCategoryImageRecursivity(entityId) );
		}

    	private string GetCategoryImageRecursivity(Guid entityId)
    	{
			var category = Repository.Data.Get<Category>(entityId);
    		
			if(category.HasImage)
    			return ImageUploadHelper.Helper.GetImageUrl(entityId, EntityType.Category);
			if (category.Parent != null)
				return GetCategoryImageRecursivity(category.Parent.Id);
    		return String.Empty;
    	}

		public ActionResult FilterByBrandName( Guid categoryId )
		{
            return PartialView("PublicSite/Controls/BrandFilter", _productService.GetBrandsSelectedList(categoryId));
    	}

		public ActionResult LoyalityProgram()
		{
			return View();
		}

		public ActionResult HowToOrder ()
		{
			return View();
		}

    }
}
