using System.ComponentModel.DataAnnotations;
using proj2.Models;

namespace proj2.Validations
{
    public class UniqueSSNAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyValue = value.ToString();
            var isUnique = IsUniqueSSN(propertyValue);

            if (!isUnique)
            {
                return new ValidationResult("SSN must be unique.");
            }

            return ValidationResult.Success;
        }

        private bool IsUniqueSSN(string ssn)
        {
            var _context = new HRContext();
            return !_context.Employees.Any(e => e.SSN == ssn);
        }
    }
}
