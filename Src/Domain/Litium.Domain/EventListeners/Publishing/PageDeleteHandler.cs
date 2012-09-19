using System;
using Litium.Common;
using Litium.Common.Events;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.EventListeners.Publishing
{
	internal sealed class PageDeleteHandler : IEventListener<Page, DeleteEvent>
	{
		public void HandleEvent(EntityEventArgs<Page> eventArgs)
		{
			DeleteChildrenAndRelations(eventArgs.Entity);
		}

		private void DeleteChildrenAndRelations(Page page)
		{
			if (page == null)
				throw new ArgumentNullException("page");

			var webSite = Repository.Data.Get<WebSite>(page.WebSiteId);
			if (webSite != null && webSite.StartPage != null && webSite.StartPage.Id == page.Id)
			{
				webSite.StartPage = null;
				Repository.Data.Save(webSite);
			}

			Repository.Data.Delete<WorkingCopy>(x => x.Page.Id == page.Id);
			Repository.Data.Delete<Page>(x => x.Parent != null && x.Parent.Id == page.Id);
		}
	}
}
