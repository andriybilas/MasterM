using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using Litium.Resources;

namespace Site.Infrastuctures.Security.Model
{
	public class SimpleRegistartionUserModel
	{
		public Guid Id { get; set; }

		[ResourceDisplayName(ResourceKey.LoginName)]
		[Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
		public string LoginName { get; set; }

		[Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
		[DataType(DataType.Password)]
        [ResourceDisplayName(ResourceKey.Password)]
		public virtual string Password { get; set; }

		[EqualTo("Password")]
		[DataType(DataType.Password)]
        [ResourceDisplayName(ResourceKey.PasswordConfirm)]
		public virtual string ConfirmPassword { get; set; }
	}
}