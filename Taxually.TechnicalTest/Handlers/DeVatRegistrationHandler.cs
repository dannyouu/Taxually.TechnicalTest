using System.Xml.Serialization;
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
            using var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
            serializer.Serialize(stringWriter, request);
            var xml = stringWriter.ToString();
            await _queueClient.EnqueueAsync("vat-registration-xml", xml);
        }
    }
}
