using Manisa.Web.Models;

namespace Manisa.Web.ViewModels;

public class DashboardViewModel
{
    public string BrandName { get; set; } = "مانیسا";
    public string BrandDescription { get; set; } = string.Empty;
    public string InstagramUrl { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PrimaryPhone { get; set; } = string.Empty;
    public string SecondaryPhone { get; set; } = string.Empty;
    public string Landline { get; set; } = string.Empty;
    public string FoundedYear { get; set; } = "2017";
    public int NewLeads { get; set; }
    public int FollowUpLeads { get; set; }
    public int ConvertedLeads { get; set; }
    public int DemoLeads { get; set; }
    public IEnumerable<Lead> RecentLeads { get; set; } = []; 
}
