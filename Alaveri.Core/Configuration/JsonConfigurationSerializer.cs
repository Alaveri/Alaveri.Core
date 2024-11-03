using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Alaveri.Core.Configuration;

/// <summary>
/// Represents a configuration serializer that uses JSON serialization.
/// </summary>
/// <seealso cref="ConfigurationSerializer" />
/// <remarks>
/// Initializes a new instance of the <see cref="JsonConfigurationSerializer"/> class using the specified encoding.  Defaults to UTF8.
/// </remarks>
/// <param name="encoding">The encoding to use when serializing the configuration data.</param>
public class JsonConfigurationSerializer(Encoding? encoding = null) : TextConfigurationSerializer(encoding)
{
    /// <summary>
    /// Gets or sets the JSON serialization options.
    /// </summary>
    public JsonSerializerSettings Settings { get; set; } = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Include,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = { new StringEnumConverter(new CamelCaseNamingStrategy()) }
    };

    /// <summary>
    /// Gets the file extension for this serialization format.
    /// </summary>
    /// <value>The file extension for this serialization format.</value>
    public override string FileExtension => ".json";

    /// <summary>
    /// Serializes a configuration to JSON.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration to serialize.</typeparam>
    /// <param name="config">The configuration to serialize.</param>
    /// <returns>a byte array containing the serialized configuration data.</returns>
    public override byte[] SerializeConfiguration<TConfiguration>(TConfiguration config)
    {
        var json = JsonConvert.SerializeObject(config, Settings);
        return Encoding.GetBytes(json);
    }

    /// <summary>
    /// Serializes a configuration from JSON.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration to deserialize.</typeparam>
    /// <param name="configurationData">The configuration data to deserialize.</param>
    /// <returns>a deserialized Configuration object of type <typeparamref name="TConfiguration" />.</returns>
    public override TConfiguration DeserializeConfiguration<TConfiguration>(byte[] configurationData)
    {
        var json = Encoding.GetString(configurationData);
        return JsonConvert.DeserializeObject<TConfiguration>(json, Settings) ?? new TConfiguration();
    }
}
