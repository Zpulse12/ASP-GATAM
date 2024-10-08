using FluentValidation.Results;
using Gatam.Application.Exceptions;

namespace Gatam.WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) { 
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex) { 
                ErrorResponseObject result = new ErrorResponseObject();
                result.Message = ex.Message;
                result.TimeStamp = DateTime.UtcNow;
                result.Failures = null;
                switch (ex)
                {
                    case FailedValidationException validationException:
                        result.StatusCode = StatusCodes.Status400BadRequest;
                            if(validationException._validationFailures != null) //Mogelijks null. Dus checken!
                            {
                                result.Message = "Failed validations";
                                result.Failures = validationException._validationFailures;
                            } else
                            {
                                result.Message = "Validation failures are null.";
                            }
                        break;
                    default:
                        result.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                httpContext.Response.StatusCode = result.StatusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(result);
            }
        }
    }

    public class ErrorResponseObject
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<ValidationFailure>? Failures { get; set; }
    }
}
