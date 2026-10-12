namespace Manisa.Web.Services;

public static class SeoMetadataService
{
    public static string BuildPageTitle(string title, string siteName)
    {
        return string.IsNullOrWhiteSpace(title) ? siteName : $"{title} | {siteName}";
    }

    public static string BuildMetaDescription(string title, string siteName, string fallbackDescription)
    {
        if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(siteName) && title.Contains(siteName, StringComparison.OrdinalIgnoreCase))
        {
            return fallbackDescription;
        }

        return string.IsNullOrWhiteSpace(fallbackDescription)
            ? $"{siteName} — تولید و تامین پوشاک شب و لباس‌های اختصاصی برای فروشگاه‌ها و بوتیک‌ها."
            : fallbackDescription;
    }
}
