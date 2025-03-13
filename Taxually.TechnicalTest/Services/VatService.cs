using FluentValidation;
using System;
using System.Text;
using System.Xml.Serialization;
using Taxually.TechnicalTest.Interface;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Validators;
namespace Taxually.TechnicalTest.Services
{
    public class VatService : IVatHandler
    {
        private readonly IValidator<VatRegistrationRequest> _validator;
        private readonly ILogger<VatService> _logger;
        private readonly Dictionary<string, IVatRegistrationStrategy> _strategies;

        public VatService(IEnumerable<IVatRegistrationStrategy> strategies, IValidator<VatRegistrationRequest> validator, ILogger<VatService> logger)
        {
            _validator = validator;
            _logger = logger;
            _strategies = strategies.ToDictionary(s => s.CountryCode, s => s);
        }
        public async Task<bool> RegisterVatAsync(VatRegistrationRequest request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if (validation.IsValid)
            {
                _strategies.TryGetValue(request.Country, out var strategy);

                if (strategy != null)
                {
                    await strategy.RegisterAsync(request);
                }
                return true;
            }
            else
            {
                var errorMessage = validation.Errors.First().ErrorMessage;
                _logger.LogInformation(errorMessage);
            }

            return false;
        }
    }
}
