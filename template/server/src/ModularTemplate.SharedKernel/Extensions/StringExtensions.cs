namespace ModularTemplate.SharedKernel.Extensions;

public static class StringExtensions
{
    public static string? TrimToNull(this string? value)
    {
        string? trimmed = value?.Trim();

        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }

    public static string TrimRequired(this string value, string parameterName)
    {
        ArgumentNullException.ThrowIfNull(value, parameterName);

        string trimmed = value.Trim();
        if (trimmed.Length == 0)
        {
            throw new ArgumentException("Value must not be empty or whitespace.", parameterName);
        }

        return trimmed;
    }

    public static string ToSafeLocalReturnUrl(this string? value, string fallback = "/")
    {
        if (string.IsNullOrWhiteSpace(value)
            || !Uri.IsWellFormedUriString(value, UriKind.Relative)
            || value.StartsWith("//", StringComparison.Ordinal))
        {
            return fallback;
        }

        return value;
    }
}
