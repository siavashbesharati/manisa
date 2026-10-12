using System.ComponentModel.DataAnnotations;

namespace Manisa.Web.Models;

public class SiteSetting
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Key { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Value { get; set; } = string.Empty;
}
