using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Domain.Entities.Publishing
{
	/// <summary>
	/// 	Web site.
	/// </summary>
	public class WebSite : Entity
	{
		/// <summary>
		/// 	Gets or sets the currency.
		/// </summary>
		/// <value>The currency.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual Currency Currency { get; set; }

		/// <summary>
		/// 	Domain name.
		/// </summary>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual string DomainName { get; set; }

		/// <summary>
		/// 	Gets or sets the framework path.
		/// </summary>
		/// <value>The framework path.</value>
		public virtual string FrameworkPath { get; set; }

		/// <summary>
		/// 	Google analytics script.
		/// </summary>
		public virtual string GoogleAnalyticsAccount { get; set; }

		/// <summary>
		/// 	Gets or sets the icon path.
		/// </summary>
		/// <value>The icon path.</value>
		public virtual string IconPath { get; set; }

		/// <summary>
		/// 	Gets a value indicating whether this instance include VAT.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance include VAT; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IncludeVat { get; set; }

		/// <summary>
		/// 	Whether this web site is the default web site or not.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is default web site; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsDefault { get; set; }

		/// <summary>
		/// 	Gets or sets the language.
		/// </summary>
		/// <value>The language.</value>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual Language Language { get; set; }

		/// <summary>
		/// 	The name of the web site.
		/// </summary>
        [Required(ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof(Notification))]
		public virtual string Name { get; set; }

		/// <summary>
		/// 	Gets or sets the start page.
		/// </summary>
		/// <value>The start page.</value>
		public virtual Page StartPage { get; set; }

		/// <summary>
		/// 	Gets or sets a value indicating whether [use page permissions in GUI].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [use page permissions in GUI]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool UsePagePermissionsInGui { get; set; }

		/// <summary>
		/// 	Gets or sets a value indicating whether [use page responsibility in GUI].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [use page responsibility in GUI]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool UsePageResponsibilityInGui { get; set; }
	}
}