using System;
using System.ComponentModel.DataAnnotations;
using Litium.Domain.Entities.Customers;
using Litium.Resources;

namespace Site.Infrastuctures.ModelHelpers.User
{
    [MetadataType(typeof(UserProfileMetadata))]
    public class UserProfileModel
    {
        private readonly Person _person;
        private Guid _personId;

        public UserProfileModel()
        {
            _person = new Person();
            _person.DeliveryAddress = new Address();
            _person.Profile = new PersonProfile();
        }

        public UserProfileModel(Person person)
        {
            _person = person;
            _personId = person.Id;
        }

        public virtual Guid Id { get { return _personId; } set { _personId = value; } }

        public virtual string Address1
        {
            get
            {
                if (_person.DeliveryAddress == null)
                    return String.Empty;
                return _person.DeliveryAddress.Address1;
            }
            set
            {
                if (_person.DeliveryAddress == null)
                    _person.DeliveryAddress = new Address();
                _person.DeliveryAddress.Address1 = value;
            }
        }

        public virtual string City
        {
            get
            {
                if (_person.DeliveryAddress == null)
                    return String.Empty;
                return _person.DeliveryAddress.City;
            } 
            set
            {
                if (_person.DeliveryAddress == null)
                    _person.DeliveryAddress = new Address();
                _person.DeliveryAddress.City = value;
            }
        }

        public virtual string Email
        {
            get
            {
                if (_person.Profile == null)
                    return String.Empty;
                return _person.Profile.Email;
            } 
            set
            {
                if (_person.Profile == null)
                    _person.Profile = new PersonProfile();
                _person.Profile.Email = value;
            }
        }

        public virtual string DisplayName { get; set; }

        public virtual string FirstName
        {
            get
            {
                if (_person.Profile == null)
                    return String.Empty;
                return _person.Profile.FirstName;
            }
            set
            {
                if (_person.Profile == null)
                    _person.Profile = new PersonProfile();
                _person.Profile.FirstName = value;
            }
        }

        public virtual string LastName
        {
            get
            {
                if (_person.Profile == null)
                    return String.Empty;
                return _person.Profile.LastName;
            } 
            set
            {
                if (_person.Profile == null)
                    _person.Profile = new PersonProfile();
                _person.Profile.LastName = value;
            }
        }

        public virtual string MiddleName
        {
            get
            {
                if (_person.Profile == null)
                    return String.Empty;
                return _person.Profile.MiddleName;
            } 
            set
            {
                if (_person.Profile == null)
                    _person.Profile = new PersonProfile();
                _person.Profile.MiddleName = value;
            }
        }

        public virtual string PhoneHome
        {
            get
            {
                if (_person.Profile == null)
                    return String.Empty;
                return _person.Profile.PhoneHome;
            } 
            set
            {
                if (_person.Profile == null)
                    _person.Profile = new PersonProfile();
                _person.Profile.PhoneHome = value;
            }
        }

        public virtual string PhoneMobile
        {
            get
            {
                if (_person.Profile == null)
                    return String.Empty;
                return _person.Profile.PhoneMobile;
            } 
            set
            {
                if (_person.Profile == null)
                    _person.Profile = new PersonProfile();
                _person.Profile.PhoneMobile = value;
            }
        }
    }

    public class UserProfileMetadata
    {
        [ResourceDisplayName(ResourceKey.Address1)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual string Address1 { get; set; }

        [ResourceDisplayName(ResourceKey.City)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual string City { get; set; }

        [ResourceDisplayName(ResourceKey.Email)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }

        [ResourceDisplayName(ResourceKey.FirstName)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual string FirstName { get; set; }

        [ResourceDisplayName(ResourceKey.LastName)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual string LastName { get; set; }

        [ResourceDisplayName(ResourceKey.MiddleName)]
        public virtual string MiddleName { get; set; }

        [ResourceDisplayName(ResourceKey.PhoneHome)]
        [DataType(DataType.PhoneNumber)]
        public virtual string PhoneHome { get; set; }

        [ResourceDisplayName(ResourceKey.PhoneMobile)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [DataType(DataType.PhoneNumber)]
        public virtual string PhoneMobile { get; set; }
    }
}
