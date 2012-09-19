using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Validation;
using Litium.Resources;
using LPerson = Litium.Domain.Entities.Customers.Person;

namespace Litium.Domain.EventListeners.Customers
{
	public class PersonRegisterHandler : IEventListener<LPerson, InsertEvent>
	{
		public void HandleEvent(EntityEventArgs<LPerson> eventArgs)
		{
			ValidateIfLoginNameExist(eventArgs.Entity);
		}

		private void ValidateIfLoginNameExist(LPerson entity)
		{
			LPerson person = Repository.Data.Get<LPerson>().Where(
				x => x.LoginName.Equals(entity.LoginName))
				.FirstOrDefault().Value;

			if(person != null)
				throw new ValidationArgumentException (person, ResourceKey.PersonLoginExistException);
		}
	}
}
