using Litium.Common.Entities;

namespace Litium.Test.Common.Xunit.TestEntities
{
	public class SimpleCacheEntity : Entity
	{
		public virtual string Name { get; set; }

		public override object Clone()
		{
			return new SimpleEventEntity { Name = Name };
		}

		public override object ValidationCopy()
		{
			return Clone();
		}
	}
}
