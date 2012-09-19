using System;
using System.ComponentModel.DataAnnotations;

namespace Litium.Common.Validation.CustomAttribute
{
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class CountValueCompatibleAttribute : ValidationAttribute
	{
		private readonly String _errorMessage;

		#region .ctor
		public CountValueCompatibleAttribute () { }
		public CountValueCompatibleAttribute ( String errorMessage = null )
			: base (errorMessage)
		{
			_errorMessage = errorMessage;
		}
		#endregion

		public override bool IsValid ( object value )
		{
			if (value == null)
				return true;

			int count;

			if (!(int.TryParse (value.ToString (), out count)))
				return true;

			return count >= 0m;
		}

		protected override ValidationResult IsValid ( object value, ValidationContext validationContext )
		{
			var result = new ValidationResult (_errorMessage);
			int count;

			if(!int.TryParse(value.ToString(), out count) || count < 0)
				return result;

			return ValidationResult.Success;
		}
	}
}
