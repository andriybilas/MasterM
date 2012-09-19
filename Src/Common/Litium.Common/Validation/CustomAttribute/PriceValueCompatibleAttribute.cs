using System;
using System.ComponentModel.DataAnnotations;

namespace Litium.Common.Validation.CustomAttribute
{
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class PriceValueCompatibleAttribute : ValidationAttribute
	{
		private readonly String _errorMessage;

		#region .ctor
		public PriceValueCompatibleAttribute () { }
		public PriceValueCompatibleAttribute ( String errorMessage = null )
			: base (errorMessage)
		{
			_errorMessage = errorMessage;
		}
		#endregion

		public override bool IsValid ( object value )
		{
			if (value == null)
				return true;

			decimal price;

			if (!(decimal.TryParse (value.ToString (), out price)))
				return true;

			return price > 0m;
		}

		protected override ValidationResult IsValid ( object value, ValidationContext validationContext )
		{
			var result = new ValidationResult (_errorMessage);
			decimal price;

			if(!decimal.TryParse(value.ToString(), out price) || price <= 0m)
				return result;

			return ValidationResult.Success;
		}
	}
}
