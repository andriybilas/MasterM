using System;
using Litium.Common.Entities;

namespace Litium.Domain.Entities.Customers
{
	/// <summary>
	/// 	Address list member.
	/// </summary>
	public class AddressListMember : Entity
	{
		/// <summary>
		/// 	Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
		public virtual string Email { get; set; }

		/// <summary>
		/// 	Gets or sets the person.
		/// </summary>
		/// <value>The person.</value>
		public virtual Person Person { get; set; }

		/// <summary>
		/// 	Whether the receiver unsubscribed.
		/// </summary>
		/// <value><c>true</c> if unsubscribed; otherwise, <c>false</c>.</value>
		public virtual bool Subscribed { get; set; }

		/// <summary>
		/// 	When the receiver unsubscribed.
		/// </summary>
		/// <value>The unsubscribed date time.</value>
		public virtual DateTime SubscribedDateTime { get; set; }

		public override object ValidationCopy()
		{
			return MemberwiseClone();
		}
	}
}