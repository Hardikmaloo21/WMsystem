// WMS.Application/Common/Behaviours/PerformanceBehaviour.cs
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WMS.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer = new();
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

    public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        _timer.Restart();
        var response = await next();
        _timer.Stop();

        if (_timer.ElapsedMilliseconds > 500)
            _logger.LogWarning(
                "WMS Long Running Request: {Name} ({Elapsed}ms) {@Request}",
                typeof(TRequest).Name, _timer.ElapsedMilliseconds, request);

        return response;
    }
}