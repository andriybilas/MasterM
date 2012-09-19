using System;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Validation;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.EventListeners.Publishing
{
	internal sealed class PageTypeUniqueRule : IEventListener<PageType, ValidateEvent>
	{
		public void HandleEvent(EntityEventArgs<PageType> eventArgs)
		{
			CheckForSamePageType(eventArgs.Entity);
		}

		private void CheckForSamePageType(PageType pageType)
		{
			if (pageType == null)
				throw new ArgumentNullException("pageType");

			var samePageType = Repository.Data.Get<PageType>()
				.Where(x => x.Id != pageType.Id && x.Name == pageType.Name)
				.FirstOrDefault()
				.Value;

			if (samePageType != null)
			{
				throw new ValidationArgumentException(pageType, "PageTypeExistsException");
			}
		}
	}
}
