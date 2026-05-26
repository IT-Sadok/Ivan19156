using System.Text.RegularExpressions;

namespace IoT.Domain.Enums;

public static class CommandTypeSlugExtensions
{
    private static readonly Regex _upperAfterLower = new("([a-z])([A-Z])", RegexOptions.Compiled);

    public static string ToSlug(this CommandTypeSlug slug)
        => _upperAfterLower.Replace(slug.ToString(), "$1_$2").ToLowerInvariant();
}
