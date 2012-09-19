using System;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Validation;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.EventListeners.Publishing
{
	internal sealed class WebSiteDeleteHandler : IEventListener<WebSite, DeleteEvent>
	{
		public void HandleEvent(EntityEventArgs<WebSite> eventArgs)
		{
			CheckForDefault(eventArgs.Entity);

			DeletePages(eventArgs.Entity);
		}

		private void CheckForDefault(WebSite site)
		{
			if (site == null)
				throw new ArgumentNullException("site");

			if (site.IsDefault)
				throw new ValidationArgumentException(site, "DeleteDefaultWebSiteException");
		}

		private void DeletePages(WebSite site)
		{
			if (site == null)
				throw new ArgumentNullException("site");

			//Delete top folder and rest pages will be deleted by the page delete handler
			Repository.Data.Delete<Page>(x => x.WebSiteId == site.Id && x.Parent == null);
		}
	}
}