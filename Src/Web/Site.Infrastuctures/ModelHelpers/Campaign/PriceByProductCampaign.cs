using System;
using System.Collections.Generic;
using System.Linq;
using Litium.Common;
using Newtonsoft.Json;
using Site.Infrastuctures.ModelHelpers.Product;
using EntityProduct = Litium.Domain.Entities.ProductCatalog.Product;

namespace Site.Infrastuctures.ModelHelpers.Campaign
{
    public class PriceByProductCampaign : ConcreteCampaignBase
	{
        public override void ApplyCampaign(IList<ProductModel> products)
        {
            foreach (var product in products.Where(x => x.Published))
            {
                var model = (IEnumerable<PriceByProductCampaignItemModel>)Model;
                var campaignModel = new PriceByProductCampaignModel(model);

                if (campaignModel.ContainsKey(product.Id))
                {
                    product.IsInCampaign = true;
                    product.CampaignPrice = campaignModel[product.Id].CampaignPrice;
                }
            }
            
        }

        public override IEnumerable<ProductModel> GetCampaignProducts()
        {
			if (Model != null)
			{
				var model = (IEnumerable<PriceByProductCampaignItemModel>)Model;
				var campaignModel = new PriceByProductCampaignModel(model);
				IList<EntityProduct> products = campaignModel.Select(item => Repository.Data.Get<EntityProduct>().Where(x => x.Id == item.Id && x.Published).FirstOrDefault().Value).ToList();
				IList<ProductModel> productModels = products.ConvertAll().ToList();
				ApplyCampaign(productModels);
				return productModels;
			}
			return new List<ProductModel>();
        }
	}
}