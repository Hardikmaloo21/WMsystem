// File: WMS.Application/Features/Allocations/DTOs/AllocationDto.cs
namespace WMS.Application.Features.Allocations.DTOs;

public record AllocationDto
{
    public int AllocationId { get; init; }
    public int EmpId { get; init; }
    public string EmployeeName { get; init; } = string.Empty;
    public string DepartmentName { get; init; } = string.Empty;
    public int ProjectId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public DateTime AssignedOn { get; init; }
    public bool Status { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreateDate { get; init; }
    public string? UpdatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
}

public record AssignEmployeeDto
{
    public int EmpId { get; init; }
    public int ProjectId { get; init; }
    public DateTime AssignedOn { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
}

public record UpdateAllocationDto
{
    public int AllocationId { get; init; }
    public bool Status { get; init; }
    public string UpdatedBy { get; init; } = string.Empty;
}