using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Infrastuctures.ModelHelpers.Product
{
    public class CategoryProductModel
    {
        public CategoryProductModel()
        {
            Products = new List<ProductModel>();
        }

        public Guid CategoryId { get; set; }
        public String CategoryName { get; set; }
        public IEnumerable<ProductModel> Products { get; set; }
    }
}
