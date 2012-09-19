using System;
using System.ComponentModel.DataAnnotations;

namespace Litium.Common.Validation.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class GuidRequiredAttribute : ValidationAttribute
    {
        private readonly String _errorMessage;

        #region .ctor
        public GuidRequiredAttribute() { }
        public GuidRequiredAttribute(String errorMessage = null) : base(errorMessage)
        {
            _errorMessage = errorMessage;
        }
        #endregion

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            Guid outcome;
            Guid.TryParse(value.ToString(), out outcome);
            return outcome != Guid.Empty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = new ValidationResult(_errorMessage);
            if (!IsValid(value)) return result;
            return ValidationResult.Success;
        }
    }
}
