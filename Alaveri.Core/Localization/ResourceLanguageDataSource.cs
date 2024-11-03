using System.Globalization;
using System.Resources;

namespace Alaveri.Core.Localization;

/// <summary>
/// Represents a data source for language translations that reads from a resource manager.
/// </summary>
public class ResourceLanguageDataSource : LanguageDataSource
{
    /// <summary>
    /// The resource manager used to read translations.
    /// </summary>
    public ResourceManager ResourceManager { get; }

    /// <param name="resourceManager">The resource manager used to read translations.</param>
    public ResourceLanguageDataSource(ResourceManager resourceManager)
    {
        ResourceManager = resourceManager;
        var code = resourceManager.GetString("FullCountryCode") ?? "en-US";
        Culture = CultureInfo.GetCultureInfo(code);
    }

    /// <summary>
    /// Reads a translation from the resource manager.
    /// </summary>
    /// <param name="id">The identifier of the translation.</param>
    /// <returns>The translation of the specified identifier.</returns>
    public override string ReadItem(string id)
    {
        return ResourceManager.GetString(id, Culture) ?? id;
    }

    /// <summary>
    /// Not implemented for this data source.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="item"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void WriteItem(string id, string item)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Reads all translations from the resource manager.
    /// </summary>
    /// <returns>A dictionary containing all IDs and translations.</returns>
    public override IDictionary<string, string> ReadAllItems()
    {
        return ResourceManager?.GetResourceSet(Culture, true, true)?
            .Cast<KeyValuePair<string, string>>()
            .ToDictionary(item => item.Key.ToString(), de => de.Value.ToString()) ?? [];
    }
}
