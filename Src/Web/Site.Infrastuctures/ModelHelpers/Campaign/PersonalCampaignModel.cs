using System.ComponentModel.DataAnnotations;
using Litium.Resources;

namespace Site.Infrastuctures.ModelHelpers.Campaign
{
    public class PersonalCampaignModel : CampaignModel
    {
        [ResourceDisplayName(ResourceKey.ExpirationPeriod)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public int ExpirationPeriod { get; set; }

        [ResourceDisplayName(ResourceKey.PercentDiscount)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public int Percent { get; set; }

        [ResourceDisplayName(ResourceKey.SummaryBought)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public decimal SummaryBought { get; set; }
    }
}
