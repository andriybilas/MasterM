using System;
using System.ComponentModel.DataAnnotations;

namespace Site.Infrastuctures.ModelHelpers.Order
{
	public class CartModel
	{
		public  Guid ProductId  { get; set; }

        [DataType(DataType.Currency)]
		public Decimal CampaignPrice { get; set; }

        [DataType(DataType.Currency)]
		public Decimal Price { get; set; }

		public int Count { get; set; }

		public String ProductName { get; set; }

        [DataType(DataType.Currency)]
		public Decimal Summa { get; set; }

		public CartModel CalculateSumma ()
		{
			Summa = CampaignPrice == 0m ? Price * Count : CampaignPrice * Count;
			return this;
		}
	}
}