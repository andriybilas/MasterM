using System;
using System.ComponentModel.DataAnnotations;

namespace Litium.Common.Validation.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class SQLDateValidAttribute : ValidationAttribute
    {
        private readonly String _errorMessage;

        #region .ctor
        public SQLDateValidAttribute() { }
        public SQLDateValidAttribute(String errorMessage = null) : base(errorMessage)
        {
            _errorMessage = errorMessage;
        } 
        #endregion

        private readonly DateTime _minSQLValue = new DateTime(1753, 1, 1);
        private readonly DateTime _maxSQLValue = new DateTime(9999, 12, 31);

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            DateTime date;
            DateTime.TryParse(value.ToString(), out date);
            return _minSQLValue <= date && date < _maxSQLValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = new ValidationResult(_errorMessage);
            if (!IsValid(value)) return result;
            return ValidationResult.Success;
        }
    }
}
