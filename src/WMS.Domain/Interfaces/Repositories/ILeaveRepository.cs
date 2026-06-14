using WMS.Domain.Common;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces.Repositories;

public interface ILeaveRepository
    : IGenericRepository<Leave>
{
    Task<PagedResult<Leave>> GetByEmployeeAsync(
        int empId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default);

    Task<IReadOnlyList<Leave>> GetPendingAsync(
        CancellationToken ct = default);
}