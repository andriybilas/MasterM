using System;
using System.Collections.Generic;

namespace Site.Infrastuctures.ModelHelpers.Product
{
    public class ProductBagModel
    {
        public ProductBagModel()
        {
            Products = new List<ProductModel>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasImage { get; set; }
        public IEnumerable<ProductModel> Products { get; set; }
        public RequestType TypeRequest { get; set; }
    }
}
