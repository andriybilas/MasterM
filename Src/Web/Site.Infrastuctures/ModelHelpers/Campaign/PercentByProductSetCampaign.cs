using System;
using System.Collections.Generic;
using System.Linq;
using Litium.Common;
using Litium.Domain.Entities.ProductCatalog;
using Site.Infrastuctures.ModelHelpers.Product;
using EntityProduct = Litium.Domain.Entities.ProductCatalog.Product;


namespace Site.Infrastuctures.ModelHelpers.Campaign
{
	[Serializable]
    public class PercentByProductSetCampaign : ConcreteCampaignBase
	{
	    public override void ApplyCampaign(IList<ProductModel> products)
	    {
            var productSet = Repository.Data.Get<ProductSet>(((PercentByProductSetCampaignModel)Model).ProductSetId);

            foreach (var product in products.Where(x => x.Published))
	        {
                if (productSet.Products.Any(x => x.Id == product.Id))
                {
                    product.IsInCampaign = true;
                    product.CampaignPrice = GetCampaignPrice(productSet.Products.FirstOrDefault(x => x.Id == product.Id).Price);
                }
	        }
	    }

	    public override IEnumerable<ProductModel> GetCampaignProducts()
	    {
			if (Model != null)
			{
				var productSet = Repository.Data.Get<ProductSet>(((PercentByProductSetCampaignModel)Model).ProductSetId);
				IList<ProductModel> productModels = productSet.Products.Where(x => x.Published).ConvertAll().ToList();
				ApplyCampaign(productModels);
				return productModels;	
			}
            return new List<ProductModel>();
	    }

	    private decimal GetCampaignPrice(decimal price)
	    {
            return price - ( price * ((PercentByProductSetCampaignModel)Model).Percent  / 100 );
	    }
	}
}