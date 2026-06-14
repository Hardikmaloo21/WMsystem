// WMS.Application/Features/Leaves/DTOs/LeaveDto.cs
namespace WMS.Application.Features.Leaves.DTOs;

public class LeaveDto
{
    public int LeaveId { get; set; }
    public int EmpId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string LeaveType { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TotalDays => (int)(ToDate - FromDate).TotalDays + 1;
    public string Status { get; set; } = string.Empty;
    public DateTime AppliedOn { get; set; }
    public int? ApprovedBy { get; set; }
    public string? ApproverName { get; set; }
    public DateTime? ApprovedOn { get; set; }
}

public class ApplyLeaveDto
{
    public int EmpId { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class ApproveRejectLeaveDto
{
    public int LeaveId { get; set; }
    public int ApproverId { get; set; }
    public string Action { get; set; } = string.Empty; // "Approve" or "Reject"
    public string? Remarks { get; set; }
}