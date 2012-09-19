using System.ComponentModel.DataAnnotations;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Test.Common.Xunit.TestEntities
{
	public class SimpleValidationEntity : Entity
	{
		[Required(ErrorMessageResourceName = "DisplayName", ErrorMessageResourceType = typeof(DomainNotification))]
		public virtual string DisplayName { get; set; }

		public virtual string ExtraInfo { get; set; }

		public override object Clone()
		{
			return new SimpleValidationEntity { DisplayName = DisplayName, ExtraInfo = ExtraInfo};
		}

		public override object ValidationCopy()
		{
			return Clone();
		}
	}
}
