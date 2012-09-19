using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Litium.Common;
using Litium.Common.WorkUnit;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.Product;
using Site.Infrastuctures.Security;
using Telerik.Web.Mvc;

namespace Web.Site.Controllers
{
	[Security(Roles = UserRole.Administer)]
	public class ProductController : Controller
    {
    	public class PublishedProduct
		{
			public string Id { get; set; }
			public bool Published { get; set; }
		}

		[AcceptVerbs (HttpVerbs.Post)]
		public ActionResult Remove( string[] fileNames )
		{
			foreach (var fullName in fileNames)
			{
				var fileName = Path.GetFileName (fullName);
				var physicalPath = Path.Combine (Server.MapPath (StoreResourceStrings.PublicUploadFolderPath), fileName);

				if (System.IO.File.Exists (physicalPath))
				{
					System.IO.File.Delete (physicalPath);
				}
			}

			return Content (String.Empty);
		}

		private IEnumerable<ProductModel> GetProducts(GridCommand command)
		{
            IEnumerable<ProductModel> products = ProductModelService.Service.GetProducts(command);
			return products;
		}

		private GridModel<ProductModel> GetProductrsGridModel(GridCommand command)
		{
			return new GridModel<ProductModel> {Data = GetProducts (command).AsQueryable(), Total = GetCount(command)};
		}

        public ActionResult Index()
        {
            ProductModelService.Service.ClearSearchCriteria();
            return View("Products", GetProducts(new GridCommand { Page = 1, PageSize = 20 }));
        }

        [GridAction(EnableCustomBinding = true)]
		public ActionResult GetProductsAsync (GridCommand command)
        {
			return View ("Products", GetProductrsGridModel (command));
		}

        private int GetCount(GridCommand command)
        {
            return ProductModelService.Service.GetProductsCount(command);
        }

		[GridAction]
		public ActionResult InsertProductAsync ()
		{
			var productModel = new ProductModel ();

			if (TryUpdateModel (productModel))
			{
                using (var uow = new UnitOfWork(UnitOfWorkScopeType.New))
                {
                    var product = productModel.GetProduct();
                    product.CreateDate = DateTime.Now;
                    product.UpdateDate = DateTime.Now;
                    
                    //var productProperty = new ProductProperty {
                    //    Brend = productModel.ProductProperty.Brend,
                    //    Capacity = productModel.ProductProperty.Capacity,
                    //    Country = productModel.ProductProperty.Country,
                    //    Weight = productModel.ProductProperty.Weight
                    //};

                    Repository.Data.Save(product.ProductProperty);
                    //product.ProductProperty = productProperty;
                    Repository.Data.Save(product);
                    uow.Commit();
                }
			}

            return GetProductsAsync(new GridCommand { Page = 1, PageSize = 20 });
		}

		[GridAction]
		public ActionResult UpdateProductAsync()
		{
			var productModel = new ProductModel();

			if (TryUpdateModel (productModel))
			{
                using (var uow = new UnitOfWork())
                {
                    var product = Repository.Data.Get<Product>(productModel.Id);
                    product.CopyFrom(productModel, true);
                    product.UpdateDate = DateTime.Now;
                    Repository.Data.Save(product);
                    uow.Commit();
                }
			}
			ViewBag.Message = WebStroreResource.ProductUpdatedSuccesfull;
            return GetProductsAsync(new GridCommand { Page = 1, PageSize = 20 });
		}

		[GridAction]
        public ActionResult DeleteProductAsync(string productId)
		{
		    Guid entityId;
		    Guid.TryParse(productId, out entityId);

			if (entityId != Guid.Empty)
			{
				var product = Repository.Data.Get<Product> (entityId);
				Repository.Data.Delete(product);
			}

            return GetProductsAsync(new GridCommand { Page = 1, PageSize = 20 });
		}

		[AcceptVerbs (HttpVerbs.Post)]
		public ActionResult GetCountriesAsync ()
		{
			CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

			var countries = cultures.Select(x => new {
					Id = x.Name,
					Country = x.DisplayName
				});

			return new JsonResult { Data = new SelectList (countries.ToList (), "Id", "Country") };
		}

		[AcceptVerbs (HttpVerbs.Post)]
		public void PublishProductsAsync( PublishedProduct [] products )
        {
			var repositoryProducts = new List<Product>();
			foreach (var publishedProduct in products)
			{
				Guid id;
				Guid.TryParse(publishedProduct.Id, out id);
				if (id != Guid.Empty)
				{
					var product = Repository.Data.Get<Product>(id);

					if (product.Published != publishedProduct.Published)
					{
						product.Published = publishedProduct.Published;
						repositoryProducts.Add (product);
					}
				}
			}

			if (repositoryProducts.Count > 0)
				Repository.Data.Save (repositoryProducts);
        }
    }
}
