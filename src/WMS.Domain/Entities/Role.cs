// WMS.Domain/Entities/Role.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;

namespace WMS.Domain.Entities;

[Table("Roles")]
public class Role : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleId { get; set; }

    [Required]
    [MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Description { get; set; }

    // Navigation
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();
}