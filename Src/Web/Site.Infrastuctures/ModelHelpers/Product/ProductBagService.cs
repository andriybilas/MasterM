using System;
using System.Collections.Generic;
using System.Linq;
using Litium.Common;
using Litium.Domain.Entities.ProductCatalog;
using Site.Infrastuctures.ModelHelpers.Campaign;
using LCampaign = Litium.Domain.Entities.ProductCatalog.Campaign;

namespace Site.Infrastuctures.ModelHelpers.Product
{
    public class ProductBagService
    {
        private int _index;
        private readonly List<ProductBagModel> _productBags = new List<ProductBagModel>();

        public ProductBagService()
        {
            LoadAllCampaignsProducts();
            LoadAllProductSetsProducts();
            //RemoveEmptyItems();
        }

        private void RemoveEmptyItems()
        {
            foreach (var model in _productBags)
            {
                if (!model.HasImage && !model.Products.Any())
                {
                    _productBags.Remove(model);
                }
            }
        }

        private void LoadAllProductSetsProducts()
        {
            IEnumerable<ProductSet> productSets = Repository.Data.Get<ProductSet>().Where(x => x.Campaign == null).All();
            foreach (var set in productSets)
            {
                _productBags.Add(new ProductBagModel {
                        Id = set.Id,
                        HasImage = set.HasImage,
                        Name = set.Name,
                        TypeRequest = RequestType.Campaign, 
                        Description = set.Description,
                        Products = set.Products.Where(x=>x.Published).ConvertAll() });
            }
        }

        private void LoadAllCampaignsProducts()
        {
            List<LCampaign> campaigns = Repository.Data.Get<LCampaign>().All()
                .Where(x => x.Active && x.StartDate.Date <= DateTime.Now.Date && x.EndDate.Date >= DateTime.Now.Date).ToList();

            var campaignsWithoutPersonal = new List<LCampaign>();

            foreach (var campaign in campaigns)
                if ((CampaignType)campaign.Metadata.CampaignType != CampaignType.PersonalCampaign)
                    campaignsWithoutPersonal.Add(campaign);

            foreach (var campaign in campaignsWithoutPersonal)
            {
                IEnumerable<ProductModel> products = ((ConcreteCampaignBase)campaign.Metadata.Data).GetCampaignProducts();
                
                _productBags.Add(new ProductBagModel {
                        Id = campaign.Id,
                        HasImage = campaign.HasImage,
                        Name = campaign.Name,
                        Description = campaign.Description,
                        TypeRequest = RequestType.Campaign, 
                        Products = products });
            }
        }

        public ProductBagModel GetNextProductsBag ()
        {
            if (_index >= _productBags.Count)
                _index = 0;

            if (_productBags.Count > 0)
            {
                var result = _productBags[_index];
                _index += 1;
                return result;
            }
            return null;
        }
    }
}
