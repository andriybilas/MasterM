using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Domain.Entities
{
	/// <summary>
	/// 	The Language describes the languages in the system.
	/// </summary>
	public class Language : Entity
	{
		/// <summary>
		/// 	Culture.
		/// </summary>
		[Required (ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof (DomainNotification))]
		public virtual CultureInfo Culture { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is default.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is default; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsDefault { get; set; }

		public override object ValidationCopy()
		{
			return Clone();
		}
	}
}
