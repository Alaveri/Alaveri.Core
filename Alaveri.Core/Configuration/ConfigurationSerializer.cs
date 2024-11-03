namespace Alaveri.Core.Configuration;

/// <summary>
/// Represents a serialization object for converting Configurations to various formats.
/// </summary>
public abstract class ConfigurationSerializer : IConfigurationSerializer
{
    /// <summary>
    /// Gets the file extension for this serialization format.
    /// </summary>
    /// <value>The file extension for this serialization format.</value>
    public abstract string FileExtension { get; }

    /// <summary>
    /// Serializes a configuration.
    /// </summary>
    /// <param name="config">The configuration to serialize.</param>
    /// <typeparam name="TConfiguration">The type of the configuration to serialize.</typeparam>
    /// <returns>a byte array containing the serialized configuration data.</returns>
    public abstract byte[] SerializeConfiguration<TConfiguration>(TConfiguration config) where TConfiguration : IConfiguration;

    /// <summary>
    /// Deserializes a configuration.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration to deserialize.</typeparam>
    /// <param name="configurationData">The configuration data to deserialize.</param>
    /// <returns>A deserialized Configuration object of type <typeparamref name="TConfiguration"/>.</returns>
    public abstract TConfiguration DeserializeConfiguration<TConfiguration>(byte[] configurationData) where TConfiguration : IConfiguration, new();
}
