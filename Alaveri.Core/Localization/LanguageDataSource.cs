using Alaveri.Core.Data;
using System.Globalization;

namespace Alaveri.Core.Localization;

/// <summary>
/// Represents a base class for language data sources.
/// </summary>
public abstract class LanguageDataSource : DataSource<string, string>, ILanguageDataSource
{
    /// <summary>
    /// The culture information used during translations.
    /// </summary>
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Gets the translation for a given ID.
    /// </summary>
    /// <param name="id">The ID of the translated phrase.</param>
    /// <returns>A string containing the translated phrase.</returns>
    public string GetTranslation(string id)
    {
        return ReadItem(id);
    }
}
