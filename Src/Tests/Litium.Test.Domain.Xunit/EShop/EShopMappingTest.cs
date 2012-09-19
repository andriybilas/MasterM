using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FluentNHibernate.Testing;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ECommerce;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Domain.Entities.Shop;
using Litium.Infrastructure.DataAccess.Collection;
using Litium.Test.Domain.Xunit.Mappings.Base;
using NHibernate.Impl;
using Xunit;

namespace Litium.Test.Domain.Xunit.Mappings
{

	public class EShopMappingTest : MappingTestBase
	{
		private readonly DateTime _testDateTime = new DateTime(2012, 05, 17, 19, 50, 30);
		private readonly PersonProfile _personProfile;
		private readonly Address _address;


		public EShopMappingTest()
		{
			_personProfile = new PersonProfile
								 {
									 Email = "Email",
									 FirstName = "FirstName",
									 LastName = "LastName",
									 MiddleName = "MiddleName",
									 Phone = "0121654651",
									 PhoneHome = "0121654651",
									 PhoneMobile = "0121654651"
								 };

			_address = new Address
						   {
							   Address1 = "Some address",
							   Address2 = "some adress",
							   City = "Lviv"
						   };

		}

		[Fact]
		public Person PersonMapTest()
		{
			return new PersistenceSpecification<Person>(Session)
				.CheckProperty(x => x.HashString, "BabagAlamaga03")
				.CheckProperty(x => x.LoginName, "Template 66")
				.CheckProperty(x => x.LastLoginDate, _testDateTime)
				.CheckProperty(x => x.Role, UserRole.Customer)
				.CheckProperty(x => x.DeliveryAddress, _address)
				.CheckProperty(x => x.Profile,_personProfile )
				.VerifyTheMappings();
		}

		[Fact]
		public ProductSet ProductSetMappingTest()
		{
			return new PersistenceSpecification<ProductSet>(Session)
				.CheckProperty(x => x.Name, "ProductSetName1")
				.CheckProperty(x => x.Description, "Description1")
				.VerifyTheMappings();
		}

		[Fact]
		public Campaign CampaignMappingTest()
		{
			return new PersistenceSpecification<Campaign>(Session)
				.CheckProperty(x => x.Name, "Campaign1")
				.CheckProperty(x => x.Description, "Description1")
				.CheckProperty(x => x.Active, false)
				.CheckProperty(x => x.StartDate, _testDateTime)
				.CheckProperty(x => x.EndDate, _testDateTime)
				.VerifyTheMappings();
		}

		[Fact]
		public Product ProductMappingTest()
		{
			return new PersistenceSpecification<Product>(Session)
				.CheckProperty(x => x.Name, "ProductName")
				.CheckProperty(x => x.Description, "Description1")
				.CheckProperty(x => x.CreateDate, _testDateTime)
				.CheckProperty(x => x.UpdateDate, _testDateTime)
				.CheckProperty(x => x.Price, 10.42m)
				.CheckProperty(x => x.StockBalance, 10)
				.CheckProperty(x => x.Published, false)
				.CheckReference(x => x.Category, CategoryMapTest())
				//.CheckProperty(x => x.ProductSets, ProductSetMapTest())
				.VerifyTheMappings();
		}

		[Fact]
		public ISet<ProductSet> ProductSetMapTest()
		{
			ProductSet productSet = new PersistenceSpecification<ProductSet>(Session)
			.CheckProperty(x => x.Name, "ProductSet1")
			.CheckProperty(x => x.Description, "Description1")
			//.CheckReference(x => x.Campaign, CampaignMappingTest())
			.VerifyTheMappings();
			return new HashSet<ProductSet> { productSet };
		}

		[Fact]
		public Category CategoryMapTest()
		{
			return new PersistenceSpecification<Category>(Session)
				.CheckProperty(x => x.Name, "Category1")
				.CheckProperty(x => x.Description, "Description1")
				.CheckProperty(x =>x.Parent, null)
				.VerifyTheMappings();
		}

		[Fact]
		public DeliveryMethod DeliveryMethodTest()
		{
			return new PersistenceSpecification<DeliveryMethod>(Session)
				.CheckProperty(x => x.Name, "Home")
				.CheckProperty(x => x.Description, "Description")
				.CheckProperty(x => x.Cost, 10.00m)
				.VerifyTheMappings();
		}

		[Fact]
		public PaymentMethod  PaymentMethodTest()
		{
			return new PersistenceSpecification<PaymentMethod>(Session)
				.CheckProperty(x => x.Name, "PayPal")
				.CheckProperty(x => x.Description, "Description")
				.CheckProperty(x => x.Cost, 0m)
				.VerifyTheMappings();
		}

		[Fact]
		public ISet<OrderProduct> OrderProductTest ()
		{
		    Product product = ProductMappingTest();
			OrderProduct orderProduct = new PersistenceSpecification<OrderProduct>(Session)
				.CheckProperty(x => x.ProductName, product.Name)
				.CheckProperty(x => x.Product, product)
				.CheckProperty(x => x.Price, 10.00m)
				.CheckProperty(x => x.Count, 10)
				.CheckProperty(x => x.CampaignPrice, 9.99m)
				.CheckProperty(x => x.Summa, 99.90m)
				.VerifyTheMappings();
			return new HashSet<OrderProduct> { orderProduct };
		}

		[Fact]
		public Order OrderTest()
		{
			return new PersistenceSpecification<Order>(Session)
			.CheckProperty(x =>x.Customer, PersonMapTest())
			.CheckProperty(x =>x.OrderProducts, OrderProductTest())
			.CheckProperty(x =>x.CreateDate, _testDateTime)
			.CheckProperty(x =>x.DeliveryMethod, DeliveryMethodTest() )
			.CheckProperty(x =>x.PaymentMethod, PaymentMethodTest() )
			.CheckProperty(x =>x.OrderState, State.Created)
			.CheckProperty(x =>x.OrderSumma, 100.00m )
			.VerifyTheMappings();
		}
	}
}
