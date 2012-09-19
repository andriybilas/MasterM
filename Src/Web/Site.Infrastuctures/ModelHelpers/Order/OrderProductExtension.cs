using Litium.Common;
using Litium.Domain.Entities.ECommerce;

namespace Site.Infrastuctures.ModelHelpers.Order
{
    public static class OrderProductExtension
    {
        public static OrderProduct CopyFromModel (this OrderProduct orderRow, CartModel model )
        {
            orderRow.CampaignPrice = model.CampaignPrice;
            orderRow.Count = model.Count;
            var product = Repository.Data.Get<Litium.Domain.Entities.ProductCatalog.Product>(model.ProductId);
            orderRow.Product = product;
            orderRow.ProductName = product.Name;
            orderRow.Price = model.Price;
            orderRow.Summa = model.Summa;
            return orderRow;
        }
    }
}
