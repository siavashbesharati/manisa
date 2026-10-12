using System.ComponentModel.DataAnnotations;
using Manisa.Web.Models;

namespace Manisa.Web.ViewModels;

public class LeadFormViewModel
{
    [Required(ErrorMessage = "نام الزامی است.")]
    [StringLength(200, ErrorMessage = "نام نباید بیش از ۲۰۰ کاراکتر باشد.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "شماره موبایل الزامی است.")]
    [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "شماره موبایل معتبر نیست.")]
    public string MobileNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "نوع کسب‌وکار الزامی است.")]
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

    public string Honeypot { get; set; } = string.Empty;
}