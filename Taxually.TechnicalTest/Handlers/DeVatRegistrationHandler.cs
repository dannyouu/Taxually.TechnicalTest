using System.Xml.Serialization;
using Taxually.TechnicalTest.Exceptions.Types;
using Taxually.TechnicalTest.Interface;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers
{
    public class DeVatRegistrationHandler : IVatRegistrationStrategy
    {
        private readonly TaxuallyQueueClient _queueClient = new();
        public string CountryCode => "DE";

        public async Task RegisterAsync(VatRegistrationRequest request)
        {
            try
            {
                using var stringWriter = new StringWriter();
                var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
                serializer.Serialize(stringWriter, request);
                var xml = stringWriter.ToString();
                await _queueClient.EnqueueAsync("vat-registration-xml", xml);
            }
            catch (Exception ex)
            {

                throw new DeVatCustomException(ex.Message, System.Net.HttpStatusCode.NotFound);
            }
        }
    }
}
