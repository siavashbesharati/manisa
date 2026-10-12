namespace Manisa.Web.Services;

public static class PhoneNumberHelper
{
    public static string NormalizePersianDigits(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var mappings = new Dictionary<char, char>
        {
            ['۰'] = '0', ['۱'] = '1', ['۲'] = '2', ['۳'] = '3', ['۴'] = '4', ['۵'] = '5', ['۶'] = '6', ['۷'] = '7', ['۸'] = '8', ['۹'] = '9',
            ['٠'] = '0', ['١'] = '1', ['٢'] = '2', ['٣'] = '3', ['٤'] = '4', ['٥'] = '5', ['٦'] = '6', ['٧'] = '7', ['٨'] = '8', ['٩'] = '9'
        };

        return new string(value.Select(ch => mappings.TryGetValue(ch, out var mapped) ? mapped : ch).ToArray());
    }

    public static bool IsValidIranianMobile(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalized = NormalizePersianDigits(value);
        normalized = normalized.Trim();
        normalized = normalized.Replace(" ", string.Empty).Replace("-", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);

        if (normalized.StartsWith("+", StringComparison.Ordinal))
        {
            normalized = normalized[1..];
        }

        if (normalized.StartsWith("09", StringComparison.Ordinal) && normalized.Length == 11 && normalized.All(char.IsDigit))
        {
            var trailing = normalized[2..];
            return !trailing.All(ch => ch == '0');
        }

        if (normalized.StartsWith("98", StringComparison.Ordinal) && normalized.Length == 12 && normalized.All(char.IsDigit))
        {
            var trailing = normalized[2..];
            return !trailing.All(ch => ch == '0');
        }

        return false;
    }

    public static string BuildWhatsAppLink(string? mobileNumber)
    {
        if (!IsValidIranianMobile(mobileNumber))
        {
            return string.Empty;
        }

        var digits = NormalizePersianDigits(mobileNumber).Replace(" ", string.Empty).Replace("-", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);
        if (digits.StartsWith("+", StringComparison.Ordinal))
        {
            digits = digits[1..];
        }

        if (digits.StartsWith("0", StringComparison.Ordinal))
        {
            digits = "98" + digits[1..];
        }

        if (digits.StartsWith("98", StringComparison.Ordinal) && digits.Length == 12)
        {
            return $"https://wa.me/{digits}";
        }

        return $"https://wa.me/98{digits}";
    }
}
