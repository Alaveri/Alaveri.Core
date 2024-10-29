namespace Alaveri.Core.Extensions;

/// <summary>
/// Provides string-related utilities.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns substring of a string.  Will not throw exceptions even if the parameters are out of 
    /// range, instead it will return a blank string.  If the start + length is longer than the string, it will
    /// return the string from the start to the end of the string.  If value is null, it will return a blank string.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="start">The starting position.</param>
    /// <param name="length">The length of the substring.  If not specified, returns the entire string starting at the start index.</param>
    /// <returns>The truncated string.</returns>
    public static string SafeSubstring(this string value, int start, int length = int.MaxValue)
    {
        return Strings.SafeSubstring(value, start, length);
    }

    /// <summary>
    /// Truncates a string to a given length.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="length">The new length of the string.</param>
    /// <returns>the truncated string.</returns>
    public static string Truncate(this string value, int length)
    {
        return Strings.Truncate(value, length);
    }
}
