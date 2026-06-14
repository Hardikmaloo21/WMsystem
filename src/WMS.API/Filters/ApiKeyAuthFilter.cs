using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WMS.API.Filters;

public sealed class ApiKeyAuthFilter(IConfiguration configuration) : IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var expected = configuration["ApiKey"];
        if (string.IsNullOrWhiteSpace(expected)) return Task.CompletedTask;
        if (!context.HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var actual) || actual != expected)
        {
            context.Result = new UnauthorizedResult();
        }
        return Task.CompletedTask;
    }
}
