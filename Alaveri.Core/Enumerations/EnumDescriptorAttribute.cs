namespace Alaveri.Core.Enumerations;

/// <summary>
/// Contains metadata about a single Enum value.
/// </summary>
/// <remarks>
/// Initializes an instance of the EnumDescriptorAttribute class using the specified parameters.
/// </remarks>
/// <param name="description">The human readable description of this Enum value.</param>
/// <param name="identifier">The string identifier used to match this enum value to another value, such as error or status text returned by a function.</param>
/// <param name="additionalData">Additional data associated with this enum value.</param>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class EnumDescriptorAttribute(string identifier = "", string description = "", object? additionalData = default) : Attribute
{
    /// <summary>
    /// A human readable description of this Enum value.
    /// </summary>
    public string Description { get; } = description;

    /// <summary>
    /// A string identifier used to match this enum value to another value, such as error or status text returned by a function.
    /// </summary>
    public string Identifier { get; } = identifier;

    /// <summary>
    /// additional data associated with this enum value.
    /// </summary>
    public object? AdditionalData { get; } = additionalData;
}

/// <summary>
/// Contains metadata about a single Enum value, linked to another Enum value.
/// </summary>
/// <remarks>
/// Initializes an instance of the EnumDescriptorAttribute class using the specified parameters.
/// </remarks>
/// <param name="linkedEnumValue">The linked enum value.</param>
/// <param name="description">The human readable description of this Enum value.</param>
/// <param name="identifier">The string identifier used to match this enum value to another value, such as error or status text returned by a function.</param>
/// <param name="additionalData">Additional data associated with this enum value.</param>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class LinkedEnumDescriptorAttribute<TLinkedEnum>(TLinkedEnum linkedEnumValue, string identifier = "", string description = "", object? additionalData = default)
    : EnumDescriptorAttribute(identifier, description, additionalData)
    where TLinkedEnum : struct, Enum
{
    /// <summary>
    /// A secondary Enum value that is linked to this Enum value.
    /// </summary>
    public TLinkedEnum LinkedEnumValue { get; set; } = linkedEnumValue;
}