using Taxually.TechnicalTest.Models;
namespace Taxually.TechnicalTest.Interface
{
    public interface IVatHandler
    {
        Task<bool> RegisterVatAsync(VatRegistrationRequest request, CancellationToken cancellationToken);
    }
}
