using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Litium.Common;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.Campaign;
using Site.Infrastuctures.Security;
using Telerik.Web.Mvc;


//TODO: doesn't work validation on create campaign view

namespace Web.Site.Controllers
{
	[Security(Roles = UserRole.Administer)]
	public class CampaignController : Controller
	{
		public ActionResult Index()
		{
			return View ("CampaignView", GetCampaigns ());
		}

		private IEnumerable<Campaign> GetCampaigns()
		{
			return Repository.Data.Get<Campaign> ().All ();
		}

		[AcceptVerbs (HttpVerbs.Post)]
		public ActionResult GetCampaignsTypes()
		{
			IEnumerable<string> campaignTypes = Enum.GetNames (typeof (CampaignType)).Where (x => x != "Undefined");
            campaignTypes.ToList().Remove("Undefined");
			var campaignNames = campaignTypes.ToDictionary (type => type, StoreResourceStrings.Get);
			return new JsonResult { Data = new SelectList (campaignNames.ToList (), "Key", "Value") };
		}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetProductSets ()
        {
            IEnumerable<ProductSet> productSets = Repository.Data.Get<ProductSet>().All();
            return new JsonResult { Data = new SelectList(productSets, "Id", "Name") };
        }

		[AcceptVerbs (HttpVerbs.Post)]
        public ActionResult LoadCreateCampaignView ()
		{
		    CampaignModelHelper.ModelHelper.EditedCampaignId = Guid.Empty;
            return PartialView("CampaignPartial/CreateCampaign", new Campaign());
		}

		[AcceptVerbs (HttpVerbs.Post)]
        public ActionResult LoadCampaignTypeView (string campaignViewId)
		{
            switch (campaignViewId)
            {
                case "PriceByProductCampaign":
                    {
                        CampaignModelHelper.ModelHelper.EditedCampaignType = CampaignType.PriceByProductCampaign;
						CampaignModelHelper.ModelHelper.ConcreteCampaign = new PriceByProductCampaign();
                        return PartialView("CampaignPartial/PriceByProductCampaign");
                    }
                case "PercentByProductSetCampaign":
                    {
                        CampaignModelHelper.ModelHelper.EditedCampaignType = CampaignType.PercentByProductSetCampaign;
                    	CampaignModelHelper.ModelHelper.ConcreteCampaign = new PercentByProductSetCampaign();
                        return PartialView("CampaignPartial/PercentByProductSetCampaign");
                    }
                case "PersonalCampaign":
                    {
                        CampaignModelHelper.ModelHelper.EditedCampaignType = CampaignType.PersonalCampaign;
						CampaignModelHelper.ModelHelper.ConcreteCampaign = new PersonalCampaign();
                        return PartialView("CampaignPartial/PersonalCampaign");
                    }
            }
			return PartialView ("CampaignPartial/PriceByProductCampaign");
		}

       public ActionResult LoadCampaignForUpdate(CampaignType campaignType)
        {
            switch (campaignType)
            {
                case CampaignType.PriceByProductCampaign:
                    {
                        return PartialView("CampaignPartial/PriceByProductCampaign");
                    }
                case CampaignType.PercentByProductSetCampaign:
                    {
                        return PartialView("CampaignPartial/PercentByProductSetCampaign");
                    }
                case CampaignType.PersonalCampaign:
                    {
                        return PartialView("CampaignPartial/PersonalCampaign");
                    }
            }
            return PartialView("CampaignPartial/PriceByProductCampaign");
        }

        [HttpPost]
       public ActionResult SetCampaignDetailsDataAsync (string campaignId)
        {
            Guid campaignGuidId;
            Guid.TryParse(campaignId, out campaignGuidId);
            var campaign = Repository.Data.Get<Campaign>(campaignGuidId);
            CampaignModelHelper.ModelHelper.Clear();
            CampaignModelHelper.ModelHelper.EditedCampaignType = (CampaignType)campaign.Metadata.CampaignType;
            CampaignModelHelper.ModelHelper.EditedCampaignId = campaign.Id;
            CampaignModelHelper.ModelHelper.ConcreteCampaign = campaign.Metadata.Data;
            return PartialView("CampaignPartial/EditCampaign", campaign);
        }

        [HttpPost]
        public ActionResult ViewAllCampaignAsync ()
        {
            return PartialView("CampaignPartial/ViewCampaigns");
        }

        [GridAction]
        public ActionResult GetCampaignProductsAsync()
		{
            return View("CampaignPartial/PriceByProductCampaign", GetPriceCampaignGridModel());
		}

        [HttpPost]
        public ActionResult GetCampainTypeData ()
        {
            CampaignModel campaignDetails = CampaignModelHelper.ModelHelper.ConcreteCampaign.Model;
            return new JsonResult { Data = campaignDetails };
        }

        [GridAction]
        public ActionResult GetCampaignsAsync ()
        {
            IEnumerable<Campaign> campaigns = Repository.Data.Get<Campaign>().All();
            return PartialView("CampaignPartial/ViewCampaigns", new GridModel<Campaign> {Data = campaigns });
        }

	    private static Guid ParseId(string campaignId)
	    {
	        Guid campaignGuidId;
	        Guid.TryParse(campaignId, out campaignGuidId);
	        return campaignGuidId;
	    }

        private GridModel<PriceByProductCampaignItemModel> GetPriceCampaignGridModel()
        {
            var model = new PriceByProductCampaignModel();
			
			if (CampaignModelHelper.ModelHelper.ConcreteCampaign.Model != null)
				model.AddRange(CampaignModelHelper.ModelHelper.ConcreteCampaign.Model);

            CampaignModelHelper.ModelHelper.ConcreteCampaign.Model = model;
            return new GridModel<PriceByProductCampaignItemModel> { Data = CampaignModelHelper.ModelHelper.ConcreteCampaign.Model };
        }

        [GridAction]
        public ActionResult SavePriceByProductCampaign()
		{
            var campaignModel = new PriceByProductCampaignItemModel();
            
            if (TryUpdateModel(campaignModel))
            {
                var model = CampaignModelHelper.ModelHelper.ConcreteCampaign.Model as PriceByProductCampaignModel;
                model.Remove(campaignModel);
                model.Add(campaignModel);
                CampaignModelHelper.ModelHelper.ConcreteCampaign.Model = model;
            }

            return View("CampaignPartial/PriceByProductCampaign", GetPriceCampaignGridModel());
		}

        [GridAction]
        public ActionResult DeleteCampaignProductsAsync(Guid productId)
        {
            var model = (PriceByProductCampaignModel) CampaignModelHelper.ModelHelper.ConcreteCampaign.Model;
            PriceByProductCampaignItemModel item = model.FirstOrDefault(x => x.Id == productId);
            model.Remove(item);
            return View("CampaignPartial/PriceByProductCampaign", GetPriceCampaignGridModel());
        }

		[HttpPost]
        public ActionResult OpenProductCampaignWindow()
		{
			ViewBag.WindowTitle = StoreResourceStrings.SelectProductCampaign;
			ViewBag.ActionName = "GetViewForAssignCampaignProducts";
			ViewBag.ControllerName = "Campaign";
			ViewBag.Width = 500;
			return PartialView("Partial/Window" );
		}

		public ActionResult GetViewForAssignCampaignProducts ()
		{
			return PartialView("CampaignPartial/AssignProductsForCampaign");
		}

        [HttpPost]
        public void AssignProductsForCampaign(List<String> assignProducts)
        {
			List<Guid> productIds = assignProducts.Select(Guid.Parse).ToList();
            var productCampaigns = (PriceByProductCampaignModel)CampaignModelHelper.ModelHelper.ConcreteCampaign.Model ??
                new PriceByProductCampaignModel();
            
            foreach (Guid id in productIds)
            {
                var product = Repository.Data.Get<Product>(id);
                var productCampaign = new PriceByProductCampaignItemModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price
                };
                productCampaigns.Add(productCampaign);
            }

            CampaignModelHelper.ModelHelper.ConcreteCampaign.Model = productCampaigns;
        }

        public ActionResult SaveUpdateCampaign(Guid campaignId)
	    {
            var campaign = new Campaign();
            if (TryUpdateModel(campaign))
            {
                campaign.Metadata.Data = CampaignModelHelper.ModelHelper.ConcreteCampaign;
                campaign.Metadata.CampaignType = CampaignModelHelper.ModelHelper.EditedCampaignType;

                if (campaignId != Guid.Empty)
                {
                    var campaignEntity = Repository.Data.Get<Campaign>(campaignId);
                    campaignEntity.Name = campaign.Name;
                    campaignEntity.StartDate = campaign.StartDate;
                    campaignEntity.EndDate = campaign.EndDate;
                    campaignEntity.Active = campaign.Active;
                    campaignEntity.Metadata.Data = campaign.Metadata.Data;
                    campaignEntity.Metadata.CampaignType = campaign.Metadata.CampaignType;
                    Repository.Data.Save(campaignEntity);
                }
                else
                {
                    Repository.Data.Save(campaign);    
                }
            }

	        return Index();
	    }

        [HttpPost]
		public void DeleteCampaign (string campaignId)
        {
            Guid campaignGuidId = ParseId(campaignId);
            Campaign campaign = Repository.Data.Get<Campaign>(campaignGuidId);
            Repository.Data.Delete(campaign);
        }

       [HttpPost]
        public ActionResult SubmitProductSetCampaignChanges(PercentByProductSetCampaignModel campaignDetails)
       {
            CampaignModelHelper.ModelHelper.ConcreteCampaign.Model = campaignDetails;
           return Content(String.Empty);
        }       

        [HttpPost]
       public ActionResult SubmitPersonalCampaignChanges(PersonalCampaignModel campaignDetails)
       {
           CampaignModelHelper.ModelHelper.ConcreteCampaign.Model = campaignDetails;
           return Content(String.Empty);
        }
	}
}