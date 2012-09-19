using System;
using System.ComponentModel.DataAnnotations;
using Litium.Common.Validation.CustomAttribute;
using Litium.Resources;

namespace Litium.Domain.Entities.Publishing
{
	/// <summary>
	/// 	Working copy of a page
	/// </summary>
	public class WorkingCopy : PageBase
	{
		/// <summary>
		/// 	The page the working copy created to.
		/// </summary>
		/// <value>The page.</value>
		public virtual Page Page { get; protected internal set; }

		/// <summary>
		/// 	Gets or sets the publish date time.
		/// </summary>
		/// <value>The publish date time.</value>
		public virtual DateTime? PublishDateTime { get; protected internal set; }

		/// <summary>
		/// 	Gets or sets the name of the URL.
		/// </summary>
		/// <value>The name of the URL.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual string UrlName { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
        [EnumTypeCompatible(ErrorMessageResourceName = "EnumFormat", ErrorMessageResourceType = typeof(Notification))]
		public virtual WorkingCopyStatus Status { get; protected internal set; }

		/// <summary>
		/// 	Delays the publish.
		/// </summary>
		/// <param name = "publishDate">The publish date.</param>
		public virtual void DelayPublish(DateTime publishDate)
		{
			PublishDateTime = publishDate;
			Status = WorkingCopyStatus.DelayedPublish;
		}

		/// <summary>
		/// 	Not ready for publish.
		/// </summary>
		public virtual void NotReadyForPublish()
		{
			PublishDateTime = null;
			Status = WorkingCopyStatus.NotPublished;
		}
	}
}