using System.Diagnostics;
using Manisa.Web.Data;
using Manisa.Web.Models;
using Manisa.Web.Services;
using Manisa.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Manisa.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var model = BuildDashboardModel();
        return View(model);
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Factory()
    {
        return View();
    }

    public IActionResult Products()
    {
        ViewBag.Categories = new[] { "لباس شب", "پوشاک زنانه", "مجموعه ویژه" };
        return View();
    }

    public IActionResult ProductDetail(string slug)
    {
        ViewBag.Slug = slug;
        return View();
    }

    public IActionResult Contact()
    {
        var settings = GetSettingsSnapshot();
        var model = new ContactPageViewModel
        {
            BrandName = settings["BrandName"],
            Address = settings["Address"],
            PrimaryPhone = settings["PrimaryPhone"],
            SecondaryPhone = settings["SecondaryPhone"],
            Landline = settings["Landline"],
            InstagramUrl = settings["InstagramUrl"],
            Message = TempData["LeadSuccess"] as string ?? string.Empty,
            LeadForm = new LeadFormViewModel
            {
                SourcePage = "/contact",
                Campaign = "website"
            }
        };

        return View(model);
    }

    public IActionResult RequestQuote()
    {
        return View("Contact", new ContactPageViewModel
        {
            LeadForm = new LeadFormViewModel { SourcePage = "/request-quote", Campaign = "quote" },
            BrandName = GetSettingsSnapshot()["BrandName"],
            Address = GetSettingsSnapshot()["Address"],
            PrimaryPhone = GetSettingsSnapshot()["PrimaryPhone"],
            SecondaryPhone = GetSettingsSnapshot()["SecondaryPhone"],
            Landline = GetSettingsSnapshot()["Landline"],
            InstagramUrl = GetSettingsSnapshot()["InstagramUrl"]
        });
    }

    public IActionResult Search(string q)
    {
        ViewBag.Query = q;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private DashboardViewModel BuildDashboardModel()
    {
        var settings = GetSettingsSnapshot();
        var recentLeads = _db.Leads
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(5)
            .ToList();

        return new DashboardViewModel
        {
            BrandName = settings["BrandName"],
            BrandDescription = settings["BrandDescription"],
            InstagramUrl = settings["InstagramUrl"],
            Address = settings["Address"],
            PrimaryPhone = settings["PrimaryPhone"],
            SecondaryPhone = settings["SecondaryPhone"],
            Landline = settings["Landline"],
            FoundedYear = settings["FoundedYear"],
            NewLeads = _db.Leads.Count(x => !x.IsDeleted && x.Status == LeadStatus.New),
            FollowUpLeads = _db.Leads.Count(x => !x.IsDeleted && x.Status == LeadStatus.Contacted),
            ConvertedLeads = _db.Leads.Count(x => !x.IsDeleted && x.Status == LeadStatus.Converted),
            DemoLeads = _db.Leads.Count(x => x.IsDemo),
            RecentLeads = recentLeads
        };
    }

    private Dictionary<string, string> GetSettingsSnapshot()
    {
        return _db.SiteSettings
            .AsNoTracking()
            .ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
    }
}
