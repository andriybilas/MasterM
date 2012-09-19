using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Site.Infrastuctures.ModelHelpers.Product;
using EntityProduct = Litium.Domain.Entities.ProductCatalog.Product;

namespace Site.Infrastuctures.ModelHelpers.Campaign
{
	public abstract class ConcreteCampaignBase
	{
		[JsonProperty]
	    public dynamic Model { get; set; }
	    public abstract void ApplyCampaign(IList<ProductModel> products);
        public abstract IEnumerable<ProductModel> GetCampaignProducts();
	}
}