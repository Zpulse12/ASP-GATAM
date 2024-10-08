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
                switch (ex)
                {
                    case FailedValidationException:
                        result.StatusCode = StatusCodes.Status400BadRequest;
                        if(ex is FailedValidationException validationException) //Casten naar te gebruiken variable.
                        {
                            if(validationException._validationFailures != null) //Mogelijks null. Dus checken!
                            {
                                result.Message = string.Join(",",validationException._validationFailures);
                            } else
                            {
                                result.Message = "Validation failures are null.";
                            }
                        } else
                        {
                            result.Message = ex.Message; //Defaulten naar message van exception
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

    internal class ErrorResponseObject
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public DateTime timeStamp = DateTime.UtcNow;
    }
}
