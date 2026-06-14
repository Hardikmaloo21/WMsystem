// WMS.Domain/Entities/Announcement.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Entities;

[Table("Announcements")]
public class Announcement
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AnnouncementId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(CreatedByEmployee))]
    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation
    public Employee CreatedByEmployee { get; set; } = null!;
}