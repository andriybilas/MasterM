using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Litium.Resources;
using Newtonsoft.Json;

namespace Site.Infrastuctures.ModelHelpers.Campaign
{
    public class PriceByProductCampaignItemModel 
	{
		public Guid Id { get; set; }

        [ResourceDisplayName(ResourceKey.ProductName)]
		public string Name { get; set; }

        [ResourceDisplayName(ResourceKey.Price)]
        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }

        [ResourceDisplayName(ResourceKey.CampaignPrice)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public Decimal CampaignPrice { get; set; }
    }
}


