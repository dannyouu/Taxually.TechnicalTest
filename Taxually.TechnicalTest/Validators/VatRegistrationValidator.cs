using FluentValidation;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Validators
{
    public class VatRegistrationValidator : AbstractValidator<VatRegistrationRequest>
    {
        public VatRegistrationValidator() 
        {
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.");
            RuleFor(x => x.CompanyName).NotEmpty().WithMessage("Company Name is required.");
            RuleFor(x => x.CompanyId).NotEmpty().WithMessage("Company Id is required.");
        }
    }
}
