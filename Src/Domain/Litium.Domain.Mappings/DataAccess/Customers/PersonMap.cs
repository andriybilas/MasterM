using FluentNHibernate.Mapping;
using Litium.Domain.Entities.Customers;

namespace Litium.Domain.Mappings.DataAccess.Customers
{
	public sealed class PersonMap : ClassMap<Person>
	{
		public PersonMap()
		{
			Id(x => x.Id);
			Map(x => x.HashString);
			Map(x => x.EncryptedPassword);
			Map(x => x.LoginName).Not.Nullable();
			Map(x => x.LastLoginDate).Nullable();
			Map(x => x.Role);
			Map(x => x.Active).Not.Nullable();

			Component(x => x.DeliveryAddress, m =>
												{
													m.Map(x => x.Address1);
													m.Map(x => x.Address2);
													m.Map(x => x.City);
												});

			Component(x => x.Profile, m =>
										{
											m.Map(x => x.Email);
											m.Map(x => x.FirstName);
											m.Map(x => x.LastName);
											m.Map(x => x.MiddleName);
											m.Map(x => x.Phone);
											m.Map(x => x.PhoneHome);
											m.Map(x => x.PhoneMobile);
										});
		}
	}
}
