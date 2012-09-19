using System.Collections.Generic;
using Litium.Common;
using Litium.Domain.Entities.ProductCatalog;
using LProduct = Litium.Domain.Entities.ProductCatalog.Product;


namespace Site.Infrastuctures.ModelHelpers.Synchronization
{
    public class SynchActionService
    {
        public void InsertProductToRepository(IEnumerable<LProduct> products)
        {
            foreach (var product in products)
            {
                UpdateCategory(product);
                Repository.Data.Save(product);
            }
        }

        private void UpdateCategory(LProduct product)
        {
            if(product.Category == null)
                return;

            Category category = Repository.Data.Get<Category>()
                .Where(x => x.Name.Equals(product.Category.Name)).FirstOrDefault().Value;

            if (category == null)
                Repository.Data.Save(product.Category);
            else
                product.Category = category;
        }
    }
}
