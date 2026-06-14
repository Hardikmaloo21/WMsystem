// WMS.Domain/Entities/Project.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities;

[Table("Projects")]
public class Project : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProjectId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ProjectName { get; set; } = string.Empty;

    [ForeignKey(nameof(Client))]
    public int? ClientId { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = ProjectStatus.Active.ToString();

    // Navigation
    public Client? Client { get; set; }
    public ICollection<EmployeeProjectAllocation> Allocations { get; set; } = new List<EmployeeProjectAllocation>();
}