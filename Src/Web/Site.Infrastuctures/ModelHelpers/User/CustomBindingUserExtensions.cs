using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Litium.Domain.Entities.Customers;
using Telerik.Web.Mvc;

namespace Site.Infrastuctures.ModelHelpers.User
{
    public static class CustomBindingUserExtensions
    {
        public static IQueryable<UserModel> ApplyFiltering(this IQueryable<UserModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<UserModel>(filterDescriptors));
            }
            return data;
        }

        public static IQueryable<UserModel> ApplyPaging(this IQueryable<UserModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }
            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<UserModel> ApplySorting(this IQueryable<UserModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<UserModel> AddSortExpression(IQueryable<UserModel> data, ListSortDirection sortDirection, string memberName)
        {
            if (sortDirection == ListSortDirection.Ascending)
            {
                switch (memberName)
                {
                    case "DisplayName":
                        data = data.OrderBy(x => x.DisplayName);
                        break;
                    case "PhoneMobile":
                        data = data.OrderBy(x => x.PhoneMobile);
                        break;
                    case "LastLoginDate":
                        data = data.OrderBy(x => x.LastLoginDate);
                        break;
                    case "Active":
                        data = data.OrderBy(x => x.Active);
                        break;
                }
            }
            else
            {
                switch (memberName)
                {
                    case "DisplayName":
                        data = data.OrderByDescending(x => x.DisplayName);
                        break;
                    case "PhoneMobile":
                        data = data.OrderByDescending(x => x.PhoneMobile);
                        break;
                    case "LastLoginDate":
                        data = data.OrderByDescending(x => x.LastLoginDate);
                        break;
                    case "Active":
                        data = data.OrderByDescending(x => x.Active);
                        break;
                }
            }
            return data;
        }
    }
}
