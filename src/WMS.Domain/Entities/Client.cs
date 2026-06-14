// WMS.Domain/Entities/Client.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;

namespace WMS.Domain.Entities;

[Table("Clients")]
public class Client : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ClientId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;

    public string? ClientAddress { get; set; }

    [Column(TypeName = "numeric(10,0)")]
    public decimal? ClientPhoneNumber { get; set; }

    [MaxLength(20)]
    public string? ClientLocation { get; set; }

    public bool Status { get; set; } = true;

    // Navigation
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}