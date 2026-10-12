using System.Text;
using System.Text.RegularExpressions;

namespace Manisa.Web.Services;

public static class SlugGenerator
{
    public static string Generate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var builder = new StringBuilder();
        foreach (var ch in value.Trim())
        {
            if (ch is ' ' or '-')
            {
                builder.Append('-');
                continue;
            }

            var mapped = ch switch
            {
                'ا' => "a",
                'ب' => "b",
                'پ' => "p",
                'ت' => "t",
                'ث' => "s",
                'ج' => "j",
                'چ' => "ch",
                'ح' => "h",
                'خ' => "kh",
                'د' => "d",
                'ذ' => "z",
                'ر' => "r",
                'ز' => "z",
                'ژ' => "zh",
                'س' => "s",
                'ش' => "sh",
                'ص' => "s",
                'ض' => "z",
                'ط' => "t",
                'ظ' => "z",
                'ع' => "a",
                'غ' => "gh",
                'ف' => "f",
                'ق' => "q",
                'ک' => "k",
                'گ' => "g",
                'ل' => "l",
                'م' => "m",
                'ن' => "n",
                'و' => "v",
                'ه' => "h",
                'ی' => "y",
                'ي' => "y",
                'ئ' => "y",
                'أ' => "a",
                'ء' => "a",
                'ة' => "h",
                _ when char.IsLetterOrDigit(ch) => char.ToLowerInvariant(ch).ToString(),
                _ => string.Empty,
            };

            builder.Append(mapped);
        }

        var slug = Regex.Replace(builder.ToString(), "-+", "-");
        return slug.Trim('-').ToLowerInvariant();
    }
}
