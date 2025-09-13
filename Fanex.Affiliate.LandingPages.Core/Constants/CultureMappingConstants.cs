using System.Collections.Immutable;

namespace Fanex.Affiliate.LandingPages.Core.LandingPageConstants;

/// <summary>
/// Culture mapping constants for language codes
/// </summary>
public static class CultureMappingConstants
{
    public const string DefaultCulture = "en-US";
    
    /// <summary>
    /// Maps language codes to culture names
    /// </summary>
    private static readonly ImmutableDictionary<string, string> LanguageToCultureMapInternal = 
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "vn", "vi-VN" },
            { "th", "th-TH" },
            { "cn", "zh-CN" },
            { "te", "te-IN" },
            { "en", "en-US" }
        }.ToImmutableDictionary();

    /// <summary>
    /// Gets the culture name for a given language code
    /// </summary>
    /// <param name="languageCode">Language code (e.g., "vn", "th")</param>
    /// <returns>Culture name or default culture if not found</returns>
    public static string GetCultureName(string languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            return DefaultCulture;
        }

        return LanguageToCultureMapInternal.GetValueOrDefault(languageCode, DefaultCulture);
    }

    /// <summary>
    /// Checks if a language code is supported
    /// </summary>
    /// <param name="languageCode">Language code to check</param>
    /// <returns>True if supported, false otherwise</returns>
    public static bool IsLanguageSupported(string languageCode)
    {
        return !string.IsNullOrWhiteSpace(languageCode) && 
               LanguageToCultureMapInternal.ContainsKey(languageCode);
    }
}