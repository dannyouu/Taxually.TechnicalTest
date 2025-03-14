using System.Net;

namespace Taxually.TechnicalTest.Exceptions.Types
{
    public class DeVatCustomException : BaseCustomException
    {
        public DeVatCustomException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
