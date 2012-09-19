using System;
using System.Globalization;
using FluentNHibernate.Testing;
using Litium.Domain.Entities;
using Litium.Test.Domain.Xunit.Mappings.Base;
using Xunit;

namespace Litium.Test.Domain.Xunit.Mappings
{
	public class CommonMappingTest : MappingTestBase
	{
		[Fact]
		public Currency MapCurrency()
		{
			var uniqueCode = Guid.NewGuid().ToString();

			return new PersistenceSpecification<Currency>(Session)
				.CheckProperty(x => x.Code, uniqueCode)
				.CheckProperty(x => x.TextFormat, "C")
				.VerifyTheMappings();
		}

		[Fact]
		public Language MapLanguage()
		{
			var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
			var index = new Random().Next(0, allCultures.Length - 1);
			var uniqueCulture = allCultures[index];

			return new PersistenceSpecification<Language>(Session)
				.CheckProperty(x => x.Culture, uniqueCulture)
				.CheckProperty(x => x.IsDefault, true)
				.VerifyTheMappings();
		}
	}
}
