using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Litium.Common.Validation.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class ReferenceRequaredAttribute : ValidationAttribute
    {
        private readonly String _errorMessage;

        #region .ctor
        public ReferenceRequaredAttribute() { }
        public ReferenceRequaredAttribute(String errorMessage = null)
            : base(errorMessage)
        {
            _errorMessage = errorMessage;
        }
        #endregion
        
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            return true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = new ValidationResult(_errorMessage);
            if (!IsValid(value)) return result;
            return ValidationResult.Success;
        }
    }
}
