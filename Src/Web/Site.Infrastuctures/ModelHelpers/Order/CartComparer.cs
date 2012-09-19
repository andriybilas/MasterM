using System.Collections.Generic;

namespace Site.Infrastuctures.ModelHelpers.Order
{
    public class CartComparer : IEqualityComparer<CartModel>
    {
        public bool Equals(CartModel x, CartModel y)
        {
            if(x == null || y == null)
                return false;

            if (x.ProductId == y.ProductId)
                return true;
            return false;
        }

        public int GetHashCode(CartModel obj)
        {
            return obj.GetHashCode();
        }
    }
}