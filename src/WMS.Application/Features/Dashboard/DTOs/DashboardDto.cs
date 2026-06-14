
using WMS.Application.Features.Announcements.DTOs;

public class DashboardSummaryDto

{
    // KPI Cards
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int TotalDepartments { get; set; }
    public int TotalProjects { get; set; }
    public int ActiveProjects { get; set; }
    public int TotalClients { get; set; }
    public int TodayAttendanceCount { get; set; }
    public int PendingLeaves { get; set; }

    // Charts data
    public List<AttendanceTrendDto> AttendanceTrend { get; set; } = new();
    public List<LeaveTrendDto> LeaveTrend { get; set; } = new();
    public List<DepartmentDistributionDto> DepartmentDistribution { get; set; } = new();
    public List<ProjectStatusDto> ProjectStatusDistribution { get; set; } = new();
    public List<AnnouncementDto> RecentAnnouncements { get; set; } = new();
}

public class AttendanceTrendDto
{
    public string Date { get; set; } = string.Empty;
    public int Present { get; set; }
    public int Absent { get; set; }
    public int WFH { get; set; }
    public int WFO { get; set; }
}

public class LeaveTrendDto
{
    public string Month { get; set; } = string.Empty;
    public int Approved { get; set; }
    public int Pending { get; set; }
    public int Rejected { get; set; }
}

public class DepartmentDistributionDto
{
    public string DepartmentName { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
}

public class ProjectStatusDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}
