using Taxually.TechnicalTest.Exceptions.Types;
using Taxually.TechnicalTest.Interface;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers
{
    public class UkVatRegistrationHandler : IVatRegistrationStrategy
    {
        private readonly TaxuallyHttpClient _httpClient = new();
        public string CountryCode => "GB";

        public async Task RegisterAsync(VatRegistrationRequest request)
        {
            try
            {
                await _httpClient.PostAsync("https://api.uktax.gov.uk", request);
            }
            catch (Exception ex)
            {

                throw new UkVatCustomException(ex.Message, System.Net.HttpStatusCode.NotFound);
            }
        }
    }
}
