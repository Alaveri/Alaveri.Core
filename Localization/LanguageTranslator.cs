namespace Alaveri.Localization;


/// <summary>
/// Delegate for translating a string given an identifier.
/// </summary>
/// <param name="identifier">The identifier to translate.</param>
/// <returns>The translated identifier.</returns>
public delegate string TranslateFunc(string identifier);


/// <summary>
/// Represents a base class for language translators.
/// </summary>
/// <remarks>
/// Initializes a new instance of the LanguageTranslator class using the specified data source.
/// </remarks>
/// <param name="dataSource">The source of the language data.</param>
public class LanguageTranslator(ILanguageDataSource dataSource) : ILanguageTranslator
{
    /// <summary>
    /// The data source used to look up translations.
    /// </summary>
    public ILanguageDataSource DataSource { get; set; } = dataSource;

    /// <summary>
    /// Translates a phrase given the specified key.
    /// </summary>
    /// <param name="key">The key of the translated string.</param>
    /// <returns>the translated phrase from the specified identifier.</returns>
    public virtual string Translate(string key)
    { 
        return DataSource.ReadItem(key);
    }

    /// <summary>
    /// Translates a phrase given the specified key.
    /// </summary>
    /// <param name="key">The key of the translated string.</param>
    /// <returns></returns>
    public virtual string this[string key] => Translate(key);

    /// <summary>
    /// Translates a phrase given the specified key.
    /// </summary>
    /// <param name="key">The key of the translated string.</param>
    /// <param name="args">Formatting arguments.</param>
    public virtual string TranslateFormat(string key, params object[] args)
    {
        return string.Format(DataSource.Culture, Translate(key), args);
    }
}
