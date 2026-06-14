// WMS.Domain/Interfaces/IUnitOfWork.cs
using WMS.Domain.Entities;
using WMS.Domain.Interfaces.Repositories;

namespace WMS.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository Employees { get; }
    IGenericRepository<Department> Departments { get; }
    IGenericRepository<Role> Roles { get; }
    IAttendanceRepository Attendances { get; }
    ILeaveRepository Leaves { get; }
    IGenericRepository<Announcement> Announcements { get; }
    IGenericRepository<Project> Projects { get; }
    IGenericRepository<Client> Clients { get; }
    IGenericRepository<EmployeeProjectAllocation> Allocations { get; }
    IGenericRepository<UserLogin> UserLogins { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
