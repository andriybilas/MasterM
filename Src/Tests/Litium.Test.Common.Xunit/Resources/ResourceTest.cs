using System.Globalization;
using System.Threading;
using Litium.Common;
using Litium.Common.Resources;
using Litium.Test.Common.Xunit.Base;
using Litium.Test.Common.Xunit.TestEntities;
using Xunit;

namespace Litium.Test.Common.Xunit.Resources
{
	public class ResourceTest : ConversationalTestBase
	{
		[Fact]
		public void WebStringTest()
		{
		    string systemString = ResourceExpressionBuilder.GetWebString("System.No");
			Assert.True(systemString.Equals("No"));

            // Check for an other culture
		    var currentUiCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfoByIetfLanguageTag("sv-se");
		    systemString = ResourceExpressionBuilder.GetWebString("System.No");
            Assert.True(systemString.Equals("Nej"));
		    
            // Check fallback 
            Thread.CurrentThread.CurrentCulture = currentUiCulture;
            systemString = ResourceExpressionBuilder.GetWebString("System.Yes");
            Assert.True(systemString.Equals("Yes"));

		    Thread.CurrentThread.CurrentUICulture = currentUiCulture;
		}   
	}
}