using System.ComponentModel.DataAnnotations;
using Litium.Resources;

namespace Site.Infrastuctures.Security.Model
{
	public class LogOnModel
	{
		[ResourceDisplayName(ResourceKey.LoginName)]
		[Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
		public string LoginName { get; set; }

		[Required(ErrorMessageResourceName = ResourceKey.NoNullOrEmpty, ErrorMessageResourceType = typeof(DomainNotification))]
		[DataType(DataType.Password)]
        [ResourceDisplayName(ResourceKey.Password)]
		public virtual string Password { get; set; }

	}
}
