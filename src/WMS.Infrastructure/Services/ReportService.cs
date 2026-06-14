namespace WMS.Infrastructure.Services;

public sealed class ReportService
{
    public Task<byte[]> GenerateAsync(string reportName, CancellationToken cancellationToken = default) => Task.FromResult(Array.Empty<byte>());
}
