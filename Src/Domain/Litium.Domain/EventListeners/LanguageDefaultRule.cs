using System;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Validation;
using Litium.Common.WorkUnit;
using Litium.Domain.Entities;

namespace Litium.Domain.EventListeners
{
	internal sealed class LanguageDefaultRule : IEventListener<Language, InsertEvent>,
	                                            IEventListener<Language, UpdateEvent>,
	                                            IEventListener<Language, DeleteEvent>
	{
		void IEventListener<Language, InsertEvent>.HandleEvent(EntityEventArgs<Language> eventArgs)
		{
			Insert(eventArgs.Entity);
		}

		void IEventListener<Language, UpdateEvent>.HandleEvent(EntityEventArgs<Language> eventArgs)
		{
			Update(eventArgs.Entity);
		}

		void IEventListener<Language, DeleteEvent>.HandleEvent(EntityEventArgs<Language> eventArgs)
		{
			Delete(eventArgs.Entity);
		}

		private void Insert(Language language)
		{
			if (language == null)
				throw new ArgumentNullException("language");

			//If the language to insert is default then old default language if it exists should be changed to not default.
			if (language.IsDefault)
			{
				var oldDefaultLanguage = Repository.Data.Get<Language>()
					.Where(x => x.IsDefault && x.Id != language.Id)
					.FirstOrDefault()
					.Value;

				if (oldDefaultLanguage != null)
				{
					oldDefaultLanguage.IsDefault = false;
					Repository.Data.Save(oldDefaultLanguage);
				}
			}
				//If the language to insert isn't default but there are no other languages then it should be default
			else
			{
				var anyLanguage = Repository.Data.Get<Language>()
					.FirstOrDefault()
					.Value;

				if (anyLanguage == null)
				{
					language.IsDefault = true;
				}
			}
		}

		private void Update(Language language)
		{
			if (language == null)
				throw new ArgumentNullException("language");

			//If the language to update is default then old default language if it exists should be changed to not default.
			if (language.IsDefault)
			{
				var oldDefaultLanguage = Repository.Data.Get<Language>()
					.Where(x => x.IsDefault && x.Id != language.Id)
					.FirstOrDefault()
					.Value;
				if (oldDefaultLanguage != null)
				{
					oldDefaultLanguage.IsDefault = false;
					Repository.Data.Save(oldDefaultLanguage);
				}
			}
				//If the language to update isn't default then ensure that some default language will exist after
			else
			{
				//If this update event is caused by inserting event of another default language then the following action should be performed after insert complete
				UnitOfWork.Current.PreCommitActions.Add(() =>
				                                        	{
				                                        		var otherDefaultLanguage = Repository.Data.Get<Language>()
				                                        			.Where(x => x.IsDefault && x.Id != language.Id)
				                                        			.FirstOrDefault()
				                                        			.Value;
				                                        		//if there is no other default language
				                                        		if (otherDefaultLanguage == null)
				                                        		{
				                                        			var firstOtherLanguage = Repository.Data.Get<Language>()
				                                        				.Where(x => x.Id != language.Id)
				                                        				.FirstOrDefault()
				                                        				.Value;
				                                        			//set first other language as default if it exists
				                                        			if (firstOtherLanguage != null)
				                                        			{
				                                        				firstOtherLanguage.IsDefault = true;
				                                        				Repository.Data.Save(firstOtherLanguage);
				                                        			}
				                                        				//set current language to default if no other language exists
				                                        			else
				                                        			{
				                                        				language.IsDefault = true;
				                                        				Repository.Data.Save(language);
				                                        			}
				                                        		}
				                                        	});
			}
		}

		private void Delete(Language language)
		{
			if (language == null)
				throw new ArgumentNullException("language");

			//If the language to delete is default then ensure that some default language will exist after
			if (language.IsDefault)
			{
				var firstAnotherLanguage = Repository.Data.Get<Language>()
					.Where(x => x.Id != language.Id)
					.FirstOrDefault()
					.Value;
				//set first other language as default if it exists
				if (firstAnotherLanguage != null)
				{
					firstAnotherLanguage.IsDefault = true;
					Repository.Data.Save(firstAnotherLanguage);
				}
					//forbid deleting the last and default language
				else
				{
					throw new ValidationArgumentException(language, "DeleteDefaultLanguageException");
				}
			}
		}
	}
}