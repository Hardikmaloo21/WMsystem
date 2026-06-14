// WMS.Application/Features/Attendance/DTOs/AttendanceDto.cs
namespace WMS.Application.Features.Attendance.DTOs;

public class AttendanceDto
{
    public int AttendanceId { get; set; }
    public int EmpId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public double? TotalHours { get; set; }
    public string? WorkMode { get; set; }
    public DateTime AttendanceDate { get; set; }
    public string Status => CheckOut.HasValue ? "Complete" : "In Progress";
}

public class CheckInDto
{
    public int EmpId { get; set; }
    public string WorkMode { get; set; } = "WFO";
}

public class CheckOutDto
{
    public int EmpId { get; set; }
}