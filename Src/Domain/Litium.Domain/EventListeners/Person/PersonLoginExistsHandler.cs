using System.Linq;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Validation;

namespace Litium.Domain.EventListeners.Person
{
	internal sealed class PersonLoginExistsHandler : IEventListener<Litium.Domain.Entities.Customers.Person, InsertEvent>
	{
		public void HandleEvent(EntityEventArgs<Entities.Customers.Person> eventArgs)
		{
			if(Repository.Data.Get<Entities.Customers.Person>().Where(x => x.LoginName.Equals(eventArgs.Entity.LoginName)).All().Any())
				throw new ValidationArgumentException(eventArgs.Entity, "PersonLoginExistException");

			if (eventArgs.Entity.Profile != null && !string.IsNullOrWhiteSpace(eventArgs.Entity.Profile.Email))
				if(Repository.Data.Get<Entities.Customers.Person>().Where(x => x.Profile.Email.Equals(eventArgs.Entity.Profile.Email)).All().Any())
					throw new ValidationArgumentException(eventArgs.Entity, "PersonEmailExistException");

		}
	}
}
