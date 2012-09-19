using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Litium.Common;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.Campaign;
using Site.Infrastuctures.Utility;
using Telerik.Web.Mvc;
using log4net;

namespace Site.Infrastuctures.ModelHelpers.Product
{
    public class ProductModelService
    {
        private static ProductModelService _instance;

        private int _lastRequestCount;

        public static ProductModelService Service
        {
            get { return _instance ?? (_instance = new ProductModelService()); }
        }

        #region BacOffice UI

        private List<Litium.Domain.Entities.ProductCatalog.Product> GetData(GridCommand command)
        {
            IQueryable<Litium.Domain.Entities.ProductCatalog.Product> products = Repository.Data.Get<Litium.Domain.Entities.ProductCatalog.Product>().All().AsQueryable();
            products = products.ApplyFiltering(command.FilterDescriptors);
            products = products.ApplySorting(command.GroupDescriptors, command.SortDescriptors);
            products = Filter(products);
            _lastRequestCount = products.Count();
            products = products.ApplyPaging(command.Page, command.PageSize);

            //Grouping is switched off 
            return products.ToList();
        }

        public string SearchProductName { get; private set; }

        public Guid EntityId { get; private set; }

        public FilterBy FilterBy { get; private set; }

        private IQueryable<Litium.Domain.Entities.ProductCatalog.Product> Filter(IQueryable<Litium.Domain.Entities.ProductCatalog.Product> products)
        {
            if (FilterBy == FilterBy.Category)
                products = FilterByCategory(products);
            if (FilterBy == FilterBy.ProductSet)
                products = FilterByProductSet(products);
            return FilterByName(products);
        }

        private IQueryable<Litium.Domain.Entities.ProductCatalog.Product> FilterByProductSet(IQueryable<Litium.Domain.Entities.ProductCatalog.Product> products)
        {
            if (EntityId == Guid.Empty)
                return products;
            else
            {
                var prod = Repository.Data.Get<ProductSet>(EntityId);
                if (prod == null)
                    return new List<Litium.Domain.Entities.ProductCatalog.Product>().AsQueryable();
                return prod.Products.AsQueryable();
            }
        }

        private IQueryable<Litium.Domain.Entities.ProductCatalog.Product> FilterByCategory(IQueryable<Litium.Domain.Entities.ProductCatalog.Product> products)
        {
            if (EntityId == Guid.Parse(StoreResourceStrings.UncategorizedId))
                return products.Where(prod => prod.Category == null);
            else if (EntityId == Guid.Empty)
                return products;
            else
                return products.Where(prod => prod.Category != null && prod.Category.Id == EntityId);
        }

        private IQueryable<Litium.Domain.Entities.ProductCatalog.Product> FilterByName(IQueryable<Litium.Domain.Entities.ProductCatalog.Product> products)
        {
            if (string.IsNullOrWhiteSpace(SearchProductName))
                return products;

            var collection = products.Where(prod => prod.Name.ContainsIgnoreKey(SearchProductName));
            products = collection;

            return products;
        }

        private ProductModel ConvertFrom(Litium.Domain.Entities.ProductCatalog.Product product)
        {
            return new ProductModel(product);
        }

        public void SearchCriteria(FilterBy filterBy, Guid id)
        {
            EntityId = id;
            FilterBy = filterBy;
        }

        public void SearchCriteria(String productName)
        {
            SearchProductName = productName;
            FilterBy = FilterBy.Name;
        }

        public void ClearSearchCriteria()
        {
            EntityId = Guid.Empty;
            FilterBy = FilterBy.None;
        }

        #endregion BacOffice UI

        private ProductBagModel GetData(ProductCommandDescriptor descriptor)
        {
            IQueryable<Litium.Domain.Entities.ProductCatalog.Product> products = null;
            var bagModel = new ProductBagModel();

            if (descriptor is CategoryCommandDescriptor)
            {
                products = GetDataByCategory(descriptor);
                bagModel.TypeRequest = RequestType.Category;
            }

            if (descriptor is ProductSetCommandDescriptor)
                products = GetDataByProductSet(descriptor);

            if (descriptor is SearchCommandDescriptor)
            {
                products = GetDataByKeyWord((SearchCommandDescriptor)descriptor);
                bagModel.TypeRequest = RequestType.Search;
            }

            if (descriptor is BrandCommandDescriptor)
            {
                products = FilterByBrand(products, (BrandCommandDescriptor)descriptor);
                bagModel.TypeRequest = RequestType.Category;
            }
			
            IList<ProductModel> productModels = products.ToList().ConvertAll(ConvertFrom);
            productModels = productModels.ApplyCampaigns();
            productModels = productModels.OrderByDescending(x => x.IsInCampaign).ThenBy(x => x.CampaignPrice).ToList();
            productModels = productModels.ApplyPaging(descriptor.Page, descriptor.PageSize);


            bagModel.Id = descriptor.EnityId;
            bagModel.Products = productModels;
            
            if (descriptor.EnityId != Guid.Empty)
                bagModel.Name = Repository.Data.Get<Category>(descriptor.EnityId).Name;

            return bagModel;
        }

        private IQueryable<Litium.Domain.Entities.ProductCatalog.Product> FilterByBrand(IQueryable<Litium.Domain.Entities.ProductCatalog.Product> products, BrandCommandDescriptor descriptor)
        {
            return products.Where(x => x.ProductProperty != null && !String.IsNullOrWhiteSpace(x.ProductProperty.Brend) &&
                                       x.ProductProperty.Brend.Equals(descriptor.Brand, StringComparison.InvariantCultureIgnoreCase) && x.Published);
        }

        private IQueryable<Litium.Domain.Entities.ProductCatalog.Product> GetDataByKeyWord(SearchCommandDescriptor descriptor)
        {
            return Repository.Data.Get<Litium.Domain.Entities.ProductCatalog.Product>().Where(x => x.Name.Contains(descriptor.KeyWord) && x.Published).All().AsQueryable();
        }

        private IQueryable<Litium.Domain.Entities.ProductCatalog.Product> GetDataByCategory(ProductCommandDescriptor descriptor)
        {
            var childCategories = new List<Category>();
            var category = Repository.Data.Get<Category>(descriptor.EnityId);
            childCategories = GetAllCategoryRecursively(category);

            if(childCategories == null)
                return new List<Litium.Domain.Entities.ProductCatalog.Product>().AsQueryable();
            
            var joinedCollection = from child in childCategories join product in Repository.Data.Get<Litium.Domain.Entities.ProductCatalog.Product>().Where(x => x.Category != null && x.Published).All() on
                                                                     child.Id equals product.Category.Id 
                                   select product;

            return joinedCollection.AsQueryable();
        }

        private List<Category> GetAllCategoryRecursively(Category category)
        {
            var combinedCategories =  new List<Category>{category};

            try
            {
                var childVategories = Repository.Data.Get<Category>().Where(x => x.Parent != null && x.Parent.Id == category.Id).All();

                if (!childVategories.Any())
                    return new List<Category> { category };

                foreach (var child in childVategories)
                {
                    combinedCategories.AddRange(GetAllCategoryRecursively(child));
                }
            }
            catch(Exception ex)
            {
                ILog log = LogManager.GetLogger(typeof(ActionHelper));
                log.Error(ex);
            }

            return combinedCategories;
        }

        private IQueryable<Litium.Domain.Entities.ProductCatalog.Product> GetDataByProductSet(ProductCommandDescriptor descriptor)
        {
            throw new NotImplementedException();
        }


        // Public 

        public int GetProductsCount(GridCommand command)
        {
            return _lastRequestCount;
        }


        public IEnumerable<SelectListItem> GetBrandsSelectedList(Guid categoryId)
        {
            IEnumerable<Litium.Domain.Entities.ProductCatalog.Product> products = GetDataByCategory(new ProductCommandDescriptor { EnityId = categoryId });

            var brendNames = (products.Where(prd => prd.ProductProperty != null
                && !String.IsNullOrWhiteSpace(prd.ProductProperty.Brend))
                .Select(prd => new
                {
                    Id = prd.ProductProperty.Brend.Trim(),
                    Brand = prd.ProductProperty.Brend.Trim()
                })).Distinct().OrderBy(arg => arg.Brand);

            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem());

            listItems.AddRange(brendNames.Select(brand => new SelectListItem
            {
                Value = brand.Id,
                Text = brand.Brand
            }));

            return listItems;
        }

        /// <summary>
        /// Uses for populating data on BackOffice  UI.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IEnumerable<ProductModel> GetProducts( GridCommand command )
        {
            List<Litium.Domain.Entities.ProductCatalog.Product> products = GetData (command);
            return products.ConvertAll (ConvertFrom);
        }

        /// <summary>
        /// Uses for populating data on Public site
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public ProductBagModel GetProducts(ProductCommandDescriptor descriptor)
        {
            return GetData(descriptor);
        }
    }
}