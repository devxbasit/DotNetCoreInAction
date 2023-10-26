using System.ComponentModel.DataAnnotations;

namespace ModelValidation.CustomAttributes
{
    public class ValueStartsWithAttribute : ValidationAttribute
    {
        private readonly string _startsWith;

        public ValueStartsWithAttribute(string startsWith)
        {
            _startsWith = startsWith;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue && !stringValue.StartsWith(_startsWith))
            {
                return new ValidationResult($"{validationContext.MemberName} does not starts with {_startsWith}");
            }

            return ValidationResult.Success;
        }
    }
}