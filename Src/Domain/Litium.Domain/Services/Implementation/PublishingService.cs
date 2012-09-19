using System;
using Litium.Common;
using Litium.Common.Validation;
using Litium.Common.WorkUnit;
using Litium.Domain.Entities.Publishing;

namespace Litium.Domain.Services.Implementation
{
	internal class PublishingService : IPublishingService
	{
		public Page CopyPaste(Guid pageToCopyId, Guid pageToPasteInId, bool recursive)
		{
			if (pageToCopyId == Guid.Empty)
				throw new ArgumentOutOfRangeException("pageToCopyId");

			if (pageToPasteInId == Guid.Empty)
				throw new ArgumentOutOfRangeException("pageToPasteInId");

			var pageToCopy = Repository.Data.Get<Page>(pageToCopyId);
			if (pageToCopy == null)
				throw new NullReferenceException(string.Format("Page with Id={0} doesn't exist", pageToCopyId));

			var pageToPasteIn = Repository.Data.Get<Page>(pageToPasteInId);
			if (pageToPasteIn == null)
				throw new NullReferenceException(string.Format("Page with Id={0} doesn't exist", pageToPasteInId));

			var copy = (Page) pageToCopy.Clone();
			copy.Parent = pageToPasteIn;
			copy.WebSiteId = pageToPasteIn.WebSiteId;

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(copy);
                if (recursive)
                {
                    CopyPasteRecursive(pageToCopy, copy);
                }
			    unit.Commit();
			}

			return copy;
		}

        public Page CopyPasteAsTop(Guid pageToCopyId, Guid webSiteToPasteInId, bool recursive)
		{
			if (pageToCopyId == Guid.Empty)
				throw new ArgumentOutOfRangeException("pageToCopyId");

			if (webSiteToPasteInId == Guid.Empty)
				throw new ArgumentOutOfRangeException("webSiteToPasteInId");

			var pageToCopy = Repository.Data.Get<Page>(pageToCopyId);
			if (pageToCopy == null)
				throw new NullReferenceException(string.Format("Page with Id={0} doesn't exist", pageToCopyId));

			var webSiteToPasteIn = Repository.Data.Get<WebSite>(webSiteToPasteInId);
			if (webSiteToPasteIn == null)
				throw new NullReferenceException(string.Format("WebSite with Id={0} doesn't exist", webSiteToPasteInId));

			var copy = (Page) pageToCopy.Clone();
			copy.Parent = null;
			copy.WebSiteId = webSiteToPasteInId;

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(copy);
                if (recursive)
                {
                    CopyPasteRecursive(pageToCopy, copy);
                }
			    unit.Commit();
			}

			return copy;
		}

		public Page CutAndPaste(Guid pageToCutId, Guid pageToPasteInId)
		{
			if (pageToCutId == Guid.Empty)
				throw new ArgumentOutOfRangeException("pageToCutId");

			if (pageToPasteInId == Guid.Empty)
				throw new ArgumentOutOfRangeException("pageToPasteInId");

			var pageToCut = Repository.Data.Get<Page>(pageToCutId);
			if (pageToCut == null)
				throw new NullReferenceException(string.Format("Page with Id={0} doesn't exist", pageToCutId));

			var pageToPasteIn = Repository.Data.Get<Page>(pageToPasteInId);
			if (pageToPasteIn == null)
				throw new NullReferenceException(string.Format("Page with Id={0} doesn't exist", pageToPasteInId));

			var parent = pageToPasteIn.Parent;
			while (parent != null)
			{
				if (parent.Id == pageToCutId)
					throw new ValidationArgumentException(pageToCut, "MoveToChildException");

			    parent = parent.Parent;
			}

			using (var unit = new UnitOfWork())
			{
				pageToCut.Parent = pageToPasteIn;
				var changeWebSite = pageToCut.WebSiteId != pageToPasteIn.WebSiteId;
				if (changeWebSite)
				{
					pageToCut.WebSiteId = pageToPasteIn.WebSiteId;
					Repository.Data.Save(pageToCut);
					ChangeWebSiteRecursive(pageToCut);
				}
				else
				{
					Repository.Data.Save(pageToCut);
				}

				unit.Commit();
			}

			return pageToCut;
		}

		public Page CutAndPasteAsTop(Guid pageToCutId, Guid webSiteToPasteInId)
		{
			if (pageToCutId == Guid.Empty)
				throw new ArgumentOutOfRangeException("pageToCutId");

			if (webSiteToPasteInId == Guid.Empty)
				throw new ArgumentOutOfRangeException("webSiteToPasteInId");

			var pageToCut = Repository.Data.Get<Page>(pageToCutId);
			if (pageToCut == null)
				throw new NullReferenceException(string.Format("Page with Id={0} doesn't exist", pageToCutId));

			var webSiteToPasteIn = Repository.Data.Get<WebSite>(webSiteToPasteInId);
			if (webSiteToPasteIn == null)
				throw new NullReferenceException(string.Format("WebSite with Id={0} doesn't exist", webSiteToPasteInId));

			using (var unit = new UnitOfWork())
			{
				pageToCut.Parent = null;
				var changeWebSite = pageToCut.WebSiteId != webSiteToPasteInId;
				if (changeWebSite)
				{
					pageToCut.WebSiteId = webSiteToPasteInId;
					Repository.Data.Save(pageToCut);
					ChangeWebSiteRecursive(pageToCut);
				}
				else
				{
					Repository.Data.Save(pageToCut);
				}

				unit.Commit();
			}

			return pageToCut;
		}

		private void CopyPasteRecursive(Page source, Page destination)
		{
			var pagesToCopy = Repository.Data.Get<Page>().Where(x => x.Parent.Id == source.Id).All();
			foreach (var pageToCopy in pagesToCopy)
			{
				var copy = (Page) pageToCopy.Clone();
				copy.Parent = destination;
				copy.WebSiteId = destination.WebSiteId;
				Repository.Data.Save(copy);
				CopyPasteRecursive(pageToCopy, copy);
			}
		}

		private void ChangeWebSiteRecursive(Page page)
		{
			var children = Repository.Data.Get<Page>().Where(x => x.Parent.Id == page.Id).All();
			foreach (var child in children)
			{
				child.WebSiteId = page.WebSiteId;
				Repository.Data.Save(child);
				ChangeWebSiteRecursive(child);
			}
		}
	}
}