using System.Collections.Generic;
using System.Linq;
using Litium.Common.Events;
using Litium.Domain.Entities.ProductCatalog;

namespace Litium.Domain.EventListeners.ProductCatalog
{
    class AssignProductToProductSetEventListeber : IEventListener< Product, UpdateEvent>
    {
        public void HandleEvent(EntityEventArgs<Product> eventArgs)
        {
            Product product = eventArgs.Entity;
            SkipIfProductSetAllreadyExists(product);
        }

        private void SkipIfProductSetAllreadyExists(Product product)
        {
            foreach (var productSet in product.ProductSets)
            {
                if (product.ProductSets.Select(x => x.Id == productSet.Id).Count() > 1)
                {
                    product.ProductSets.Remove(productSet);
                    break;
                }
            }
        }
    }
}
