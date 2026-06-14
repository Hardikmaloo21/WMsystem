using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Announcements.DTOs;

namespace WMS.Application.Features.Dashboard.Queries;


public record GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>;


public class GetDashboardSummaryQueryHandler
    : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly IUnitOfWork _uow;

    public GetDashboardSummaryQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DashboardSummaryDto> Handle(
        GetDashboardSummaryQuery request, CancellationToken ct)
    {
        var today = DateTime.UtcNow.Date;
        var last7Days = today.AddDays(-6);
        var last6Months = today.AddMonths(-5);

        // ── KPI counts ───────────────────────────────────
        var totalEmployees = await _uow.Employees.Query().CountAsync(ct);

        var activeEmployees = await _uow.Employees.Query()
            .CountAsync(e => e.Status == "Active", ct);

        var totalDepartments = await _uow.Departments.Query().CountAsync(ct);

        var totalProjects = await _uow.Projects.Query().CountAsync(ct);

        var activeProjects = await _uow.Projects.Query()
            .CountAsync(p => p.Status == "Active", ct);

        var totalClients = await _uow.Clients.Query()
    .CountAsync(ct);

        var todayAttendance = await _uow.Attendances.Query()
            .CountAsync(a => a.AttendanceDate == today, ct);

        var pendingLeaves = await _uow.Leaves.Query()
            .CountAsync(l => l.Status == "Pending", ct);

        // ── Attendance trend (last 7 days) ────────────────
        var attendanceRaw = await _uow.Attendances.Query()
    .Where(a => a.AttendanceDate >= last7Days)
    .GroupBy(a => a.AttendanceDate)
    .Select(g => new
    {
        Date = g.Key,
        Present = g.Count(),
        WFH = g.Count(a => a.WorkMode == "WFH"),
        WFO = g.Count(a => a.WorkMode == "WFO")
    })
    .ToListAsync(ct);

var attendanceTrend = attendanceRaw
    .Select(x => new AttendanceTrendDto
    {
        Date = x.Date.ToString("MMM dd"),
        Present = x.Present,
        WFH = x.WFH,
        WFO = x.WFO,
        Absent = 0
    })
    .OrderBy(x => x.Date)
    .ToList();
        // ── Leave trend (last 6 months) ───────────────────
    var leaveTrendRaw = await _uow.Leaves.Query()
    .Where(l => l.AppliedOn >= last6Months)
    .GroupBy(l => new
    {
        l.AppliedOn.Year,
        l.AppliedOn.Month
    })
    .Select(g => new
    {
        g.Key.Year,
        g.Key.Month,
        Approved = g.Count(l => l.Status == "Approved"),
        Pending = g.Count(l => l.Status == "Pending"),
        Rejected = g.Count(l => l.Status == "Rejected")
    })
    .ToListAsync(ct);

var leaveTrend = leaveTrendRaw
    .OrderBy(x => x.Year)
    .ThenBy(x => x.Month)
    .Select(x => new LeaveTrendDto
    {
        Month = $"{x.Month}/{x.Year}",
        Approved = x.Approved,
        Pending = x.Pending,
        Rejected = x.Rejected
    })
    .ToList();
        // ── Department distribution ───────────────────────
        var deptDistribution = await _uow.Departments.Query()
            .Select(d => new DepartmentDistributionDto
            {
                DepartmentName = d.DepartmentName,
                EmployeeCount = d.Employees.Count(e => e.Status == "Active")
            })
            .OrderByDescending(d => d.EmployeeCount)
            .ToListAsync(ct);

        // ── Project status distribution ───────────────────
        var projectStatus = await _uow.Projects.Query()
            .GroupBy(p => p.Status)
            .Select(g => new ProjectStatusDto
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync(ct);

        // ── Recent active announcements ───────────────────
        var announcements = await _uow.Announcements.Query()
            .Include(a => a.CreatedByEmployee)
            .Where(a => a.IsActive)
            .OrderByDescending(a => a.CreatedOn)
            .Take(5)
            .Select(a => new AnnouncementDto
            {
                AnnouncementId = a.AnnouncementId,
                Title = a.Title,
                Message = a.Message,
                CreatedByName = a.CreatedByEmployee.FirstName
                              + " " + a.CreatedByEmployee.LastName,
                CreatedOn = a.CreatedOn,
                IsActive = a.IsActive
            })
            .ToListAsync(ct);

        return new DashboardSummaryDto
        {
            TotalEmployees = totalEmployees,
            ActiveEmployees = activeEmployees,
            TotalDepartments = totalDepartments,
            TotalProjects = totalProjects,
            ActiveProjects = activeProjects,
            TotalClients = totalClients,
            TodayAttendanceCount = todayAttendance,
            PendingLeaves = pendingLeaves,
            AttendanceTrend = attendanceTrend,
            LeaveTrend = leaveTrend,
            DepartmentDistribution = deptDistribution,
            ProjectStatusDistribution = projectStatus,
            RecentAnnouncements = announcements
        };
    }
}
