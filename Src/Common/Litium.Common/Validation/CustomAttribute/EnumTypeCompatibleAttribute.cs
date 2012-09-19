using System;
using System.ComponentModel.DataAnnotations;

namespace Litium.Common.Validation.CustomAttribute
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class EnumTypeCompatibleAttribute : ValidationAttribute 
    {
         private readonly String _errorMessage;

        #region .ctor
        public EnumTypeCompatibleAttribute() { }
        public EnumTypeCompatibleAttribute(String errorMessage = null) : base(errorMessage)
        {
            _errorMessage = errorMessage;
        }
        #endregion

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            return Enum.IsDefined(value.GetType(), value); 
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = new ValidationResult(_errorMessage);
            if (!Enum.IsDefined(value.GetType(), value)) return result;
            return ValidationResult.Success;
        }
    }
}
