using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Common.Events;

namespace Litium.Common.Validation
{
	internal sealed class ValidationEventListener : IEventListener<Entity, ValidateEvent>
	{
		public void HandleEvent(EntityEventArgs<Entity> eventArgs)
		{
			var cloneEntity = eventArgs.Entity.ValidationCopy();
			var results = new List<ValidationResult>();
			var context = new ValidationContext(cloneEntity, null, null);

			if (!Validator.TryValidateObject(cloneEntity, context, results, true))
			{
				throw new ValidationArgumentException(eventArgs.Entity, results);
			}
		}
	}
}