using System.Collections.Generic;
using Litium.Common;
using Litium.Common.Events;
using Litium.Domain.Entities.ProductCatalog;
using CategoryEntity = Litium.Domain.Entities.ProductCatalog.Category;


namespace Litium.Domain.EventListeners.Category
{
	public class CategoryDeleteHandler : IEventListener<CategoryEntity, DeleteEvent>
	{
		public void HandleEvent( EntityEventArgs<CategoryEntity> eventArgs )
		{
			DeleteAllChild(eventArgs.Entity);
		}

		private void DeleteAllChild(CategoryEntity entity)
		{
            IEnumerable<Product> products = Repository.Data.Get<Product>().Where(x => x.Category == entity).All();

            foreach (var product in products)
            {
                product.Category = null;
                Repository.Data.Save(product);
            }

			IEnumerable<CategoryEntity> categories = Repository.Data.Get<CategoryEntity> ()
				.Where (x => x.Parent == entity).All ();

			foreach (var category in categories)
			{
				Repository.Data.Delete(category);
			}

		}
	}
}
