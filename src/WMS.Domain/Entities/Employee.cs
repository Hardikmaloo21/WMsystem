// WMS.Domain/Entities/Employee.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities;

[Table("Employees")]
public class Employee : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(15)]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateTime DOB { get; set; }

    [Required]
    public DateTime DOJ { get; set; }

    [ForeignKey(nameof(Department))]
    public int DepartmentId { get; set; }

    [ForeignKey(nameof(Role))]
    public int RoleId { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = EmployeeStatus.Active.ToString();

    // Navigation
    public Department Department { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
    public ICollection<EmployeeProjectAllocation> Allocations { get; set; } = new List<EmployeeProjectAllocation>();
    public UserLogin? UserLogin { get; set; }
    public ICollection<Announcement> CreatedAnnouncements { get; set; } = new List<Announcement>();

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}