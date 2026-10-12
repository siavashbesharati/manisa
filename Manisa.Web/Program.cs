using Manisa.Web.Data;
using Manisa.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var appDataPath = Environment.GetEnvironmentVariable("MANISA_DB_PATH")
    ?? builder.Configuration["Database:Path"]
    ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manisa", "manisa.db");

Directory.CreateDirectory(Path.GetDirectoryName(appDataPath) ?? Directory.GetCurrentDirectory());
var connectionString = $"Data Source={appDataPath};Cache=Shared;Foreign Keys=True;Default Timeout=5;";

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    await dbContext.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL;");

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    if (!await dbContext.SiteSettings.AnyAsync())
    {
        dbContext.SiteSettings.AddRange(
            new SiteSetting { Key = "BrandName", Value = "مانیسا" },
            new SiteSetting { Key = "InstagramUrl", Value = "https://www.instagram.com/manisaposh" },
            new SiteSetting { Key = "FoundedYear", Value = "2017" },
            new SiteSetting { Key = "Address", Value = "تهران، میدان خراسان، خیابان ۱۷ شهریور، خیابان رضایی، کوچه صادقی، پلاک ۳۴" },
            new SiteSetting { Key = "PrimaryPhone", Value = "09903717036" },
            new SiteSetting { Key = "SecondaryPhone", Value = "09125379321" },
            new SiteSetting { Key = "Landline", Value = "021-66475033" },
            new SiteSetting { Key = "BrandDescription", Value = "تولید و تامین پوشاک شب و لباس‌های اختصاصی برای بوتیک‌ها، فروشگاه‌های زنجیره‌ای، فروشندگان اینستاگرامی و فروشگاه‌های آنلاین." },
            new SiteSetting { Key = "FactorySummary", Value = "کارخانه‌ی اختصاصی مانیسا از ابتدای فعالیت در بازار بزرگ سراى ملی مشغول تولید و تنظیم سفارش‌های خاص برندها و فروشگاه‌ها بوده است." },
            new SiteSetting { Key = "MetaTitle", Value = "مانیسا | تامین‌کننده لباس شب و پوشاک اختصاصی" },
            new SiteSetting { Key = "MetaDescription", Value = "مانیسا تولیدکننده لباس شب و پوشاک اختصاصی برای بوتیک‌ها، فروشگاه‌ها و برندهای مد. سفارش‌گذاری و هماهنگی با کارخانه به‌صورت حرفه‌ای انجام می‌شود." }
        );

        await dbContext.SaveChangesAsync();
    }

    if (!await dbContext.Leads.AnyAsync())
    {
        dbContext.Leads.AddRange(
            new Lead
            {
                FullName = "نمونه فروشگاه",
                MobileNumber = "09120000000",
                BusinessType = BusinessType.Boutique,
                City = "تهران",
                ProductInterest = "لباس شب",
                QuantityRange = "تا ۵۰ عدد",
                Message = "این رکورد نمونه است و برای نمایش داشبورد ایجاد شده است.",
                SourcePage = "/contact",
                Campaign = "demo",
                CreatedAtUtc = DateTime.UtcNow.AddDays(-3),
                Status = LeadStatus.New,
                IsDemo = true,
                InternalNotes = "رکورد آزمایشی، به‌صورت واضح مشخص شده و برای حذف آماده است."
            },
            new Lead
            {
                FullName = "نمونه مزون",
                MobileNumber = "09121111111",
                BusinessType = BusinessType.Mazon,
                City = "شیراز",
                ProductInterest = "پوشاک غیررسمی",
                QuantityRange = "۵۰ تا ۱۰۰ عدد",
                Message = "نمونه‌ی دوم برای تست داشبورد و فرآیند پیگیری.",
                SourcePage = "/products",
                Campaign = "demo",
                CreatedAtUtc = DateTime.UtcNow.AddDays(-10),
                Status = LeadStatus.Contacted,
                IsDemo = true,
                InternalNotes = "رکورد آزمایشی بنابراین در محیط تولید حذف شود."
            }
        );

        await dbContext.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
