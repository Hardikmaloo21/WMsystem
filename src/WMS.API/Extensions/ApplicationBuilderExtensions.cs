namespace WMS.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseWmsMiddleware(this IApplicationBuilder app) => app;
}
