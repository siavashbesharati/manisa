using Manisa.Web.Models;

namespace Manisa.Web.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalLeads { get; set; }
    public int NewLeads { get; set; }
    public int FollowUpLeads { get; set; }
    public int ConvertedLeads { get; set; }
    public int DemoLeads { get; set; }
    public IEnumerable<Lead> RecentLeads { get; set; } = []; 
}
