// WMS.Domain/Interfaces/Repositories/IEmployeeRepository.cs
using WMS.Domain.Common;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces.Repositories;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<Employee?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<PagedResult<Employee>> SearchAsync(string? search, int? departmentId, int? roleId,
        string? status, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<Employee?> GetWithDetailsAsync(int employeeId, CancellationToken ct = default);
}