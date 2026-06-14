// WMS.Domain/Entities/UserLogin.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Entities;

[Table("UserLogins")]
public class UserLogin
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(Role))]
    public int RoleId { get; set; }

    [ForeignKey(nameof(Employee))]
    public int? EmployeeId { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public DateTime? LastLogin { get; set; }

    // Navigation
    public Role Role { get; set; } = null!;
    public Employee? Employee { get; set; }
}