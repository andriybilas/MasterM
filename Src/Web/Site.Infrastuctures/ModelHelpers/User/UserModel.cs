using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using Litium.Domain.Entities.Customers;
using Litium.Resources;

namespace Site.Infrastuctures.ModelHelpers.User
{
    [MetadataType(typeof(UserMetadata))]
    public class UserModel
    {
        private readonly Person _person;
        private Guid _personId;

        public UserModel()
        {
            _person = new Person();
            _person.DeliveryAddress = new Address();
            _person.Profile = new PersonProfile();
        }

        public UserModel(Person person)
        {
            _person = person;
            _personId = person.Id;

        	if(person.Profile == null)
        		_person.Profile = new PersonProfile();

			if(person.DeliveryAddress == null)
				_person.DeliveryAddress = new Address();
        }

        public virtual String LoginName { get { return _person.LoginName; } set { _person.LoginName = value; } }

        public virtual Guid Id { get { return _personId; } set { _personId = value; } }

        public virtual bool Active { get { return _person.Active; } set { _person.Active = value; } }

        public virtual UserRole Role { get { return _person.Role; } set { _person.Role = value; } }

        [DataType(DataType.DateTime)]
        public virtual DateTime LastLoginDate { get { return _person.LastLoginDate; } set { _person.LastLoginDate = value; } }

        public virtual string Address1 { get { return _person.DeliveryAddress.Address1; } set { _person.DeliveryAddress.Address1 = value; } }

        public virtual string City { get { return _person.DeliveryAddress.City; } set { _person.DeliveryAddress.City = value; } }

        public virtual string Email { get { return _person.Profile.Email; } set { _person.Profile.Email = value; } }

        public virtual string DisplayName 
		{ 
			get
			{
				if (String.IsNullOrWhiteSpace(_person.Profile.FirstName))
					return _person.LoginName;
				return String.Format("{0} {1}", _person.Profile.FirstName, _person.Profile.LastName);
			} 
		}

        public virtual string FirstName { get { return _person.Profile.FirstName; } set { _person.Profile.FirstName = value; } }

        public virtual string LastName { get { return _person.Profile.LastName; } set { _person.Profile.LastName = value; } }

        public virtual string MiddleName { get { return _person.Profile.MiddleName; } set { _person.Profile.MiddleName = value; } }

        public virtual string PhoneHome { get { return _person.Profile.PhoneHome; } set { _person.Profile.PhoneHome = value; } }

        public virtual string PhoneMobile { get { return _person.Profile.PhoneMobile; } set { _person.Profile.PhoneMobile = value; } }
        
        public virtual string Password { get; set; }

        public virtual string ConfirmPassword { get; set; }
    }

    public class UserMetadata
    {
        [ResourceDisplayName(ResourceKey.PersonActive)]
        public virtual bool Active { get; set; }

        [ResourceDisplayName(ResourceKey.LastLogin)]
        public virtual UserRole Role { get; set; }

        public virtual DateTime LastLoginDate { get; set; }

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

        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [DataType(DataType.Password)]
        [ResourceDisplayName(ResourceKey.Password)]
        public virtual string Password { get; set; }

        [EqualTo("Password")]
        [DataType(DataType.Password)]
        [ResourceDisplayName(ResourceKey.PasswordConfirm)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual string ConfirmPassword { get; set; }
    }
}
