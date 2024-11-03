namespace Alaveri.Core.Localization;

/// <summary>
/// Interface to an object that provides translations by reading from a data source.
/// </summary>
public interface ILanguageTranslator
{
    /// <summary>
    /// Translates a phrase given the specified key.
    /// </summary>
    /// <param name="key">The key of the translated string.</param>
    string this[string key] { get; }

    /// <summary>
    /// The data source used to look up translations.
    /// </summary>
    ILanguageDataSource DataSource { get; set; }

    /// <summary>
    /// Translates a phrase given the specified key.
    /// </summary>
    /// <param name="key">The identifier of the translated string.</param>
    /// <returns>the translated phrase from the specified key.</returns>
    string Translate(string key);
}