using System;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Validation;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.EventListeners.Publishing
{
	internal sealed class PageNameDefiniteRule : IEventListener<Page, ValidateEvent>
	{
		public void HandleEvent(EntityEventArgs<Page> eventArgs)
		{
			CheckForSamePage(eventArgs.Entity);
		}

		private void CheckForSamePage(Page page)
		{
			if (page == null)
				throw new ArgumentNullException("page");

			var samePage = Repository.Data.Get<Page>()
				.Where(x =>
				       x.Id != page.Id &&
				       x.Parent == page.Parent &&
				       x.Name == page.Name)
				.FirstOrDefault()
				.Value;

			if (samePage != null)
			{
				throw new ValidationArgumentException(page, "PageNameExistsException");
			}
		}
	}
}