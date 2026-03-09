using System.Globalization;
using System.Text;

namespace MSWMS.Infrastructure.Helpers;

public static class StringProcessor
{
    public static string? NormalizeVariant(string? v)
    {
        if (string.IsNullOrWhiteSpace(v)) return null;

        // Убираем невидимые Unicode Format chars (U+202D/U+202C)
        var cleaned = string.Concat(
            v.EnumerateRunes()
                .Where(r => Rune.GetUnicodeCategory(r) != UnicodeCategory.Format)
                .Select(r => r.ToString())
        );

        cleaned = cleaned.Trim();
        return cleaned.Length == 0 ? null : cleaned.ToUpperInvariant();
    }
}