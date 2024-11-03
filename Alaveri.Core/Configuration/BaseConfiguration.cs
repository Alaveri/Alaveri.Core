using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Alaveri.Core.Configuration;

public delegate string GetConfigurationNameFunc();

/// <summary>
/// Represents a base class for holding and persisting application or user configuration information.
/// </summary>
public abstract class BaseConfiguration : IConfiguration
{
    /// <summary>
    /// Gets or sets the configuration filename.
    /// </summary>
    [JsonIgnore]
    [XmlIgnore]
    public string ConfigurationFilename { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serializer used to serialize the configuration.  Defaults to JSON with UTF8 encoding.
    /// </summary>The serializer.</value>
    [JsonIgnore]
    [XmlIgnore]
    public IConfigurationSerializer Serializer { get; set; } = new JsonConfigurationSerializer();

    /// <summary>
    /// The type of configuration.  Determines where the configuration will be stored, either in %programdata% or %appdata%.
    /// </summary>
    [JsonIgnore]
    [XmlIgnore]
    public virtual ConfigurationType ConfigurationType { get; set; } = ConfigurationType.User;

    /// <summary>
    /// Gets the path to the configuration file.  Defaults to %appdata%\[OrganizationName]\[ or %programdata%\[OrganizationName]\ depending on ConfigurationType.
    /// </summary>
    /// <param name="type">The configuration type.</param>
    /// <param name="organizationName">Name of the organization.</param>
    /// <param name="appName">Name of the application.</param>
    /// <returns>the path to the configuration file.</returns>
    public static string GetConfigurationPath(ConfigurationType type, string organizationName, string appName)
    {
        var path = type == ConfigurationType.User ?
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) :
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        return $"{Path.Combine(path, organizationName).TrimEnd(Path.DirectorySeparatorChar)}{Path.DirectorySeparatorChar}{appName}{Path.DirectorySeparatorChar}";
    }

    /// <summary>
    /// The full path and filename of the configuration file.  By default, the full path and filename is %appdata%\[Organization]\[ConfigurationName].[ConfigurationExtension], or
    /// %programdata%\[Organization]\[ConfigurationName].[ConfigurationExtension] depending on the ConfigurationType.
    /// </summary>
    public static string GetConfigurationFilename(ConfigurationType type, string organizationName, string appName, string extension, string? configurationName = null)
    {
        return Path.Combine(GetConfigurationPath(type, organizationName, appName), string.IsNullOrWhiteSpace(configurationName) ? appName : configurationName) + extension;
    }


    /// <summary>
    /// Loads a configuration from a stream.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of configuration to load.</typeparam>
    /// <param name="stream">The stream containing the configuration information.</param>
    /// <param name="size">The size of the configuration data.</param>
    /// <param name="serializer">The serializer used to serialize the configuration data.</param>
    /// <returns>A new configuration instance loaded from the stream, or a new empty configuration if an error occurred.</returns>
    public static TConfiguration LoadFromStream<TConfiguration>(Stream stream, int size, IConfigurationSerializer? serializer = null)
        where TConfiguration : class, IConfiguration, new()
    {
        serializer ??= new JsonConfigurationSerializer();
        try
        {
            var data = new byte[size];
            stream.Read(data, 0, size);
            return serializer.DeserializeConfiguration<TConfiguration>(data) ?? new TConfiguration();
        }
        catch
        {
            return new TConfiguration();
        }
    }

    /// <summary>
    /// Loads a configuration from a stream asynchronously.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of configuration to load.</typeparam>
    /// <param name="stream">The stream containing the configuration information.</param>
    /// <param name="size">The size of the configuration data.</param>
    /// <param name="serializer">The serializer used to deserialize configuration data.</param>
    /// <returns>A new configuration instance loaded from the stream, or a new empty configuration if an error occurred.</returns>
    public static async Task<TConfiguration> LoadFromStreamAsync<TConfiguration>(Stream stream, int size, IConfigurationSerializer? serializer = null)
        where TConfiguration : class, IConfiguration, new()
    {
        serializer ??= new JsonConfigurationSerializer();
        try
        {
            var data = new byte[size];
            await stream.ReadAsync(data.AsMemory(0, size));
            return await Task.Run(() => { return serializer.DeserializeConfiguration<TConfiguration>(data); });
        }
        catch
        {
            return new TConfiguration();
        }
    }

    /// <summary>
    /// Loads a configuration from a file.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of configuration to load.</typeparam>
    /// <param name="filename">The name of the file containing the configuration information.</param>
    /// <param name="serializer">The serializer used to deserialize the configuration data.</param>
    /// <returns>A new configuration instance loaded from the stream, or a new empty configuration if an error occurred.</returns>
    public static TConfiguration LoadFromFile<TConfiguration>(string filename, IConfigurationSerializer? serializer = null)
        where TConfiguration : class, IConfiguration, new()
    {
        serializer ??= new JsonConfigurationSerializer();
        if (!File.Exists(filename))
            return new TConfiguration();
        try
        {
            using var stream = new FileStream(filename, FileMode.Open);
            return LoadFromStream<TConfiguration>(stream, (int)stream.Length, serializer);
        }
        catch
        {
            return new TConfiguration();
        }
    }

    /// <summary>
    /// Loads a configuration from a file asynchronously.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of configuration to load.</typeparam>
    /// <param name="filename">The name of the file containing the configuration information.</param>
    /// <param name="serializer">The serializer used to deserialize the configuration data.</param>
    /// <returns>A new configuration instance loaded from the stream, or a new empty configuration if an error occurred.</returns>
    public static async Task<TConfiguration> LoadFromFileAsync<TConfiguration>(string filename, IConfigurationSerializer? serializer = null)
        where TConfiguration : class, IConfiguration, new()
    {
        serializer ??= new JsonConfigurationSerializer();
        if (!File.Exists(filename))
            return new TConfiguration();
        try
        {
            using var stream = new FileStream(filename, FileMode.Open);
            return await LoadFromStreamAsync<TConfiguration>(stream, (int)stream.Length, serializer);
        }
        catch
        {
            return new TConfiguration();
        }

    }

    /// <summary>
    /// Loads a configuration from a file with the default Configuration Filename asynchronously.
    /// 
    /// The configuration file name is built in the following way:
    /// 
    /// User configuration: %appdata%/[organizationName]/[appName]/[configurationName].[serializer.FileExtension].
    /// Program configuration: %programdata%/[organizationName]/[appName]/[configurationName].[serializer.FileExtension].
    /// 
    /// if configurationName is null or whitespace, [appName] is used for the file name.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of configuration to load.</typeparam>
    /// <param name="serializer">The serializer used to deserialize the configuration data.  If not provided, JSON serialization is used.</param>
    /// <param name="type">The type of configuration to load.</param>
    /// <param name="appName">The name of the application.</param>
    /// <param name="organizationName">The name of the organization.</param>
    /// <param name="configurationName">The name of the configuration file.</param>
    /// <returns>A new configuration instance loaded from the stream, or a new empty configuration if the configuration file doesn't exist.</returns>
    public static async Task<TConfiguration> LoadAsync<TConfiguration>(ConfigurationType type, string organizationName, string appName,
        IConfigurationSerializer? serializer = null, string? configurationName = null) where TConfiguration : class, IConfiguration, new()
    {
        serializer ??= new JsonConfigurationSerializer();
        var filename = GetConfigurationFilename(type, organizationName, appName, serializer.FileExtension, configurationName);
        var config = await LoadFromFileAsync<TConfiguration>(filename, serializer);
        config.ConfigurationFilename = filename;
        config.Serializer = serializer;
        return config;
    }

    /// <summary>
    /// Loads a configuration from a file with the default Configuration Filename synchronously.
    /// 
    /// The configuration file name is built in the following way:
    /// 
    /// User configuration: %appdata%/[organizationName]/[appName]/[configurationName].[serializer.FileExtension].
    /// Program configuration: %programdata%/[organizationName]/[appName]/[configurationName].[serializer.FileExtension].
    /// 
    /// if configurationName is null or whitespace, [appName] is used for the file name.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of configuration to load.</typeparam>
    /// <param name="serializer">The serializer used to deserialize the configuration data.  If not provided, JSON serialization is used.</param>
    /// <param name="type">The type of configuration to load.</param>
    /// <param name="appName">The name of the application.</param>
    /// <param name="organizationName">The name of the organization.</param>
    /// <param name="configurationName">The name of the configuration file.</param>
    /// <returns>A new configuration instance loaded from the stream, or a new empty configuration if the configuration file doesn't exist.</returns>
    public static TConfiguration Load<TConfiguration>(ConfigurationType type, string organizationName, string appName,
        IConfigurationSerializer? serializer = null, string? configurationName = null) where TConfiguration : class, IConfiguration, new()
    {
        serializer ??= new JsonConfigurationSerializer();
        var filename = GetConfigurationFilename(type, organizationName, appName, serializer.FileExtension, configurationName);
        var config = LoadFromFile<TConfiguration>(filename, serializer);
        config.ConfigurationFilename = filename;
        config.Serializer = serializer;
        return config;
    }


    /// <summary>
    /// Saves the configuration to a stream.
    /// </summary>
    /// <param name="stream">The stream to write.</param>
    public void SaveToStream(Stream stream)
    {
        var configData = Serializer.SerializeConfiguration(this);
        stream.Write(configData);
    }

    /// <summary>
    /// Saves the configuration to a stream asynchronously.
    /// </summary>
    /// <param name="stream">The stream to write.</param>
    public async Task SaveToStreamAsync(Stream stream)
    {
        var configData = await Task.Run(() => { return Serializer.SerializeConfiguration(this); });
        await stream.WriteAsync(configData);
    }


    /// <summary>
    /// Saves the configuration to a file.
    /// </summary>
    /// <param name="filename">The name of the file to write.</param>
    /// <exception cref="ArgumentNullException"
    public void SaveToFile(string filename)
    {
        var dir = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        ConfigurationFilename = filename;
        using var stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
        SaveToStream(stream);
    }

    /// <summary>
    /// Saves the configuration to a file asynchronously.
    /// </summary>
    /// <param name="stream">The name of the file to write.</param>
    public async Task SaveToFileAsync(string filename)
    {
        var dir = Path.GetDirectoryName(filename);
        if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        ConfigurationFilename = filename;
        using var stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
        await SaveToStreamAsync(stream);
    }

    /// <summary>
    /// Saves the configuration to a file using the default configuration filename.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of configuration to save.</typeparam>
    public void Save()
    {
        SaveToFile(ConfigurationFilename);
    }

    /// <summary>
    /// Saves the configuration to a file using the default configuration filename.
    /// </summary>
    public async Task SaveAsync()
    {
        await SaveToFileAsync(ConfigurationFilename);
    }
}

