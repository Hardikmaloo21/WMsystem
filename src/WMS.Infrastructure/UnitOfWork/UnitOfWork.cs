// WMS.Infrastructure/UnitOfWork/UnitOfWork.cs
using Microsoft.EntityFrameworkCore.Storage;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Domain.Interfaces.Repositories;
using WMS.Infrastructure.Persistence;
using WMS.Infrastructure.Repositories;

namespace WMS.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly WmsDbContext _context;
    private IDbContextTransaction? _transaction;

    private IEmployeeRepository? _employees;
    private IGenericRepository<Department>? _departments;
    private IGenericRepository<Role>? _roles;
    private IAttendanceRepository? _attendances;
    private ILeaveRepository? _leaves;
    private IGenericRepository<Announcement>? _announcements;
    private IGenericRepository<Project>? _projects;
    private IGenericRepository<Client>? _clients;
    private IGenericRepository<EmployeeProjectAllocation>? _allocations;
    private IGenericRepository<UserLogin>? _userLogins;

    public UnitOfWork(WmsDbContext context) => _context = context;

    public IEmployeeRepository Employees =>
        _employees ??= new EmployeeRepository(_context);
    public IGenericRepository<Department> Departments =>
        _departments ??= new GenericRepository<Department>(_context);
    public IGenericRepository<Role> Roles =>
        _roles ??= new GenericRepository<Role>(_context);
    public IAttendanceRepository Attendances =>
        _attendances ??= new AttendanceRepository(_context);
    public ILeaveRepository Leaves =>
        _leaves ??= new LeaveRepository(_context);
    public IGenericRepository<Announcement> Announcements =>
        _announcements ??= new GenericRepository<Announcement>(_context);
    public IGenericRepository<Project> Projects =>
        _projects ??= new GenericRepository<Project>(_context);
    public IGenericRepository<Client> Clients =>
        _clients ??= new GenericRepository<Client>(_context);
    public IGenericRepository<EmployeeProjectAllocation> Allocations =>
        _allocations ??= new GenericRepository<EmployeeProjectAllocation>(_context);
    public IGenericRepository<UserLogin> UserLogins =>
        _userLogins ??= new GenericRepository<UserLogin>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _transaction = await _context.Database.BeginTransactionAsync(ct);

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
            await _transaction.CommitAsync(ct);
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
            await _transaction.RollbackAsync(ct);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}