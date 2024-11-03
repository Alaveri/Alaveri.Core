namespace Alaveri.Core.Configuration;

public interface IConfigurationSerializer
{
    /// <summary>
    /// Gets the file extension for this serialization format.
    /// </summary>
    /// <value>The file extension for this serialization format.</value>
    string FileExtension { get; }

    /// <summary>
    /// Deserializes a configuration.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration to deserialize.</typeparam>
    /// <param name="configurationData">The configuration data to deserialize.</param>
    /// <returns>a deserialized Configuration object of type <typeparamref name="TConfiguration"/>.</returns>
    TConfiguration DeserializeConfiguration<TConfiguration>(byte[] configurationData) where TConfiguration : IConfiguration, new();

    /// <summary>
    /// Serializes a configuration.
    /// </summary>
    /// <param name="config">The configuration to serialize.</param>
    /// <typeparam name="TConfiguration">The type of the configuration to serialize.</typeparam>
    /// <returns>a byte array containing the serialized configuration data.</returns>
    byte[] SerializeConfiguration<TConfiguration>(TConfiguration config) where TConfiguration : IConfiguration;
}