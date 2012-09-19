using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Litium.Common;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.File;
using LProduct = Litium.Domain.Entities.ProductCatalog.Product;
using LFile = Litium.Domain.Entities.Media.File;

namespace Site.Infrastuctures.ModelHelpers.Product
{
    [MetadataType(typeof(ProductModelMetadata))]
    public class ProductModel
    {
        private readonly LProduct _product;
        private Guid _productId;

        public ProductModel()
        {
            _product = new LProduct();
        }

        public ProductModel(LProduct product)
        {
            if (product != null)
            {
                _product = product;
                _productId = product.Id;
            }
            else
                _product = new LProduct();
        }

        public Guid Id { get { return _productId; } set { _productId = value ; } }

        public String Name { get { return _product.Name; } set { _product.Name = value; } }

        public String Description { get { return _product.Description; } set { _product.Description = value; } }

        public Decimal Price { get { return _product.Price; } set { _product.Price = value; } }

		public Decimal CampaignPrice { get; set; }

		public bool IsInCampaign { get; set; }

        public int StockBalance { get { return _product.StockBalance; } set { _product.StockBalance = value; } }
        
        public String Category
        {
            get
            {
                if (_product.Category == null)
                   return StoreResourceStrings.Uncategorized;
                return _product.Category.Name;
            }
        }

    	public Guid CatgeoryId
    	{
			get
			{
				if (_product.Category == null)
					return Guid.Empty;
				return _product.Category.Id;
			}
    	}

        public bool Published { get { return _product.Published; } set { _product.Published = value; } }

        public ProductProperty ProductProperty 
        { 
            get {
                if(_product.ProductProperty == null)
                    return new ProductProperty();
                return _product.ProductProperty;
            } 
            set { _product.ProductProperty = value; } }

        public bool HasImage  {  get { return _product.HasImage; }  set { _product.HasImage = value; } }

        public DateTime? CreateDate { get { return _product.CreateDate; } set { _product.CreateDate = value; } }

        public DateTime? UpdateDate { get { return _product.UpdateDate; } set { _product.UpdateDate = value; } }

        public string ImageUrl
        {
            get { return ImageUploadHelper.Helper.GetImageUrl(_productId, EntityType.Product); }
        }

        public LProduct GetProduct ()
        {
            return _product;
        }
    }
}