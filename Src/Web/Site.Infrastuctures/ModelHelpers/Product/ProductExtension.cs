using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Litium.Common.Configurations;
using Litium.Domain.Entities.ProductCatalog;
using Site.Infrastuctures.ModelHelpers.File;
using Image = Litium.Domain.Entities.Media.File;
using LProduct = Litium.Domain.Entities.ProductCatalog.Product;

namespace Site.Infrastuctures.ModelHelpers.Product
{
    public static class ProductExtension
    {
        public static LProduct CopyFrom(this LProduct source, ProductModel product, bool update)
        {
            source.Name = product.Name;
            source.Price = product.Price;

            if (update)
                source.ProductProperty.CopyFrom(product.ProductProperty);

            source.StockBalance = product.StockBalance;
            source.UpdateDate = DateTime.Now;
            source.CreateDate = source.CreateDate;
            source.Category = source.Category;
            source.Description = source.Description;
            return source;
        }

        public static IEnumerable<ProductModel> ConvertAll(this IEnumerable<LProduct> products)
        {
            return products.Select(product => new ProductModel(product)).ToList();
        }

        public static ProductProperty CopyFrom(this ProductProperty dest, ProductProperty source)
        {
            dest.Brend = source.Brend;
            dest.Capacity = source.Capacity;
            dest.Country = source.Country;
            dest.Weight = source.Weight;
            return dest;
        }

        public static IList<ImageModel> Copy(this IList<ImageModel> models, IEnumerable<Image> images)
        {
            foreach (var image in images)
            {
                var model = new ImageModel
                {
                    ContentType = image.ContentType, 
                    DisplayName = image.DisplayName, 
                    Name = image.Name,
                    ResizedTo = image.ResizedTo,
                    Size = image.Size,
                    ImagePath = String.Format("/{0}/{1}", LitiumConfigs.Data.FilesStorage.Replace("\\", "/"), image.Name)
                };
                models.Add(model);
            }

            return models;
        }
    }
}