using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Infrastuctures.ModelHelpers.Order
{
	public class OutOfStockException : Exception
	{
		public OutOfStockException()
		{
		}

		public OutOfStockException(string name, string message) : base(String.Format(message, name))
		{
		}
	}
}
