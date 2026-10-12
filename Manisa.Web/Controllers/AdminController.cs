using Manisa.Web.Data;
using Manisa.Web.Models;
using Manisa.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manisa.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var model = new AdminDashboardViewModel
        {
            TotalLeads = _db.Leads.Count(x => !x.IsDeleted),
            NewLeads = _db.Leads.Count(x => !x.IsDeleted && x.Status == LeadStatus.New),
            FollowUpLeads = _db.Leads.Count(x => !x.IsDeleted && x.Status == LeadStatus.Contacted),
            ConvertedLeads = _db.Leads.Count(x => !x.IsDeleted && x.Status == LeadStatus.Converted),
            DemoLeads = _db.Leads.Count(x => x.IsDemo),
            RecentLeads = _db.Leads
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAtUtc)
                .Take(10)
                .ToList()
        };

        return View(model);
    }
}
