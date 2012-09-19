using System;
using Litium.Domain.Entities.Customers;

namespace Site.Infrastuctures.Security.Model
{
	public class CompleteRegistartionUserModel
	{
		public Guid Id { get; set; }

		public virtual string LoginName { get; set; }

		public virtual Address DeliveryAddress { get; set; }

		public virtual PersonProfile Profile { get; set; }
	}
}