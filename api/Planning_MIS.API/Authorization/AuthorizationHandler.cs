using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Text.Json;

namespace Planning_MIS.API.Authorization
{
    public class AuthorizationHandler : IAuthorizationMiddlewareResultHandler
    {

        private readonly AuthorizationMiddlewareResultHandler _authHandler = new();
        private readonly ILogger<AuthorizationHandler> _logger;

        public AuthorizationHandler(ILogger<AuthorizationHandler> logger)
        {
            _logger = logger;

        }

        public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
        {


            if (authorizeResult.Forbidden)
            {
                _logger.LogWarning("Authorization failed: User does not have permission.");

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { message = "Forbidden: You do not have permission." });
                await context.Response.WriteAsync(result);
                return;
            }
            await _authHandler.HandleAsync(next, context, policy, authorizeResult);

        }
    }
}
