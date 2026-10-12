using Manisa.Web.Data;
using Manisa.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Manisa.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Lead> Leads => Set<Lead>();

    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<SiteSetting>(entity =>
        {
            entity.HasIndex(x => x.Key).IsUnique();
            entity.Property(x => x.Key).HasMaxLength(200);
            entity.Property(x => x.Value).HasMaxLength(2000);
        });

        builder.Entity<Lead>(entity =>
        {
            entity.Property(x => x.FullName).HasMaxLength(200);
            entity.Property(x => x.MobileNumber).HasMaxLength(30);
            entity.Property(x => x.Message).HasMaxLength(2000);
            entity.Property(x => x.InternalNotes).HasMaxLength(2000);
            entity.Property(x => x.SourcePage).HasMaxLength(200);
            entity.Property(x => x.Campaign).HasMaxLength(200);
            entity.Property(x => x.ProductInterest).HasMaxLength(200);
            entity.Property(x => x.City).HasMaxLength(100);
            entity.Property(x => x.QuantityRange).HasMaxLength(50);
            entity.HasIndex(x => x.CreatedAtUtc);
            entity.HasIndex(x => x.Status);
        });
    }
}
