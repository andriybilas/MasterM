using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Litium.Common.Entities;
using Litium.Resources;

namespace Litium.Test.Common.Xunit.TestEntities
{
    public class LocalizationTestEntity : Entity
    {
        public virtual string Column1 { get; set; }
        public virtual string Column2 { get; set; }
        public virtual string Column3 { get; set; }
        public virtual string Column4 { get; set; }

        [Required(ErrorMessageResourceName = "LocalizationTestMessage", ErrorMessageResourceType = typeof(DomainNotification))]
        public virtual string Name { get; set; }
        public virtual CultureInfo Culture { get; set; }

    	public override object ValidationCopy()
    	{
    		return Clone();
    	}
    }
}
