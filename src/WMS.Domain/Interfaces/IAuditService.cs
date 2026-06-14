using WMS.Domain.Enums;

namespace WMS.Domain.Interfaces;

public interface IAuditService
{
    Task LogAsync(string entityName, int recordId, AuditAction action, int userId, string? oldValues = null, string? newValues = null, CancellationToken cancellationToken = default);
}
