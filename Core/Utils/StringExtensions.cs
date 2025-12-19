using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Utils;
public static class StringExtensions
{
    public static string ToSeoFriendly(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        string result = text.Trim();

        result = result.ToLowerInvariant()
            .Replace('ç', 'c').Replace('Ç', 'C')
            .Replace('ğ', 'g').Replace('Ğ', 'G')
            .Replace('ı', 'i').Replace('İ', 'I')
            .Replace('ö', 'o').Replace('Ö', 'O')
            .Replace('ş', 's').Replace('Ş', 'S')
            .Replace('ü', 'u').Replace('Ü', 'U');

        result = result.Normalize(System.Text.NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in result)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        result = sb.ToString();

        result = Regex.Replace(result, @"[^a-z0-9\s-]", "");
        result = Regex.Replace(result, @"\s+", "-").Trim('-');
        result = Regex.Replace(result, @"-+", "-");

        return result;
    }
}