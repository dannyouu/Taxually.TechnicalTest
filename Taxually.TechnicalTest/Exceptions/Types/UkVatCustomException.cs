using System.Net;

namespace Taxually.TechnicalTest.Exceptions.Types
{
    public class UkVatCustomException : BaseCustomException
    {
        public UkVatCustomException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
