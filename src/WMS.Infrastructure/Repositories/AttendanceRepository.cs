// WMS.Infrastructure/Repositories/AttendanceRepository.cs
using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces.Repositories;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(WmsDbContext context) : base(context) { }

    public async Task<Attendance?> GetTodayAttendanceAsync(int empId, CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet.FirstOrDefaultAsync(
            a => a.EmpId == empId && a.AttendanceDate == today, ct);
    }

    public async Task<IReadOnlyList<Attendance>> GetMonthlyAsync(
        int empId, int year, int month, CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
            .Where(a => a.EmpId == empId &&
                        a.AttendanceDate.Year == year &&
                        a.AttendanceDate.Month == month)
            .OrderBy(a => a.AttendanceDate)
            .ToListAsync(ct);

    public async Task<bool> HasCheckedInTodayAsync(int empId, CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet.AnyAsync(
            a => a.EmpId == empId && a.AttendanceDate == today, ct);
    }


public async Task<IReadOnlyList<Attendance>> GetTodayWithEmployeeAsync(
    CancellationToken ct = default)
{
    var today = DateTime.UtcNow.Date;

    return await _dbSet
        .Include(x => x.Employee)
        .Where(x => x.AttendanceDate.Date == today)
        .OrderByDescending(x => x.CheckIn)
        .ToListAsync(ct);
}
}