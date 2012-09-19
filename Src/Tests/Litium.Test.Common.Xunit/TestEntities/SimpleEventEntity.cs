using Litium.Common.Entities;

namespace Litium.Test.Common.Xunit.TestEntities
{
	public class SimpleEventEntity : Entity
	{
		public virtual string Name { get; set; }
		public override object ValidationCopy()
		{
			return Clone();
		}
	}
}