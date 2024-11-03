namespace Alaveri.Core.Enumerations;
/// <summary>
/// Extension methods for enum related functions.
/// </summary>
public static class EnumerationExtensions
{
    /// <summary>
    /// Retrieves the attribute of the specified type and enum value.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute to find.</typeparam>
    /// <param name="value">The enum value to use.</param>
    /// <returns>The attribute of the specified type and enum value, or null if none found.</returns>
    public static TAttribute? GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
    {
        return EnumHelper.GetEnumAttribute<TAttribute>(value);
    }

    /// <summary>
    /// Returns the enum descriptor attribute of this enum value, if it exists.
    /// </summary>
    /// <param name="value">The enumeration value to find.</param>
    /// <returns>The EnumDescriptor attribute, or null if none found.</returns>
    public static EnumDescriptorAttribute? GetDescriptor(this Enum value)
    {
        return EnumHelper.GetDescriptorAttribute(value);
    }

    /// <summary>
    /// If an Enum is decorated with an EnumDescriptor attribute, this will return the value of the Description.
    /// </summary>
    /// <param name="value">The enum value to use.</param>
    /// <returns>The value of the Description or null if not found.</returns>
    public static string GetDescription(this Enum value)
    {
        return EnumHelper.GetDescription(value) ?? string.Empty;
    }

    /// <summary>
    /// If an Enum is decorated with an EnumDescriptor attribute, this will return the value of the Identifier.
    /// </summary>
    /// <param name="value">The enum value to use.</param>
    /// <returns>The value of the Description or null if not found.</returns>
    public static string GetIdentifier(this Enum value)
    {
        return EnumHelper.GetIdentifier(value) ?? string.Empty;
    }

    /// <summary>
    /// If an Enum is decorated with an EnumDescriptor attribute, this will return the value of the AdditionalData.
    /// </summary>
    /// <param name="value">The enum value to use.</param>
    /// <returns>The value of the AdditionalData or null if not found.</returns>
    public static object? GetAdditionalData(this Enum value)
    {
        return EnumHelper.GetAdditionalData(value);
    }

    /// <summary>
    /// If an Enum is decorated with an EnumDescriptor attribute, this will return the value of the LinkedEnumValue.
    /// </summary>
    /// <param name="enumValue">The enum value to use.</param>
    /// <returns>The value of the LinkedEnumValue or null if not found.</returns>
    public static TEnum GetLinkedEnum<TEnum>(this Enum enumValue) where TEnum : struct, Enum
    {
        return EnumHelper.GetLinkedEnum<TEnum>(enumValue);
    }

    /// <summary>
    /// Returns true if the specified enum has the specified attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute.</typeparam>
    /// <param name="enumValue">The value of the enum to check.</param>
    /// <returns>true if the specified enum has the specified attribute</returns>
    public static bool HasAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
    {
        return EnumHelper.HasAttribute<TAttribute>(enumValue);
    }

    /// <summary>
    /// Gets the values and descriptions of an enum type where the enum values are decorated with an EnumDescriptor attribute as a Dictionary.   
    /// </summary>
    /// <typeparam name="TEnum">The enumerable type.</typeparam>
    /// <returns>The identifier and value combinations of the enum.</returns>
    public static IDictionary<TEnum, string> GetValuesAndDescriptions<TEnum>(this Enum value) where TEnum : struct, Enum
    {
        return Enum.GetValues(value.GetType())
            .Cast<TEnum>()
            .ToDictionary(value => value, value => value.GetDescription());
    }
}
