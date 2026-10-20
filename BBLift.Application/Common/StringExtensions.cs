using System.Text.RegularExpressions;

namespace BBLift.Application.Common;

public static class StringExtensions
{
    public static string ToSlug(this string text)
    {
        text = text.ToLowerInvariant();

        text = text.Replace("İ", "i")
            .Replace("ı", "i")
            .Replace("ü", "u")
            .Replace("ö", "o")
            .Replace("ğ", "g")
            .Replace("ş", "s")
            .Replace("ç", "c");

        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

        text = Regex.Replace(text, @"\s+", " ").Trim();

        text = text.Replace(" ", "-");

        text = Regex.Replace(text, @"-+", "-");

        return text;
    }

    public static string ToUserName(this string text)
    {
        text = text.ToLowerInvariant();

        text = text.Replace("İ", "i")
            .Replace("ı", "i")
            .Replace("ü", "u")
            .Replace("ö", "o")
            .Replace("ğ", "g")
            .Replace("ş", "s")
            .Replace("ç", "c");

        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

        text = Regex.Replace(text, @"\s+", " ").Trim();

        text = text.Replace(" ", ".");

        return text;
    }
}