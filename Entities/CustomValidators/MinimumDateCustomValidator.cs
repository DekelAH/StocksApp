using System.ComponentModel.DataAnnotations;

namespace Entities.CustomValidators
{
    public sealed class MinimumDateCustomValidator : ValidationAttribute
    {
        #region Properties

        public int MinimumYear { get; set; } = 2000;
        public string DefaultErrorMessage { get; set; } = "Year should be less than {0}";
        #endregion

        #region Ctors

        public MinimumDateCustomValidator() { }
        public MinimumDateCustomValidator(int minimumYear)
        {
            MinimumYear = minimumYear;
        }

        #endregion

        #region Methods

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime date = (DateTime)value;
                if (date.Year < MinimumYear)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, MinimumYear));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            return null;
        }

        #endregion
    }
}
