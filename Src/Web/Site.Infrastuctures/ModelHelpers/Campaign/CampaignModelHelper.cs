using System;

namespace Site.Infrastuctures.ModelHelpers.Campaign
{
    public class CampaignModelHelper
    {
        private static CampaignModelHelper _instance ;
        public ConcreteCampaignBase ConcreteCampaign { get; set; }
        
        public static CampaignModelHelper ModelHelper { get { return _instance ?? (_instance = new CampaignModelHelper()); } }

        public Guid EditedCampaignId { get; set; }
        public CampaignType EditedCampaignType { get; set; }

        public void Clear()
        {
            EditedCampaignId = Guid.Empty;
            EditedCampaignType = CampaignType.Undefined;
            ConcreteCampaign =  null;
        }
    }
}