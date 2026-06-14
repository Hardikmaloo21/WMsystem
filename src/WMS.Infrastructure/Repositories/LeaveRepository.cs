using Microsoft.EntityFrameworkCore;
using WMS.Domain.Common;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.Interfaces.Repositories;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public sealed class LeaveRepository(WmsDbContext dbContext) : GenericRepository<Leave>(dbContext), ILeaveRepository
{
    public Task<PagedResult<Leave>> GetByEmployeeAsync(int empId, int pageNumber, int pageSize, CancellationToken ct = default)
        => GetPagedAsync(
            pageNumber,
            pageSize,
            leave => leave.EmpId == empId,
            leaves => leaves.OrderByDescending(leave => leave.AppliedOn),
            ct);

    public async Task<IReadOnlyList<Leave>> GetPendingLeavesForManagerAsync(int managerId, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Where(leave => leave.Status == LeaveStatus.Pending.ToString())
            .OrderBy(leave => leave.AppliedOn)
            .ToListAsync(ct);

    public Task<int> GetPendingCountAsync(CancellationToken ct = default)
        => _dbSet.CountAsync(leave => leave.Status == LeaveStatus.Pending.ToString(), ct);

    public async Task<IReadOnlyList<Leave>> GetPendingAsync(
    CancellationToken ct = default)
{
    return await _dbSet
        .AsNoTracking()
        .Include(x => x.Employee)
        .Include(x => x.Approver)
        .Where(x => x.Status == "Pending")
        .OrderByDescending(x => x.AppliedOn)
        .ToListAsync(ct);
}
}
