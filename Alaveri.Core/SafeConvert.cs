using System.Globalization;

namespace Alaveri.Core
{
    /// <summary>
    /// Performs safe conversions for objects to IConvertible types.
    /// </summary>
    public static class SafeConvert
    {
        /// <summary>
        /// Holds the list of values considered to be "True" for boolean conversions and comparisons.
        /// Defaults to "true", "yes", "y" and "1".
        /// </summary>
        public static IEnumerable<string> TrueValues { get; set; } =
        [
            "true", "yes", "y", "1"
        ];

        /// <summary>
        /// Holds the list of values considered to be "False" for boolean conversions and comparisons.
        /// Defaults to "true", "yes", "y" and "1".
        /// </summary>
        public static IEnumerable<string> FalseValues { get; set; } =
        [
            "false", "no", "n", "0"
        ];

        /// <summary>
        /// Converts a variable from one type to another and returns a default if the conversion is invalid, null or DBNull.
        /// </summary>
        /// <typeparam name="TResult">The type to which the value will be converted.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value of the value is invalid.</param>
        /// <param name="culture">The culture used for culture-specific formatting information during conversions.</param>
        /// <returns>The converted value or the default.</returns>
        public static TResult ChangeType<TResult>(object? value, TResult defaultValue, CultureInfo culture) where TResult : IConvertible
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;
            if (typeof(TResult) == typeof(bool))
            {
                var val = value?.ToString()?.Trim();
                if (CompareHelper.StringInSequence(val, TrueValues))
                    return (TResult)(object)true;
                else if (CompareHelper.StringInSequence(val, FalseValues))
                    return (TResult)(object)false;
                else
                    return defaultValue;
            }
            try
            {
                return (TResult)Convert.ChangeType(value, typeof(TResult), culture);
            }
            catch (Exception ex) when
            (
                ex is FormatException ||
                ex is InvalidCastException ||
                ex is OverflowException
            )
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Converts a variable from one type to another and returns a default if the conversion is invalid, null or DBNull.  Uses
        /// the current culture of the current thread for culture-specific formatting information used in conversions.
        /// </summary>
        /// <typeparam name="TResult">The type to which the value will be converted.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value of the value is invalid.</param>
        /// <returns>The converted value or the default.</returns>
        public static TResult ChangeType<TResult>(object? value, TResult defaultValue) where TResult : IConvertible
        {
            return ChangeType(value, defaultValue, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts an object from one type to another and returns a default if the conversion is invalid, null or DBNull.
        /// </summary>
        /// <typeparam name="TResult">The type to which the value will be converted.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value of the value is invalid.</param>
        /// <returns>The converted value or the default.</returns>
        public static TResult ConvertObject<TResult>(object? value, TResult defaultValue) where TResult : IConvertible
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to Int16.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static short ToInt16(object? value, short defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to Int32.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static int ToInt32(object? value, int defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to Int64.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static long ToInt64(object? value, long defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to UInt16.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static ushort ToUInt16(object? value, ushort defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to UInt32.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static uint ToUInt32(object? value, uint defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to UInt64.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static ulong ToUInt64(object? value, ulong defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to Single.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static float ToSingle(object? value, float defaultValue = default)
        {
            var converted = ChangeType(value, defaultValue);
            if (float.IsInfinity(converted))
                return defaultValue;
            else
                return converted;
        }

        /// <summary>
        /// Converts an object to Double.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static double ToDouble(object? value, double defaultValue = default)
        {
            var converted = ChangeType(value, defaultValue);
            if (double.IsInfinity(converted))
                return defaultValue;
            else
                return converted;
        }

        /// <summary>
        /// Converts an object to String.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static string ToString(object? value, string defaultValue = "")
        {
            return value?.ToString() ?? defaultValue;
        }

        /// <summary>
        /// Converts an object to DateTime.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static DateTime ToDateTime(object? value, DateTime defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to Boolean.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static bool ToBoolean(object? value, bool defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to Byte.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static byte ToByte(object? value, byte defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to Char.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static char ToChar(object? value, char defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to Decimal.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static decimal ToDecimal(object? value, decimal defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts an object to SByte.  Returns the default value if the conversion is invalid, null, or DBNull.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted object or the default.</returns>
        public static sbyte ToSByte(object? value, sbyte defaultValue = default)
        {
            return ChangeType(value, defaultValue);
        }

        /// <summary>
        /// Converts a long value to a hexadecimal string.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The hexadecimal representation of the value.</returns>
        public static string ToHexString(object? value, string prefix = "0x")
        {
            if (value is byte byteValue)
                return prefix + byteValue.ToString("X2");
            if (value is ushort ushortValue)
                return prefix + ushortValue.ToString("X4");
            if (value is uint uintValue)
                return prefix + uintValue.ToString("X8");
            if (value is ulong ulongValue)
                return prefix + ulongValue.ToString("X16");
            return string.Empty;
        }
    }
}