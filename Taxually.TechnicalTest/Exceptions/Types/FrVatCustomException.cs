using System.Net;

namespace Taxually.TechnicalTest.Exceptions.Types
{
    public class FrVatCustomException : BaseCustomException
    {
        public FrVatCustomException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
