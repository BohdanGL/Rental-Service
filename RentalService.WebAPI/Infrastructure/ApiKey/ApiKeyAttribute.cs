using Microsoft.AspNetCore.Mvc.Filters;
using RentalService.Application.Infrastructure.Exceptions;

namespace RentalService.WebAPI.Infrastructure.ApiKey
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiKeyAttribute : Attribute, IAuthorizationFilter
    {
        private const string API_KEY_HEADER_NAME = "XAPIKey";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var submittedApiKey = GetSubmittedApiKey(context.HttpContext);

            var apiKey = GetApiKey(context.HttpContext);

            if (!IsApiKeyValid(apiKey, submittedApiKey))
            {
                throw new UnauthorizedException("Unauthorized client");
            }
        }

        private static string GetSubmittedApiKey(HttpContext context)
        {
            return context.Request.Headers[API_KEY_HEADER_NAME];
        }

        private static string GetApiKey(HttpContext context)
        {
            var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

            return configuration.GetValue<string>($"XAPIKey");
        }

        private static bool IsApiKeyValid(string apiKey, string submittedApiKey)
        {
            return apiKey.Equals(submittedApiKey);
        }
    }
}
