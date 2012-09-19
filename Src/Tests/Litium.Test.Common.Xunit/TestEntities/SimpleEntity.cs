using System.Globalization;
using Litium.Common.Entities;

namespace Litium.Test.Common.Xunit.TestEntities
{
	public class SimpleEntity : Entity
	{
		public virtual string Column1 { get; set; }
		public virtual string Column2 { get; set; }
		public virtual string Column3 { get; set; }
		public virtual string Column4 { get; set; }

		public virtual CultureInfo Culture { get; set; }
		public virtual string Name { get; set; }
		public override object ValidationCopy()
		{
			return Clone();
		}
	}
}