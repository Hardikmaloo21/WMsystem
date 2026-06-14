// WMS.Domain/Entities/Department.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;

namespace WMS.Domain.Entities;

[Table("Departments")]
public class Department : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DepartmentId { get; set; }

    [Required]
    [MaxLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }

    // Navigation
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}