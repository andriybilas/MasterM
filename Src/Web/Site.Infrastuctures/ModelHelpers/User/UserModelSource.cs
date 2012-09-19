using System;
using System.Collections.Generic;
using System.Linq;
using Litium.Common;
using Litium.Domain.Entities.Customers;
using Telerik.Web.Mvc;

namespace Site.Infrastuctures.ModelHelpers.User
{
    public class UserModelSource
    {
        private static UserModelSource _instance;

        public static UserModelSource Source
        {
            get
            {
                if(_instance == null)
                    _instance = new UserModelSource();
                return _instance;
            }
        }

        public int Count { get; private set; }

        private IEnumerable<UserModel> GetData(GridCommand command)
        {
            IQueryable<UserModel> persons = Repository.Data.Get<Person>().All().ToList().ConvertAll(ConvertFrom).AsQueryable();
            persons = persons.ApplyFiltering(command.FilterDescriptors);
            persons = persons.ApplySorting(command.GroupDescriptors, command.SortDescriptors);
            Count = persons.Count();
            persons = persons.ApplyPaging(command.Page, command.PageSize);
            return persons.ToList();
        }

        private UserModel ConvertFrom(Person person)
        {
			return new UserModel(person);
        }

        public IEnumerable<UserModel> Get (GridCommand command)
        {
            //List<Person> products = GetData(command);
            return GetData(command); 
        }
    }
}
