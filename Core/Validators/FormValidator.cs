using Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Core.Validators
{
    public static class FormValidator
    {
        public static void Validate<T>(T form) where T : class
        {
            if (form == null)
                throw new BusinessException(new List<string> { "Form cannot be null." });

            var context = new ValidationContext(form);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(form, context, results, validateAllProperties: true))
            {
                var errors = results
                    .Select(r => string.IsNullOrWhiteSpace(r.ErrorMessage) ? "Validation error." : r.ErrorMessage!)
                    .ToList();

                // throw list, not a single message
                throw new BusinessException(errors);
            }
        }
    }
}
