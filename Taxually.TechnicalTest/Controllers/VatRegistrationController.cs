using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Interface;
using Taxually.TechnicalTest.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController : ControllerBase
    {
        private readonly IVatHandler _VatHanlder;
        public VatRegistrationController(IVatHandler VatHandler)
        {
            _VatHanlder = VatHandler;
        }
        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request, CancellationToken cancellationToken)
        {
            bool result = await _VatHanlder.RegisterVatAsync(request, cancellationToken);

            return result == true? Ok() : NotFound("Country not supported");
        }
    }
}
