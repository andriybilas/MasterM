using Litium.Common;
using Litium.Common.Validation;
using Litium.Test.Common.Xunit.Base;
using Litium.Test.Common.Xunit.TestEntities;
using Xunit;

namespace Litium.Test.Common.Xunit.Validation
{
	public class ValidationTest : ConversationalTestBase
	{
		[Fact]
		public void ValidateSimpleTest()
		{
			var entity = new SimpleValidationEntity();
			//DisplayName is required but isn't initialized.
			Assert.Throws<ValidationArgumentException>(() => Repository.Data.Save(entity));
		}
	}
}