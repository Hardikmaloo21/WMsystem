// WMS.Application/Common/Behaviours/LoggingBehaviour.cs
using MediatR;
using Microsoft.Extensions.Logging;

namespace WMS.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("WMS Request: {Name} {@Request}", requestName, request);

        var response = await next();

        _logger.LogInformation("WMS Response: {Name} {@Response}", requestName, response);
        return response;
    }
}