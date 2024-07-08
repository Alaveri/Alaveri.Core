namespace Alaveri.Configuration;

public interface IConfiguration
{
    /// <summary>
    /// Gets or sets the configuration filename.
    /// </summary>
    /// <value>
    /// The configuration filename.
    /// </value>
    string ConfigurationFilename { get; set; }

    /// <summary>
    /// Gets or sets the serializer used to serialize the configuration.  Defaults to JSON with UTF8 encoding.
    /// </summary>The serializer.</value>
    IConfigurationSerializer Serializer { get; set; }

    /// <summary>
    /// The type of configuration.  Determines where the configuration will be stored, either in %programdata% or %appdata%.
    /// </summary>
    ConfigurationType ConfigurationType { get; set; }

    /// <summary>
    /// Saves the configuration to a stream.
    /// </summary>
    /// <param name="stream">The stream to write.</param>
    void SaveToStream(Stream stream);

    /// <summary>
    /// Saves the configuration to a stream asynchronously.
    /// </summary>
    /// <param name="stream">The stream to write.</param>
    Task SaveToStreamAsync(Stream stream);

    /// <summary>
    /// Saves the configuration to a file.
    /// </summary>
    /// <param name="filename">The name of the file to write.</param>
    /// <exception cref="ArgumentNullException"
    void SaveToFile(string filename);

    /// <summary>
    /// Saves the configuration to a file asynchronously.
    /// </summary>
    /// <param name="stream">The name of the file to write.</param>
    Task SaveToFileAsync(string filename);

    /// <summary>
    /// Saves the configuration to a file using the default configuration filename.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of configuration to save.</typeparam>
    void Save();

    /// <summary>
    /// Saves the configuration to a file using the default configuration filename.
    /// </summary>
    Task SaveAsync();
}