using System.Text;
using Taxually.TechnicalTest.Exceptions.Types;
using Taxually.TechnicalTest.Interface;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers
{
    public class FrVatRegistrationHandler : IVatRegistrationStrategy
    {
        private readonly TaxuallyQueueClient _queueClient = new();
        public string CountryCode => "FR";

        public async Task RegisterAsync(VatRegistrationRequest request)
        {
            try
            {
                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("CompanyName,CompanyId");
                csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
                var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
                await _queueClient.EnqueueAsync("vat-registration-csv", csv);
            }
            catch (Exception ex)
            {

                throw new FrVatCustomException(ex.Message, System.Net.HttpStatusCode.NotFound);
            }
        }
    }

}
