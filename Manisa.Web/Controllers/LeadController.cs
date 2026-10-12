using Manisa.Web.Data;
using Manisa.Web.Models;
using Manisa.Web.Services;
using Manisa.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Manisa.Web.Controllers;

public class LeadController : Controller
{
    private readonly ApplicationDbContext _db;

    public LeadController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeadFormViewModel model)
    {
        if (!string.IsNullOrWhiteSpace(model.Honeypot))
        {
            return BadRequest();
        }

        if (!PhoneNumberHelper.IsValidIranianMobile(model.MobileNumber))
        {
            ModelState.AddModelError(nameof(model.MobileNumber), "شماره موبایل باید یک شماره معتبر ایرانی باشد.");
        }

        if (string.IsNullOrWhiteSpace(model.Message) && string.IsNullOrWhiteSpace(model.ProductInterest))
        {
            ModelState.AddModelError(nameof(model.ProductInterest), "لطفاً نوع محصول یا نیاز خود را بنویسید.");
        }

        if (!ModelState.IsValid)
        {
            var settings = _db.SiteSettings.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
            return View("~/Views/Home/Contact.cshtml", new ContactPageViewModel
            {
                BrandName = settings.GetValueOrDefault("BrandName", "مانیسا"),
                Address = settings.GetValueOrDefault("Address", string.Empty),
                PrimaryPhone = settings.GetValueOrDefault("PrimaryPhone", string.Empty),
                SecondaryPhone = settings.GetValueOrDefault("SecondaryPhone", string.Empty),
                Landline = settings.GetValueOrDefault("Landline", string.Empty),
                InstagramUrl = settings.GetValueOrDefault("InstagramUrl", string.Empty),
                LeadForm = model
            });
        }

        var lead = new Lead
        {
            FullName = model.FullName.Trim(),
            MobileNumber = PhoneNumberHelper.NormalizePersianDigits(model.MobileNumber),
            BusinessType = model.BusinessType,
            City = model.City.Trim(),
            ProductInterest = model.ProductInterest.Trim(),
            QuantityRange = model.QuantityRange.Trim(),
            Message = model.Message.Trim(),
            SourcePage = string.IsNullOrWhiteSpace(model.SourcePage) ? "/contact" : model.SourcePage,
            Campaign = model.Campaign.Trim(),
            Status = LeadStatus.New,
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Leads.Add(lead);
        await _db.SaveChangesAsync();
        TempData["LeadSuccess"] = "درخواست شما با موفقیت ثبت شد. تیم مانیسا در کوتاه‌ترین زمان با شما تماس خواهد گرفت.";

        return RedirectToAction("Contact", "Home");
    }
}