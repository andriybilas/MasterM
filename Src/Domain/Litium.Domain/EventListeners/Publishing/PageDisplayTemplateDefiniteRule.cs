using System;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Validation;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.EventListeners.Publishing
{
	internal sealed class PageDisplayTemplateDefiniteRule : IEventListener<PageDisplayTemplate, ValidateEvent>
	{
		public void HandleEvent(EntityEventArgs<PageDisplayTemplate> eventArgs)
		{
			CheckForSameTemplate(eventArgs.Entity);
		}

		private void CheckForSameTemplate(PageDisplayTemplate template)
		{
			if (template == null)
				throw new ArgumentNullException("template");

			var sameTemplate = Repository.Data.Get<PageDisplayTemplate>()
				.Where(x =>
				       x.Id != template.Id &&
				       x.PageType.Id == template.PageType.Id &&
				       x.Name == template.Name)
				.FirstOrDefault()
				.Value;

			if (sameTemplate != null)
			{
				throw new ValidationArgumentException(template, "PageDisplayTemplateNameExistsException");
			}
		}
	}
}