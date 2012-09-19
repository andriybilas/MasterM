using Litium.Domain.Entities.Customers;

namespace Site.Infrastuctures.ModelHelpers.User
{
    public static class PersonModelExtension
    {
        public static void CopyFrom(this Person source, UserModel copied)
        {
            source.Active = copied.Active;

            if (source.Profile == null)
                source.Profile = new PersonProfile();

            source.Profile.FirstName = copied.FirstName;
            source.Profile.LastName = copied.LastName;
            source.Profile.MiddleName = copied.MiddleName;
            source.Profile.PhoneHome = copied.PhoneHome;
            source.Profile.PhoneMobile = copied.PhoneMobile;
            source.Profile.Email = copied.Email;

            if (source.DeliveryAddress == null)
                source.DeliveryAddress = new Address();

            source.DeliveryAddress.Address1 = copied.Address1;
            source.DeliveryAddress.City = copied.City;
        }

        public static void CopyFrom(this Person source, UserProfileModel copied)
        {
            if(source.Profile == null) 
                source.Profile = new PersonProfile();

            source.Profile.FirstName = copied.FirstName;
            source.Profile.LastName = copied.LastName;
            source.Profile.MiddleName = copied.MiddleName;
            source.Profile.PhoneHome = copied.PhoneHome;
            source.Profile.PhoneMobile = copied.PhoneMobile;
            source.Profile.Email = copied.Email;

            if (source.DeliveryAddress == null)
                source.DeliveryAddress = new Address();

            source.DeliveryAddress.Address1 = copied.Address1;
            source.DeliveryAddress.City = copied.City;
        }
    }
}