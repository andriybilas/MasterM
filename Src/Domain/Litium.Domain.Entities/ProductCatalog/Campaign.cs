using System;
using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Common.Validation.CustomAttribute;
using Litium.Resources;

namespace Litium.Domain.Entities.ProductCatalog
{
    [MetadataType(typeof(CampaignMetadata))]
	public class Campaign : DynamicEntity, IImage
	{
	    public virtual String Name { get; set; }

		public virtual String Description { get; set; }

		public virtual bool Active { get; set; }

		public virtual DateTime StartDate { get; set; }

		public virtual DateTime EndDate { get; set; }

        public virtual bool HasImage { get; set; }

    	public override object ValidationCopy()
    	{
    		return Clone();
    	}
	}

    public class CampaignMetadata
    {
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.CampaignName)]
        public virtual String Name { get; set; }

        [ResourceDisplayName(ResourceKey.Description)]
        public virtual String Description { get; set; }

        [ResourceDisplayName(ResourceKey.CampaignActive)]
        public virtual bool Active { get; set; }

        [SQLDateValid(ErrorMessageResourceName = ResourceKey.SqlDateValide, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.CampaignStartDate)]
        public virtual DateTime StartDate { get; set; }

        [SQLDateValid(ErrorMessageResourceName = ResourceKey.SqlDateValide, ErrorMessageResourceType = typeof(DomainNotification))]
        [ResourceDisplayName(ResourceKey.CampaignEndDate)]
        public virtual DateTime EndDate { get; set; }
   }
}