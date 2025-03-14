using System.Net;
using System.Text.Json;
using System;
using Taxually.TechnicalTest.Exceptions.Types;
using Newtonsoft.Json;

namespace Taxually.TechnicalTest.Exceptions.Handler
{
    public class Error
    {
        public Guid ErrorId { get; set; } = Guid.NewGuid();
        public string Message { get; set; } = string.Empty;
        public string ExceptionType { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
        public string StackTrace { get; set; } = string.Empty;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                try
                {
                    await _next(context);
                }
                catch (BaseCustomException e)
                {
                    await HandleCustomExceptionAsync(context, e);
                }
                catch (Exception e)
                {
                    await HandleExceptionAsync(context, e);
                }
            }
        }
        private async Task HandleCustomExceptionAsync(HttpContext context, BaseCustomException e)
        {
            var error = new Error
            {
                ExceptionType = e.GetType().Name,
                Message = e.Message,
                StackTrace = e.StackTrace!,
                StatusCode = e.HttpStatusCode,
            };

            _logger.LogError(e, "Custom Exception thrown by middleware");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)error.StatusCode;
            await context.Response.WriteAsJsonAsync(error);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            var error = new Error
            {
                ExceptionType = e.GetType().Name,
                Message = e.Message,
                StackTrace = e.StackTrace!,
                StatusCode = e switch
                {
                    JsonSerializationException => HttpStatusCode.UnprocessableEntity,
                    NullReferenceException => HttpStatusCode.InternalServerError,
                    ArgumentException => HttpStatusCode.BadRequest,
                    UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                    NotSupportedException => HttpStatusCode.BadRequest,
                    FormatException => HttpStatusCode.InternalServerError,
                    _ => HttpStatusCode.BadRequest
                }
            };

            _logger.LogError(e, "Exception thrown by middleware");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)error.StatusCode;
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
