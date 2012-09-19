using System;
using System.ComponentModel.DataAnnotations;
using Litium.Common.Validation.CustomAttribute;
using Litium.Resources;
using Litium.Domain.Entities.Customers;

namespace Litium.Domain.Entities.Publishing
{
	/// <summary>
	/// 	Page
	/// </summary>
	public class Page : PageBase
	{
		/// <summary>
		/// 	Gets or sets the end publish date time.
		/// </summary>
		/// <value> The end publish date time. </value>
		public virtual DateTime? EndPublishDateTime { get; set; }

		/// <summary>
		/// 	Gets or sets the index.
		/// </summary>
		/// <value> The index. </value>
		public virtual int Index { get; set; }

		/// <summary>
		/// 	Gets or sets a value indicating whether this instance is in sitemap.
		/// </summary>
		/// <value> <c>true</c> if this instance is in sitemap; otherwise, <c>false</c> . </value>
		public virtual bool IsInSitemap { get; set; }

		/// <summary>
		/// 	Gets or sets the menu status.
		/// </summary>
		/// <value> The menu status. </value>
		[EnumTypeCompatible(ErrorMessageResourceName = "EnumFormat", ErrorMessageResourceType = typeof (Notification))]
		public virtual MenuStatus MenuStatus { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		[EnumTypeCompatible(ErrorMessageResourceName = "EnumFormat", ErrorMessageResourceType = typeof (Notification))]
		public virtual PageStatus Status { get; protected internal set; }

		/// <summary>
		/// 	Gets or sets the type of the page.
		/// </summary>
		/// <value> The type of the page. </value>
		[Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof (Notification))]
		public virtual PageType PageType { get; set; }

		/// <summary>
		/// Gets or sets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public virtual Page Parent { get; set; }

		/// <summary>
		/// Gets or sets the page responsible.
		/// </summary>
		/// <value>The page responsible.</value>
		public virtual Person PageResponsible { get; set; }

		/// <summary>
		/// 	Gets or sets the start publish date time.
		/// </summary>
		/// <value> The start publish date time. </value>
		public virtual DateTime? StartPublishDateTime { get; protected internal set; }

		/// <summary>
		/// 	Gets or sets the name of the URL.
		/// </summary>
		/// <value> The name of the URL. </value>
		public virtual string UrlName { get; set; }

		/// <summary>
		/// 	Gets or sets the web site.
		/// </summary>
		/// <value> The web site. </value>
		[Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof (Notification))]
		public virtual Guid WebSiteId { get; set; }

		/// <summary>
		/// 	Publishe this page.
		/// </summary>
		public virtual void Publish()
		{
			StartPublishDateTime = DateTime.Now;
			Status = PageStatus.Published;
			//PublishedUser = CurrentUser...
		}

		/// <summary>
		/// 	Publishes the page at specified publish date.
		/// </summary>
		/// <param name="publishDate"> The publish date. </param>
		public virtual void Publish(DateTime publishDate)
		{
			StartPublishDateTime = publishDate;
			Status = PageStatus.NotPublishedDelayedPublish;
			//PublishedUser = CurrentUser...
		}

		/// <summary>
		/// 	Unpublish this page.
		/// </summary>
		public virtual void Unpublish()
		{
			EndPublishDateTime = DateTime.Now;
			Status = PageStatus.NotPublished;
		}
	}
}