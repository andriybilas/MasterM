using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Litium.Common;
using Litium.Common.WorkUnit;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.Product;
using Site.Infrastuctures.Security;

namespace Web.Site.Controllers
{
	[Security(Roles = UserRole.Administer)]
	public class ProductSetController : Controller
	{
	    public ActionResult Index ()
		{
            ProductModelService.Service.ClearSearchCriteria();
			return View("ProductSetView", GetProductSets());
		}

		private IEnumerable<ProductSet> GetProductSets()
		{
			return Repository.Data.Get<ProductSet>().All();
		}

		public void SetProductsSearchCriteriaAsync (string productSetId)
		{
    	    ProductModelService.Service.SearchCriteria(FilterBy.ProductSet, Guid.Parse(productSetId));
		}

		public ActionResult CreateProductSet(string name, string description)
		{
		    var productSet = new ProductSet();

            if (TryUpdateModel(productSet))
            {
                Repository.Data.Save(productSet);                
            }

			return View ("ProductSetView", GetProductSets ());
		}

		public ActionResult LoadNewProductSetForm ()
		{
			return PartialView("Partial/NewProductSetForm");
		}

		public ActionResult OpenEditWindow()
		{
			ViewBag.WindowTitle = StoreResourceStrings.CreateNewProductSet;
			ViewBag.ActionName = "LoadNewProductSetForm";
			ViewBag.ControllerName = "ProductSet";
		    ViewBag.Width = 325;
			return PartialView("Partial/Window");
		}

		[HttpPost]
		public void DeleteProductSet (string productSetId)
		{
            using (var uow = new UnitOfWork(UnitOfWorkScopeType.New))
            {
                var productSet = Repository.Data.Get<ProductSet>(Guid.Parse(productSetId));

                foreach (var product in productSet.Products)
                {
                    product.ProductSets.Remove(productSet);
                    Repository.Data.Save(product);
                }

                productSet.Products.Clear();
                Repository.Data.Delete(productSet);
                uow.Commit();
            }
		}

        [HttpPost]
        public string AssignProductsAsync(List<String> productIds, string productSetId)
        {
            Guid entityId;
            if (!Guid.TryParse(productSetId, out entityId) || entityId == Guid.Empty)
                return "false";

            using (var uow = new UnitOfWork(UnitOfWorkScopeType.New))
            {
                var productSetEntity = Repository.Data.Get<ProductSet>(Guid.Parse(productSetId));
                if (productSetEntity.Products == null)
                {
                    productSetEntity.Products = new List<Product>();
                }
                
                foreach (var productId in productIds.Distinct())
                {
                    var product = Repository.Data.Get<Product>(Guid.Parse(productId));
                    product.ProductSets.Add(productSetEntity);
                    Repository.Data.Save(product);
                }

                uow.Commit();
            }

            return "true";
        }

        public ActionResult SaveUpdateProductSet (Guid productSetId)
        {
            var productSet = new ProductSet();
            if (TryUpdateModel(productSet))
            {
                var originProductSet = Repository.Data.Get<ProductSet>(productSetId);
                originProductSet.Name = productSet.Name;
                originProductSet.Description = productSet.Description;
                Repository.Data.Save(originProductSet);
            }
            return Index();
        }

	    public ActionResult LoadEditProductSetForm(Guid productSetId)
	    {
	        var model = Repository.Data.Get<ProductSet>(productSetId);
	        return PartialView("EditorTemplates/EditProductSet", model);
	    }
	}
}