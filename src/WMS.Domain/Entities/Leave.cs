// WMS.Domain/Entities/Leave.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities;

[Table("Leaves")]
public class Leave
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LeaveId { get; set; }

    [Required]
    [ForeignKey(nameof(Employee))]
    public int EmpId { get; set; }

    [Required]
    [MaxLength(30)]
    public string LeaveType { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Reason { get; set; }

    [Required]
    public DateTime FromDate { get; set; }

    [Required]
    public DateTime ToDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = LeaveStatus.Pending.ToString();

    public DateTime AppliedOn { get; set; } = DateTime.UtcNow;

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    // Navigation
    public Employee Employee { get; set; } = null!;

    [ForeignKey(nameof(ApprovedBy))]
    public Employee? Approver { get; set; }
}