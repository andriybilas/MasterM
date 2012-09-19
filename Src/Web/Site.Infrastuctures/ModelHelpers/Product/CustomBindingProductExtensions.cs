using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Litium.Common;
using Site.Infrastuctures.ModelHelpers.Campaign;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Infrastructure;
using EntityProduct = Litium.Domain.Entities.ProductCatalog.Product;
using CampaignEntity = Litium.Domain.Entities.ProductCatalog.Campaign;

namespace Site.Infrastuctures.ModelHelpers.Product
{
    public static class CustomBindingProductExtensions
    {
		public static IQueryable<EntityProduct> ApplyFiltering( this IQueryable<EntityProduct> data, IList<IFilterDescriptor> filterDescriptors )
        {
            if (filterDescriptors.Any())
            {
				data = data.Where (ExpressionBuilder.Expression<EntityProduct> (filterDescriptors));
            }
            return data;
        }

		public static IEnumerable ApplyGrouping( this IQueryable<EntityProduct> data, IList<GroupDescriptor> groupDescriptors )
        {
			Func<IEnumerable<EntityProduct>, IEnumerable<AggregateFunctionsGroup>> selector = null;
            foreach (var group in groupDescriptors.Reverse())
            {
                if (selector == null)
                {
                    if (group.Member == "Name")
                    {
                        selector = products => BuildInnerGroup(products, o => o.Name);
                    }
                    else if (group.Member == "Category.Name")
                    {
                        selector = products => BuildInnerGroup(products, o => o.Category.Name);
                    }
                }
                else
                {
                    if (group.Member == "Name")
                    {
                        selector = BuildGroup(o => o.Name, selector);
                    }
                    else if (group.Member == "Category.Name")
                    {
                        selector = BuildGroup(o => o.Category.Name, selector);
                    }
                }
            }
            if (selector != null) return selector.Invoke(data).ToList();
            return null;
        }

		private static Func<IEnumerable<EntityProduct>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>( Func<EntityProduct, T> groupSelector, Func<IEnumerable<EntityProduct>,
            IEnumerable<AggregateFunctionsGroup>> selectorBuilder)
        {
            var tempSelector = selectorBuilder;
            return g => g.GroupBy(groupSelector)
                         .Select(c => new AggregateFunctionsGroup
                         {
                             Key = c.Key,
                             HasSubgroups = true,
                             Items = tempSelector.Invoke(c).ToList()
                         });
        }

		private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>( IEnumerable<EntityProduct> group, Func<EntityProduct, T> groupSelector )
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Items = i.ToList()
                    });
        }

		public static IQueryable<EntityProduct> ApplyPaging( this IQueryable<EntityProduct> data, int currentPage, int pageSize )
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }
            data = data.Take(pageSize);
            return data;
        }
        
        public static IList<ProductModel> ApplyPaging( this IEnumerable<ProductModel> data, int currentPage, int pageSize )
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }
            data = data.Take(pageSize);
            return data.ToList();
        }

		public static IQueryable<EntityProduct> ApplySorting( this IQueryable<EntityProduct> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors )
        {
            if (groupDescriptors.Any())
            {
                foreach (var groupDescriptor in groupDescriptors.Reverse())
                {
                    data = AddSortExpression(data, groupDescriptor.SortDirection, groupDescriptor.Member);
                }
            }
            if (sortDescriptors.Any())
            {
                foreach (SortDescriptor sortDescriptor in sortDescriptors)
                {
                    data = AddSortExpression(data, sortDescriptor.SortDirection, sortDescriptor.Member);
                }
            }
            return data;
        }

		private static IQueryable<EntityProduct> AddSortExpression( IQueryable<EntityProduct> data, ListSortDirection sortDirection, string memberName )
        {
            if (sortDirection == ListSortDirection.Ascending)
            {
                switch (memberName)
                {
                    case "Name":
                        data = data.OrderBy(x => x.Name);
                        break;
                    case "Price":
                        data = data.OrderBy(x => x.Price);
                        break;
                    case "StockBalance":
                        data = data.OrderBy(x => x.StockBalance);
                        break;
                    case "Category":
                        data = data.Where(x => x.Category != null).OrderBy(x => x.Category.Name);
                        break;
                    case "Published":
                        data = data.OrderBy(x => x.Published);
                        break;
                    case "CreateDate":
                        data = data.OrderBy(x => x.CreateDate);
                        break;
                }
            }
            else
            {
                switch (memberName)
                {
                    case "Name":
                        data = data.OrderByDescending(x => x.Name);
                        break;
                    case "Price":
                        data = data.OrderByDescending(x => x.Price);
                        break;
                    case "StockBalance":
                        data = data.OrderByDescending(x => x.StockBalance);
                        break;
                    case "Category":
                        data = data.Where(x => x.Category != null).OrderByDescending(x => x.Category.Name);
                        break;
                    case "Published":
                        data = data.OrderByDescending(x => x.Published);
                        break;
                    case "CreateDate":
                        data = data.OrderByDescending(x => x.CreateDate);
                        break;
                }
            }
            return data;
        }

        public static IList<ProductModel> ApplyCampaigns(this IList<ProductModel> products)
		{
            var campaigns = Repository.Data.Get<CampaignEntity>().All().Where(x => x.Active && x.StartDate.Date <= DateTime.Now.Date && x.EndDate.Date >= DateTime.Now.Date);
            foreach (var campaign in campaigns)
            {
                ConcreteCampaignBase concreteCampaign = campaign.Metadata.Data;
                concreteCampaign.ApplyCampaign(products);
            }
			return products;
		}
    }
}
