// WMS.Domain/Interfaces/Repositories/IAttendanceRepository.cs
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces.Repositories;

public interface IAttendanceRepository : IGenericRepository<Attendance>
{
    Task<Attendance?> GetTodayAttendanceAsync(int empId, CancellationToken ct = default);
    Task<IReadOnlyList<Attendance>> GetMonthlyAsync(int empId, int year, int month, CancellationToken ct = default);
    Task<bool> HasCheckedInTodayAsync(int empId, CancellationToken ct = default);

    Task<IReadOnlyList<Attendance>> GetTodayWithEmployeeAsync(
    CancellationToken ct = default);
}

