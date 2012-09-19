using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Domain.Entities
{
	/// <summary>
	/// 	This class represents the currency.
	/// </summary>
	public class Currency : Entity
	{
		/// <summary>
		/// 	Gets or sets the currency code.
		/// </summary>
		/// <value>The currency code.</value>
		[Required (ErrorMessageResourceName = "NoNullOrEmpty", ErrorMessageResourceType = typeof (DomainNotification))]
		public virtual string Code { get; set; }

		/// <summary>
		/// 	Gets or sets the text format.
		/// </summary>
		/// <value>The text format.</value>
		public virtual string TextFormat { get; set; }

		public override object ValidationCopy()
		{
			return Clone();
		}
	}
}