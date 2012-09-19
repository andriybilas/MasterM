using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Litium.Common;
using Litium.Domain.Entities.Customers;
using Litium.Domain.Entities.ECommerce;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Domain.Entities.Shop;
using LProduct = Litium.Domain.Entities.ProductCatalog.Product;
using LNotSerializable = Litium.Common.CustomSerialization.NotSerializableAttribute;
using LOrder = Litium.Domain.Entities.ECommerce.Order;

namespace Site.Infrastuctures.ModelHelpers.Synchronization
{
    public class SynchService
    {
		private XElement ConvertToXElement( LProduct product )
		{
			PropertyInfo[] propertyInfos = product.GetType().GetProperties();
			var xProduct = new XElement(NodesName.Product);

			foreach (var property in propertyInfos)
			{
				if (!Attribute.IsDefined(property, typeof(LNotSerializable), true))
				{
					var xElement = new XElement(property.Name) { Value = property.GetNullValue(product) };
					xProduct.Add(xElement);
				}

				if (property.PropertyType == typeof(Category))
				{
					var xElement = new XElement(property.Name) { Value = product.Category != null ? product.Category.Name : "NULL" };
					xProduct.Add(xElement);
				}

				if (property.PropertyType == typeof(ProductProperty))
				{
					var xElement = ConvertNestedToXElement(product.ProductProperty);
					xProduct.Add(xElement);
				}
			}

			return xProduct;
		}

		private XElement ConvertToXElement( LOrder order )
		{
			PropertyInfo[] propertyInfos = order.GetType().GetProperties();
			var xProduct = new XElement(NodesName.Order);

			foreach (var property in propertyInfos)
			{
				if (!Attribute.IsDefined(property, typeof(LNotSerializable), true))
				{
					var xElement = new XElement(property.Name) { Value = property.GetNullValue(order) };
					xProduct.Add(xElement);
				}

				if (property.PropertyType == typeof(Person))
				{
					var xElement = ConvertToXElement(order.Customer);
					xProduct.Add(xElement);
				}

				//if (property.PropertyType == typeof(State))
				//{
				//    var xElement = ConvertToXElement(order.OrderState);
				//    xProduct.Add(xElement);
				//}

				if (property.PropertyType == typeof(IList<OrderProduct>))
				{
					xProduct.Add(ConvertToXElement(order.OrderProducts));
				}
			}

			return xProduct;
		}

        private XElement ConvertToXElement(State productProperty)
        {
            var xElement = new XElement(NodesName.OrderState);
            xElement.Value = productProperty.ToString();
            return xElement;
        }

        private XElement ConvertToXElement( IEnumerable<OrderProduct> productProperty )
		{
			var xElement = new XElement(NodesName.OrderProducts);
			foreach (var product in productProperty)
			{
				xElement.Add(ConvertToXElement(product));
			}
			return xElement;
		}

		private XElement ConvertToXElement( OrderProduct orderProduct )
		{
			PropertyInfo[] propertyInfos = orderProduct.GetType().GetProperties();
			var xProduct = new XElement(NodesName.Product);

			foreach (var property in propertyInfos)
			{
				if (property.Name.InvariantEquals("Id"))
					continue;

				if (!Attribute.IsDefined(property, typeof(LNotSerializable), true))
				{
					var xElement = new XElement(property.Name) { Value = property.GetNullValue(orderProduct) };
					xProduct.Add(xElement);
				}

				if (property.PropertyType == typeof(LProduct))
				{
					xProduct.Add(ConvertProductToXElementForOrder(orderProduct.Product));
				}
			}

			return xProduct;
		}

		private IEnumerable<XElement> ConvertProductToXElementForOrder( LProduct product )
		{
			PropertyInfo[] propertyInfos = product.GetType().GetProperties();
			var xProduct = new List<XElement>();

			foreach (var info in propertyInfos)
			{
				if (info.Name.InvariantEquals("Name") || info.Name.InvariantEquals("Article"))
				{
					var xElement = new XElement(info.Name);
					xElement.Value = info.GetNullValue(product);
					xProduct.Add(xElement);
				}
			}

			return xProduct;
		}

		private XElement ConvertToXElement( Person customer )
		{
			PropertyInfo[] propertyInfos = customer.GetType().GetProperties();
			var xCustomer = new XElement(NodesName.Customer);

			foreach (var property in propertyInfos)
			{
				if (property.Name.InvariantEquals("Id"))
					continue;

				if (!Attribute.IsDefined(property, typeof(LNotSerializable), true))
				{
					var xElement = new XElement(property.Name) { Value = property.GetNullValue(customer) };
					xCustomer.Add(xElement);
				}

				if (property.PropertyType == typeof(PersonProfile))
				{
					var xElement = ConvertNestedToXElement(customer.Profile);
					xCustomer.Add(xElement);
				}

				if (property.PropertyType == typeof(Address))
				{
					var xElement = ConvertNestedToXElement(customer.DeliveryAddress);
					xCustomer.Add(xElement);
				}
			}
			return xCustomer;
		}

		private List<XElement> ConvertNestedToXElement<T>( T source )
		{
			PropertyInfo[] propertyInfos = source.GetType().GetProperties();
			return (from info in propertyInfos
					where !Attribute.IsDefined(info, typeof(LNotSerializable), true) && !info.Name.InvariantEquals("Id")
					select new XElement(String.Format("{0}.{1}", source.GetType().UnderlyingSystemType.Name, info.Name)) { Value = info.GetNullValue(source) }).ToList();
		}

        public XDocument ConvertToXDocument(IEnumerable<LProduct> products)
        {
            var xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            xDocument.Add(new XElement(NodesName.ProductCollection));

            foreach (var product in products)
            {
                xDocument.Root.Add(ConvertToXElement(product));
            }

            return xDocument;
        }

        public XDocument ConvertToXDocument(IEnumerable<LOrder> orders)
        {
            var xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            xDocument.Add(new XElement(NodesName.OrderCollection));

            foreach (var product in orders)
            {
                xDocument.Root.Add(ConvertToXElement(product));
            }

            return xDocument;
        }

        public XDocument ConvertToXDocument(IEnumerable<Person> persons)
        {
            var xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            xDocument.Add(new XElement(NodesName.CustomerCollection));

            foreach (var person in persons)
            {
                xDocument.Root.Add(ConvertToXElement(person));
            }

            return xDocument;
        }

		public IEnumerable<LProduct> CreateProductsFromXDocument( XDocument xDocument )
		{
			XElement xProductCollection = xDocument.Element(NodesName.ProductCollection);
			var products = new List<LProduct>();

			if (xProductCollection != null)
			{
				IEnumerable<XElement> xProducts = xProductCollection.Elements(NodesName.Product);
				
				foreach(var element in xProducts)
				{
					products.Add(CreateProductFromXElement(element));
				}
			}

			return products;
		}

    	private LProduct CreateProductFromXElement(XElement element)
    	{
    	    LProduct product = null;

    	    if (!String.IsNullOrWhiteSpace(element.Element(NodesName.Article).Value))
    	        product = Repository.Data.Get<LProduct>()
                    .Where(x => x.Article.Equals(element.Element(NodesName.Article).Value, 
                        StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().Value;

    	    if (product == null)
    	        product = new LProduct {CreateDate = DateTime.Now};

    	    product.UpdateDate = DateTime.Now;
    	    product.LastSynchDate = DateTime.Now;

    	    PropertyInfo[] propertyInfos = product.GetType().GetProperties();

    		foreach(var info in propertyInfos)
    		{
				if (element.Element(info.Name) != null)
				{
					if (info.PropertyType == typeof(ProductProperty))
					{
						product.ProductProperty = CreateProductPropertyFromXElement(element.Element(NodesName.ProductProperty));
						continue;
					}

					if (info.PropertyType == typeof(Category))
					{
						product.Category = CreateCategoryFromXElement(element);
						continue;
					}

					if (info.PropertyType == typeof(Decimal))
					{
						Decimal result;
						Decimal.TryParse(element.Element(info.Name).Value, out result);
						info.SetValue(product, result, null);					
						continue;
					}

					if (info.PropertyType == typeof(Int32))
					{
						Int32 result;
						Int32.TryParse(element.Element(info.Name).Value, out result);
						info.SetValue(product, result, null);
						continue;
					}

					info.SetValue(product, element.Element(info.Name).Value, null);
				}
    		}

    		return product;
    	}

        private Category CreateCategoryFromXElement(XElement element)
    	{
    		Category category = null;

            if (element.Element(NodesName.Category) != null && !String.IsNullOrWhiteSpace(element.Element(NodesName.Category).Value) )
			{
				category = Repository.Data.Get<Category>().Where(x => x.Name.Equals(element.Element(NodesName.Category).Value,
					StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().Value;

				if(category == null)
				{
					category = new Category{Name = element.Element(NodesName.Category).Value};	
				}
			}

    		return category;
    	}

    	private ProductProperty CreateProductPropertyFromXElement(XElement element)
    	{
    		var productProperty = new ProductProperty();

			if (element.Element(NodesName.Property.Brand) != null)
				productProperty.Brend = element.Element(NodesName.Property.Brand).Value;
	
			if (element.Element(NodesName.Property.Capacity) != null)
				productProperty.Capacity = element.Element(NodesName.Property.Capacity).Value;

			if (element.Element(NodesName.Property.Country) != null)
				productProperty.Country = element.Element(NodesName.Property.Country).Value;

			if (element.Element(NodesName.Property.Weight) != null)
				productProperty.Weight = element.Element(NodesName.Property.Weight).Value;

    		return productProperty;
    	}
    }
}
