using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Litium.Common;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ECommerce;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Domain.Entities.Shop;
using Litium.Test.Common.Xunit.Base;
using Site.Infrastuctures.ModelHelpers.Synchronization;
using Xunit;

namespace Litium.Test.Domain.Xunit.EShop
{
	public class EShopEntityTests : ConversationalTestBase
	{
		private Guid _testGuid;
		private readonly DateTime _createTestDate;

		public EShopEntityTests()
		{
			_testGuid = Guid.NewGuid();
			_createTestDate = DateTime.Now;
		}

	    [Fact] 
        public void TryToSerializeProductEntity ()
	    {
	        IEnumerable<Product> product = Repository.Data.Get<Product>().Where( x => x.Category.Name.Equals("Dairy", StringComparison.CurrentCultureIgnoreCase)).All().Take(5); 
            SynchService synch = new SynchService();
            XDocument xDocument = synch.ConvertToXDocument(product);
            xDocument.Save(@"E:\temp\Products.xml");

	        List<Order> orders = Repository.Data.Get<Order>().All().Take( 3 ).ToList();
	        Order order = Repository.Data.Get<Order>().Where(x => x.OrderNumber != null).FirstOrDefault().Value;
            orders.Add(order);
            xDocument = synch.ConvertToXDocument(orders);
            xDocument.Save(@"E:\temp\Orders.xml");

	        IEnumerable<Person> persons = Repository.Data.Get<Person>().All().Take(3);
            xDocument = synch.ConvertToXDocument(persons);
            xDocument.Save(@"E:\temp\Persons.xml");
	    }

		[Fact]
        public IEnumerable<Product> DeserializeProductTest()
		{
			SynchService synch = new SynchService();
			XDocument xDocument = XDocument.Load(@"E:\temp\Products.xml");
			IEnumerable<Product> products = synch.CreateProductsFromXDocument(xDocument);
            Assert.True(products.Any());
		    return products;
		}


		[Fact]
		public Product ProductCreateTest()
		{
			var product = new Product {
				Name = "Product " + _testGuid.ToString(),
				Category = CategoryCreateTest(),
				CreateDate = _createTestDate,
				Description = "Description1",
				Price = 10.00m,
				ProductProperty = ProductPropertyCreateTest(),
				Published = false,
				StockBalance = 10,
				UpdateDate = _createTestDate
			};

			Repository.Data.Save(product);
			Assert.True(product.Id != Guid.Empty);
			return product;
		}

		[Fact]
		public ProductProperty ProductPropertyCreateTest()
		{
			var property = new ProductProperty {
				Brend = "LG",
				Capacity = "10l",
				Weight = "100g",
				Country = "Ukraine"
			};

			Repository.Data.Save(property);
			Assert.True(property.Id != Guid.Empty);
			return property;
		}

		[Fact]
		public Category CategoryCreateTest()
		{
			var category = new Category {
				Name = "Category1",
				Description = "Description1",
				Parent = null
			};

			Repository.Data.Save(category);
			Assert.True(category.Id != Guid.Empty);
			return category;
		}

		[Fact]
		public OrderProduct OrderProductSaveTest()
		{
			Product product = ProductCreateTest();
			var orderProduct = new OrderProduct {
				ProductName = product.Name,
				Product = product,
				Price = product.Price,
				CampaignPrice = 21.0m,
				Count = 1,
				Summa = 22.00m
			};

			Repository.Data.Save(orderProduct);
			Assert.True(product.Id != Guid.Empty);
			return orderProduct;
		}

		[Fact]
		public Order OrderSaveTest()
		{
			Order order = new Order {
				CreateDate = _createTestDate,
				Customer = CustomerSaveTest(),
				DeliveryMethod = DeliveryMethodSaveTest(),
				OrderState = State.Created,
				OrderSumma = 100.00m,
				PaymentMethod = PaymentMethodSaveTest()
			};

			Repository.Data.Save(order);
			Assert.True(order.Id != Guid.Empty);

			order.OrderProducts.Add(OrderProductSaveTest());
			Assert.True(order.OrderProducts.Count > 0);
			return order;
		}

		[Fact]
		public PaymentMethod PaymentMethodSaveTest()
		{
			var method = new PaymentMethod {
				Name = "PaymentMethod1",
				Description = "Description1",
				Cost = 12.1m
			};

			Repository.Data.Save(method);
			Assert.True(method.Id != Guid.Empty);
			return method;
		}

		[Fact]
		public DeliveryMethod DeliveryMethodSaveTest()
		{
			var deliveryMethod = new DeliveryMethod {
				Name = "DeliveryMethod1",
				Description = "Description1",
				Cost = 12.05m
			};

			Repository.Data.Save(deliveryMethod);
			Assert.True(deliveryMethod.Id != Guid.Empty);
			return deliveryMethod;
		}

		[Fact]
		public Person CustomerSaveTest()
		{
			var person = new Person {
				LoginName = "LoginName1",
				DeliveryAddress = DeliveryAddressCreateTest(),
				LastLoginDate = _createTestDate,
				Profile = PersonProfileCreateTest(),
				Role = UserRole.Customer
			};

			person.SetPassword("Babagalamaga03");

			Repository.Data.Save(person);
			Assert.True(person.Id != Guid.Empty);
			return person;
		}

		private PersonProfile PersonProfileCreateTest()
		{
			return new PersonProfile {
				FirstName = "FirstName1",
				LastName = "LastName1",
				Email = "Email1",
				MiddleName = "Middle Name",
				Phone = "0674184225",
				PhoneHome = "2424222",
				PhoneMobile = "24564654"
			};
		}

		private Address DeliveryAddressCreateTest()
		{
			return new Address {
				Address1 = "Address1",
				Address2 = "Address2",
				City = "City1"
			};
		}

		[Fact]
		public void CampaignCreateTest()
		{
			var campaign = new Campaign {
				Name = "Campaign1",
				Description = "Description1",
				StartDate = _createTestDate,
				EndDate = _createTestDate,
			};

			campaign.Metadata.CampaignType = "Universal";
			campaign.Metadata.CampaignData = "Some campaign data";

			Repository.Data.Save(campaign);
			Assert.True(campaign.Id != Guid.Empty);

			var campaign1 = Repository.Data.Get<Campaign>(campaign.Id);
			Assert.True (campaign1.Metadata.CampaignType.Equals("Universal"));
			Assert.True (campaign1.Metadata.CampaignData.Equals ("Some campaign data"));
		}
	}
}
