using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Litium.Common.CustomSerialization;
using Litium.Common.Entities;
using Litium.Common.Validation.CustomAttribute;
using Litium.Resources;

namespace Litium.Domain.Entities.Customers
{
	/// <summary>
	/// 	Person.
	/// </summary>
    [MetadataType(typeof(PersonMetadata))]
	public class Person : Entity
	{
	    private static readonly SHA1CryptoServiceProvider _provider = new SHA1CryptoServiceProvider();

        [NotSerializable]
		public virtual Address DeliveryAddress { get; set; }

        [NotSerializable]
        public virtual bool Active { get; set; }

        [NotSerializable]
		public virtual PersonProfile Profile { get; set; }

        [NotSerializable]
		public virtual UserRole Role { get; set; }

        public virtual string LoginName { get; set; }

        [NotSerializable]
		public virtual DateTime LastLoginDate { get; set; }

        [NotSerializable]
		public virtual string EncryptedPassword { get; protected set; }

        [NotSerializable]
		public virtual string HashString { get; protected set; }

		public virtual void SetPassword(string password)
		{
			var hash = GeneratePasswordSalt();
			EncryptedPassword = EncryptPassword(password, hash);
			HashString = hash;
		}

		public virtual bool Validate(string password)
		{
			var encryptedPassword = EncryptPassword(password, HashString);
			return String.Equals(EncryptedPassword, encryptedPassword);
		}

		private string EncryptPassword(string password, string salt)
		{
			var passwordAsBytes = (new UnicodeEncoding()).GetBytes(String.Concat(password, salt));
			var result = (_provider).ComputeHash(passwordAsBytes);
			return BitConverter.ToString(result);
		}

		private string GeneratePasswordSalt()
		{
			return BitConverter.ToString(_provider.ComputeHash(Guid.NewGuid().ToByteArray()));
		}

		/// <summary>
		/// Create a shallow copy of object.
		/// </summary>
		/// <returns>new Person object.</returns>
		public override object Clone()
		{
			var clone = (Person)base.Clone();
			if (DeliveryAddress != null)
				clone.DeliveryAddress = (Address)DeliveryAddress.Clone();
			if (Profile != null)
				clone.Profile = (PersonProfile)Profile.Clone();
			return clone;
		}

		public override object ValidationCopy()
		{
			return Clone();
		}
	}

    public class PersonMetadata
    {
        [ResourceDisplayName(ResourceKey.PersonActive)]
        public virtual bool Active { get; set; }

        [EnumTypeCompatible(ErrorMessageResourceName = ResourceKey.EnumFormat, ErrorMessageResourceType = typeof(DomainNotification))]
        public UserRole Role { get; set; }

        [ResourceDisplayName(ResourceKey.LoginName)]
        public string LoginName { get; set; }

        [ResourceDisplayName(ResourceKey.LastLogin)]
        public DateTime LastLoginDate { get; set; }

        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [ScaffoldColumn(true)]
        public string EncryptedPassword { get; protected set; }

        public string HashString { get; protected set; }
    }
}