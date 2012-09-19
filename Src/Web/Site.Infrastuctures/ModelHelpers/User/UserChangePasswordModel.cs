using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using DataAnnotationsExtensions;
using Litium.Domain.Entities.Customers;
using Litium.Resources;

namespace Site.Infrastuctures.ModelHelpers.User
{
    [MetadataType(typeof(UserChangePasswordMetadata))]
    public class UserChangePasswordModel
    {
        private Guid _personId;

        public UserChangePasswordModel()
        {
        }

        public UserChangePasswordModel(Person person)
        {
            _personId = person.Id;
        }

        public virtual Guid Id { get { return _personId; } set { _personId = value; } }

        public virtual string Password { get; set; }

        public virtual string ConfirmPassword { get; set; }

        public virtual string OldPassword { get; set; }
    }

    public class UserChangePasswordMetadata
    {
        [Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
        [DataType(DataType.Password)]
        [ResourceDisplayName(ResourceKey.OldPassword)]
        public virtual string OldPassword { get; set; }

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
