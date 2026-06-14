﻿using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Services;

public interface IAuditService
{

    Task LogAsync(string entityName, int recordId,
        string action, int userId,
        string? oldValues = null, string? newValues = null,
        CancellationToken ct = default);
}

public class AuditService : IAuditService
{
    private readonly WmsDbContext _context;

    public AuditService(WmsDbContext context) => _context = context;


    public async Task LogAsync(
        string entityName, int recordId, string action,
        int userId, string? oldValues = null,
        string? newValues = null, CancellationToken ct = default)
    {
        var audit = new AuditLog
        {
            EntityName = entityName,
            RecordId = recordId,
            Action = action,
            CreatedBy = userId,
            CreatedOn = DateTime.UtcNow,
            OldValues = oldValues,
            NewValues = newValues
        };
        await _context.AuditLogs.AddAsync(audit, ct);
        await _context.SaveChangesAsync(ct);
    }
}

