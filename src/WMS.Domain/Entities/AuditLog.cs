// WMS.Domain/Entities/AuditLog.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Entities;

[Table("AuditLogs")]
public class AuditLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AuditId { get; set; }

    [Required]
    public string EntityName { get; set; } = string.Empty;

    public int RecordId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Action { get; set; } = string.Empty;

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }
}