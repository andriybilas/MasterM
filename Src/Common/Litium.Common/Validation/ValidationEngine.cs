using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Litium.Common.Validation
{
    public class ValidationEngine
    {
        public static IEnumerable<ValidationConsequence> TryValidateEntity<T>(T entity)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(entity, null, null);
            Validator.TryValidateObject(entity, context, results, true);
            return results.Select(validationResult => new ValidationConsequence(typeof (T), typeof (T).FullName, validationResult.ErrorMessage)).ToList();
        }
    }
}
