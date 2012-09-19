using System;
using System.Collections.Generic;
using System.Linq;
using Litium.Common;
using Litium.Domain.Entities.Customers;
using Site.Infrastuctures.ModelHelpers.Order;
using Site.Infrastuctures.ModelHelpers.Product;
using LOrder = Litium.Domain.Entities.ECommerce.Order;

namespace Site.Infrastuctures.ModelHelpers.Campaign
{
	[Serializable]
    public class PersonalCampaign : ConcreteCampaignBase
    {
        public int GetPersonalDiscountPercent (OrderModel order, Person user)
        {
            var model = base.Model as PersonalCampaignModel;
            IEnumerable<LOrder> orders = Repository.Data.Get<LOrder>().Where(x => x.Customer.Id == user.Id).All();
            Decimal totalUserBuyimgAmount = orders.Where(x => x.CreateDate >= DateTime.Now.AddDays( - model.ExpirationPeriod) ).Sum(ord => ord.OrderSumma);
            return totalUserBuyimgAmount >= model.SummaryBought ? model.Percent : 0;
        }

	    public override void ApplyCampaign(IList<ProductModel> products)
	    {
	    }

	    public override IEnumerable<ProductModel> GetCampaignProducts()
	    {
	        return new List<ProductModel>();
	    }
    }
}
