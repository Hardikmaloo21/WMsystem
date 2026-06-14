// WMS.Infrastructure/Repositories/EmployeeRepository.cs
using Microsoft.EntityFrameworkCore;
using WMS.Domain.Common;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces.Repositories;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(WmsDbContext context) : base(context) { }

    public async Task<Employee?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
            .Include(e => e.Department)
            .Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.Email == email, ct);

    public async Task<PagedResult<Employee>> SearchAsync(
        string? search, int? departmentId, int? roleId,
        string? status, int pageNumber, int pageSize,
        CancellationToken ct = default)
    {
        var query = _dbSet.AsNoTracking()
            .Include(e => e.Department)
            .Include(e => e.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e =>
                e.FirstName.Contains(search) ||
                e.LastName.Contains(search) ||
                e.Email.Contains(search) ||
                e.PhoneNumber.Contains(search));

        if (departmentId.HasValue)
            query = query.Where(e => e.DepartmentId == departmentId.Value);

        if (roleId.HasValue)
            query = query.Where(e => e.RoleId == roleId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status == status);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Employee>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<Employee?> GetWithDetailsAsync(int employeeId, CancellationToken ct = default)
        => await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Role)
            .Include(e => e.Allocations).ThenInclude(a => a.Project)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
}