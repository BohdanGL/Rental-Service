using Newtonsoft.Json;
using RentalService.Application.Infrastructure.Exceptions;
using System.Net;

namespace RentalService.WebAPI.Infrastructure.ExceptionsHandling
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment env;
        private readonly ILogger logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.env = env;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                ExceptionInfo info = GetExceptionInfo(ex, out int code);
                string infoJson = JsonConvert.SerializeObject(info);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = code;
                await context.Response.WriteAsync(infoJson);
            }
        }

        private ExceptionInfo GetExceptionInfo(Exception exception, out int statusCode)
        {
            ExceptionInfo info = new();

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    ValidationExceptionInfo valInfo = new();
                    valInfo.Message = validationException.Message;
                    valInfo.Errors = validationException.Failures;
                    info = valInfo;
                    break;
                case UnauthorizedException unauthorizedException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    info.Message = unauthorizedException.Message;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    if (!env.IsProduction())
                        info.Message = exception.Message;
                    else
                        info.Message = "Internal server error.";
                    logger.LogError(exception, exception.Message);
                    break;
            }

            if (!env.IsProduction())
                info.StackTrace = exception.StackTrace ?? string.Empty;

            return info;
        }
    }
}
