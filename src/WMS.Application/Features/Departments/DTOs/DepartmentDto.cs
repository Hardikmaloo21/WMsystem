// WMS.Application/Features/Departments/DTOs/DepartmentDto.cs
namespace WMS.Application.Features.Departments.DTOs;

public class DepartmentDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int EmployeeCount { get; set; }
    public DateTime CreatedOn { get; set; }
}

public record CreateDepartmentDto(
    string DepartmentName,
    string? Description);

public record UpdateDepartmentDto(
    int DepartmentId,
    string DepartmentName,
    string? Description);