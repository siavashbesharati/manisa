using System.ComponentModel.DataAnnotations;
using Manisa.Web.Models;
using Manisa.Web.Services;

namespace Manisa.Tests;

public class ManisaBusinessLogicTests
{
    [Theory]
    [InlineData("۰۱۲۳۴۵۶۷۸۹", "0123456789")]
    [InlineData("۱۲۳۴۵۶۷۸۹۰", "1234567890")]
    public void NormalizePersianDigits_ReplacesPersianAndArabicDigits(string input, string expected)
    {
        var normalized = PhoneNumberHelper.NormalizePersianDigits(input);

        Assert.Equal(expected, normalized);
    }

    [Theory]
    [InlineData("09125379321")]
    [InlineData("۰۹۱۲۵۳۷۹۳۲۱")]
    [InlineData("+989125379321")]
    public void IsValidIranianMobile_AcceptsValidIranianNumbers(string value)
    {
        Assert.True(PhoneNumberHelper.IsValidIranianMobile(value));
    }

    [Theory]
    [InlineData("9125379321")]
    [InlineData("09000000000")]
    [InlineData("123456789")]
    public void IsValidIranianMobile_RejectsInvalidIranianNumbers(string value)
    {
        Assert.False(PhoneNumberHelper.IsValidIranianMobile(value));
    }

    [Theory]
    [InlineData("09125379321", "https://wa.me/989125379321")]
    [InlineData("۰۹۱۲۵۳۷۹۳۲۱", "https://wa.me/989125379321")]
    public void BuildWhatsAppLink_ProducesInternationalLink(string mobileNumber, string expected)
    {
        Assert.Equal(expected, PhoneNumberHelper.BuildWhatsAppLink(mobileNumber));
    }

    [Theory]
    [InlineData(LeadStatus.New, LeadStatus.Contacted, true)]
    [InlineData(LeadStatus.New, LeadStatus.Lost, true)]
    [InlineData(LeadStatus.Contacted, LeadStatus.QuoteSent, true)]
    [InlineData(LeadStatus.QuoteSent, LeadStatus.Converted, true)]
    [InlineData(LeadStatus.New, LeadStatus.Converted, false)]
    [InlineData(LeadStatus.Converted, LeadStatus.Lost, false)]
    public void CanTransitionTo_ValidatesExpectedStatusTransitions(LeadStatus current, LeadStatus next, bool expected)
    {
        Assert.Equal(expected, LeadStatusHelper.CanTransitionTo(current, next));
    }

    [Theory]
    [InlineData("لباس شب")]
    [InlineData("درباره ما")]
    [InlineData("مانیسا و فروشگاه")]
    public void SlugGenerator_CreatesReadableSlug(string title)
    {
        var slug = SlugGenerator.Generate(title);

        Assert.False(string.IsNullOrWhiteSpace(slug));
        Assert.True(slug.All(ch => char.IsLower(ch) || ch == '-' || char.IsDigit(ch)));
    }

    [Fact]
    public void SeoMetadataService_BuildsPageTitleAndDescription()
    {
        var title = SeoMetadataService.BuildPageTitle("درباره ما", "مانیسا");
        var description = SeoMetadataService.BuildMetaDescription("درباره ما", "مانیسا", "تولید لباس شب و پوشاک اختصاصی");

        Assert.Equal("درباره ما | مانیسا", title);
        Assert.Equal("تولید لباس شب و پوشاک اختصاصی", description);
    }

    [Fact]
    public void LeadModel_RequiresNameAndPhone()
    {
        var lead = new Lead { FullName = string.Empty, MobileNumber = string.Empty };
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(lead);

        var isValid = Validator.TryValidateObject(lead, context, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, x => x.MemberNames.Contains(nameof(Lead.FullName)) || x.MemberNames.Contains(nameof(Lead.MobileNumber)));
    }
}
