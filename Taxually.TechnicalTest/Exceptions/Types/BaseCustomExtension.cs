using System.Net;

namespace Taxually.TechnicalTest.Exceptions.Types
{
    public class BaseCustomException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; protected set; }

        public BaseCustomException(string message)
            : base(message)
        {
            HttpStatusCode = HttpStatusCode.BadRequest;
        }
    }
}
