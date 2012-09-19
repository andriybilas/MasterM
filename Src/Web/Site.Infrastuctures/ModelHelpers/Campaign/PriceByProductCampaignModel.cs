using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Site.Infrastuctures.ModelHelpers.Campaign
{
    public class PriceByProductCampaignModel : CampaignModel, IQueryable<PriceByProductCampaignItemModel>
    {
        public PriceByProductCampaignModel()
        {
        }

        readonly List<PriceByProductCampaignItemModel> _campaignModels = new List<PriceByProductCampaignItemModel>();

        public PriceByProductCampaignModel(IEnumerable<PriceByProductCampaignItemModel> model)
        {
            _campaignModels.AddRange(model);
        }

        public void Add(PriceByProductCampaignItemModel item)
        {
            if(_campaignModels.All(x => x.Id != item.Id))
                _campaignModels.Add(item);
        }

        public void AddRange(IEnumerable<PriceByProductCampaignItemModel> items)
        {
            foreach (var model in items)
            {
                _campaignModels.Add(model);    
            }
        }

        public IEnumerable<PriceByProductCampaignItemModel> Get()
        {
            return _campaignModels;
        }

        public IEnumerator<PriceByProductCampaignItemModel> GetEnumerator()
        {
            return _campaignModels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(Guid productId)
        {
            return _campaignModels.Any(x => x.Id == productId);
        }

        public Expression Expression
        {
            get { return _campaignModels.AsQueryable().Expression; }
        }

        public Type ElementType
        {
            get { return typeof (PriceByProductCampaignItemModel); }
        }

        public IQueryProvider Provider
        {
            get { return _campaignModels.AsQueryable().Provider; }
        }

        public PriceByProductCampaignItemModel this[Guid productId]
        {
            get { return _campaignModels.FirstOrDefault(x => x.Id == productId); }
        }

        public void Remove(PriceByProductCampaignItemModel item)
        {
            var deletable = _campaignModels.FirstOrDefault(x => x.Id == item.Id);
            _campaignModels.Remove(deletable);           
        }
    }
}