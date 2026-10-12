using System.ComponentModel.DataAnnotations;

namespace Manisa.Web.Models;

public class Lead
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(30)]
    public string MobileNumber { get; set; } = string.Empty;

    [Required]
    public BusinessType BusinessType { get; set; }

    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [StringLength(200)]
    public string ProductInterest { get; set; } = string.Empty;

    [StringLength(50)]
    public string QuantityRange { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;

    [StringLength(200)]
    public string SourcePage { get; set; } = string.Empty;

    [StringLength(200)]
    public string Campaign { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public LeadStatus Status { get; set; } = LeadStatus.New;

    [StringLength(2000)]
    public string InternalNotes { get; set; } = string.Empty;

    public bool IsDemo { get; set; }

    public bool IsDeleted { get; set; }
}
