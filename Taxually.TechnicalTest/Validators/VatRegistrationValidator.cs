using FluentValidation;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Validators
{
    public class VatRegistrationValidator : AbstractValidator<VatRegistrationRequest>
    {
        public VatRegistrationValidator() 
        {
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required."); ;
        }
    }
}
