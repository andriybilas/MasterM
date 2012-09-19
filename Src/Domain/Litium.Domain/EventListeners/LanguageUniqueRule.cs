using System;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Validation;
using Litium.Domain.Entities;

namespace Litium.Domain.EventListeners
{
	internal sealed class LanguageUniqueRule : IEventListener<Language, ValidateEvent>
	{
		public void HandleEvent(EntityEventArgs<Language> eventArgs)
		{
			CheckForSameLanguage(eventArgs.Entity);
		}

		private void CheckForSameLanguage(Language language)
		{
			if (language == null)
				throw new ArgumentNullException("language");

			var sameLanguage = Repository.Data.Get<Language>()
				.Where(x => x.Id != language.Id && x.Culture == language.Culture)
				.FirstOrDefault()
				.Value;

			if (sameLanguage != null)
				throw new ValidationArgumentException(language, "LanguageExistException");
		}
	}
}