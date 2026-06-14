// WMS.Domain/Entities/EmployeeProjectAllocation.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Entities;

[Table("EmployeeProjectAllocations")]
public class EmployeeProjectAllocation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AllocationId { get; set; }

    [Required]
    [ForeignKey(nameof(Employee))]
    public int EmpId { get; set; }

    [Required]
    [ForeignKey(nameof(Project))]
    public int ProjectId { get; set; }

    [Required]
    public DateTime AssignedOn { get; set; }

    [Required]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string CreatedBy { get; set; } = string.Empty;

    public bool Status { get; set; } = true;

    [MaxLength(50)]
    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    // Navigation
    public Employee Employee { get; set; } = null!;
    public Project Project { get; set; } = null!;
}