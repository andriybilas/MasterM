using System;
using System.ComponentModel.DataAnnotations;
using Litium.Resources;

namespace Site.Infrastuctures.ModelHelpers.Campaign
{
    public class PercentByProductSetCampaignModel : CampaignModel
    {
        public Guid ProductSetId { get; set; }

        [ResourceDisplayName(ResourceKey.ProductSetName)]
        public String Name { get; set; }

        [ResourceDisplayName(ResourceKey.PercentDiscount)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public int Percent { get; set; }
    }
}
