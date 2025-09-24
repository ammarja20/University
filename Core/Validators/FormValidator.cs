using Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Core.Validators
{
    public static class FormValidator
    {
        public static void Validate<T>(T form)
        {
            if (form == null)
                throw new BusinessException("Form cannot be null.");

            var context = new ValidationContext(form);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(form, context, results, true))
            {
                var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
                throw new BusinessException(errors);
            }
        }
    }
}
