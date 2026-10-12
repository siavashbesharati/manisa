namespace Manisa.Web.ViewModels;

public class ContactPageViewModel
{
    public LeadFormViewModel LeadForm { get; set; } = new();
    public string BrandName { get; set; } = "مانیسا";
    public string Address { get; set; } = string.Empty;
    public string PrimaryPhone { get; set; } = string.Empty;
    public string SecondaryPhone { get; set; } = string.Empty;
    public string Landline { get; set; } = string.Empty;
    public string InstagramUrl { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
