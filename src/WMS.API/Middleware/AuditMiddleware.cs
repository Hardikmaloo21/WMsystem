namespace WMS.API.Middleware;

public sealed class AuditMiddleware(RequestDelegate next)
{
    public Task InvokeAsync(HttpContext context) => next(context);
}
