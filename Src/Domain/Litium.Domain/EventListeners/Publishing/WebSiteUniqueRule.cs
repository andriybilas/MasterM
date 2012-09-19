using System;
using System.Collections.Generic;
using Litium.Common;
using Litium.Common.Events;
using Litium.Common.Validation;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.EventListeners.Publishing
{
	internal sealed class WebSiteUniqueRule : IEventListener<WebSite, ValidateEvent>
	{
		public void HandleEvent(EntityEventArgs<WebSite> eventArgs)
		{
			CheckForSameWebSite(eventArgs.Entity);
		}

		private void CheckForSameWebSite(WebSite site)
		{
			if (site == null)
				throw new ArgumentNullException("site");

			var sameNameSite = Repository.Data.Get<WebSite>()
				.Where(x => x.Id != site.Id && x.Name == site.Name)
				.FirstOrDefault()
				.Value;

			var sameDomainSite = Repository.Data.Get<WebSite>()
				.Where(x => x.Id != site.Id && x.DomainName == site.DomainName)
				.FirstOrDefault()
				.Value;

			var errorMessages = new List<string>();
			if (sameNameSite != null)
			{
				errorMessages.Add("WebSiteNameExistsException");
			}
			if (sameDomainSite != null)
			{
				errorMessages.Add("WebSiteDomainExistsException");
			}
			if (errorMessages.Count > 0)
			{
				throw new ValidationArgumentException(site, errorMessages.ToArray());
			}
		}
	}
}
