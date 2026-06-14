// WMS.Domain/Entities/Attendance.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities;

[Table("Attendances")]
public class Attendance
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AttendanceId { get; set; }

    [Required]
    [ForeignKey(nameof(Employee))]
    public int EmpId { get; set; }

    [Required]
    public DateTime CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    [Column(TypeName = "float")]
    public double? TotalHours { get; set; }

    [MaxLength(20)]
    public string? WorkMode { get; set; }

    [Required]
    public DateTime AttendanceDate { get; set; }

    // Navigation
    public Employee Employee { get; set; } = null!;
}