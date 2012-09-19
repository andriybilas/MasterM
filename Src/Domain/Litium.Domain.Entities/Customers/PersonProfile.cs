using System;
using System.ComponentModel.DataAnnotations;
using Litium.Common.CustomSerialization;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Domain.Entities.Customers
{
    [MetadataType(typeof(PersonProfileMetadata))]
    public class PersonProfile : ComparableObject, ICloneable
	{
		/// <summary>
		/// 	Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
		public virtual string Email { get; set; }

		/// <summary>
		/// 	Gets or sets the first name.
		/// </summary>
		/// <value>The first name.</value>
		public virtual string FirstName { get; set; }

		/// <summary>
		/// 	Gets or sets the last name.
		/// </summary>
		/// <value>The last name.</value>
		public virtual string LastName { get; set; }

		/// <summary>
		/// 	Gets or sets the name of the middle.
		/// </summary>
		/// <value>The name of the middle.</value>
        [NotSerializable]
		public virtual string MiddleName { get; set; }

		/// <summary>
		/// 	Gets or sets the phone.
		/// </summary>
		/// <value>The phone.</value>
        [NotSerializable]
		public virtual string Phone { get; set; }

		/// <summary>
		/// 	Gets or sets the phone home.
		/// </summary>
		/// <value>The phone home.</value>
		public virtual string PhoneHome { get; set; }

		/// <summary>
		/// 	Gets or sets the phone mobile.
		/// </summary>
		/// <value>The phone mobile.</value>
		public virtual string PhoneMobile { get; set; }

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		public object Clone()
		{
			return MemberwiseClone();
		}
	}

    public class PersonProfileMetadata
    {
        [ResourceDisplayName(ResourceKey.Email)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual string Email { get; set; }

        [ResourceDisplayName(ResourceKey.FirstName)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual string FirstName { get; set; }

        [ResourceDisplayName(ResourceKey.LastName)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual string LastName { get; set; }

        [ResourceDisplayName(ResourceKey.MiddleName)]
        public virtual string MiddleName { get; set; }

        [ResourceDisplayName(ResourceKey.Phone)]
        public virtual string Phone { get; set; }

        [ResourceDisplayName(ResourceKey.PhoneHome)]
        public virtual string PhoneHome { get; set; }

        [ResourceDisplayName(ResourceKey.PhoneMobile)]
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual string PhoneMobile { get; set; }
    }
}