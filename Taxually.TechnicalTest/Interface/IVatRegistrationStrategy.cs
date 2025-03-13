using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Interface
{
    public interface IVatRegistrationStrategy
    {
        string CountryCode { get; }
        Task RegisterAsync(VatRegistrationRequest request);
    }
}
