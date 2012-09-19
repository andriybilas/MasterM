using System.Collections.Generic;
using Litium.Common;
using Litium.Common.Events;
using Litium.Domain.Entities.ECommerce;

namespace Litium.Domain.EventListeners.Person
{
    internal sealed class PersonDeleteHandler : IEventListener<Entities.Customers.Person, DeleteEvent>
    {
        public void HandleEvent(EntityEventArgs<Entities.Customers.Person> eventArgs)
        {
            DeleteAllOrders(eventArgs.Entity);
        }

        private void DeleteAllOrders(Entities.Customers.Person entity)
        {
            IEnumerable<Order> orders = Repository.Data.Get<Order>().Where(x => x.Customer.Id == entity.Id).All();
            Repository.Data.Delete(orders);
        }
    }
}
