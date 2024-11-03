using Alaveri.Core.Extensions;

namespace Alaveri.Core;

/// <summary>
/// Provides comparison utilities.
/// </summary>
public static class CompareHelper
{
    /// <summary>
    /// Checks to see if a value is in a sequence.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value used to search.</param>
    /// <param name="sequence">The sequence to search.</param>
    /// <param name="comparer">An optional comparer used to compare items in the sequence to the value.</param>
    /// <returns>True of the value is in the sequence.</returns>
    public static bool InSequence<TValue>(TValue value, IEnumerable<TValue> sequence, IComparer<TValue>? comparer = null)
    {
        return sequence.Any(item => (comparer?.Compare(item, value) ?? item?.Equals(value).AsInt32() ?? 0) == 0); ;
    }

    /// <summary>
    /// Checks to see if a value is in a sequence.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value used to search.</param>
    /// <param name="sequence">The sequence to search.</param>
    /// <param name="comparer">An optional comparer used to compare items in the sequence to the value.</param>
    /// <returns>True of the value is in the sequence.</returns>
    public static bool InSequence<TValue>(TValue value, IComparer<TValue>? comparer = null, params TValue[] sequence)
    {
        return InSequence(value, sequence, comparer);
    }

    /// <summary>
    /// Gets a string comparer based on StringComparison type.
    /// </summary>
    /// <param name="comparisonType">Type of the comparison.</param>
    /// <returns>A string comparer based on the stringComparison.</returns>
    private static StringComparer GetStringComparer(StringComparison? comparisonType)
    {
        return comparisonType switch
        {
            StringComparison.CurrentCulture => StringComparer.CurrentCulture,
            StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
            StringComparison.InvariantCulture => StringComparer.InvariantCulture,
            StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase,
            StringComparison.Ordinal => StringComparer.Ordinal,
            StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
            _ => StringComparer.CurrentCultureIgnoreCase
        };
    }

    /// <summary>
    /// Checks to see if a string is in a sequence.
    /// </summary>
    /// <param name="value">The value used to search.</param>
    /// <param name="sequence">The sequence to search.</param>
    /// <param name="comparisonType">An optional comparison type used to compare strings in the sequence to the value.
    /// If none is provided, the comparison defaults to OrdinalIgnoreCase.</param>
    /// <returns>True of the value is in the sequence.</returns>
    public static bool StringInSequence(string? value, IEnumerable<string> sequence, StringComparison? comparisonType = null)
    {
        return InSequence(value, sequence, GetStringComparer(comparisonType));
    }

    /// <summary>
    /// Checks to see if a string is in a sequence, using StringComparison.OrdinalIgnoreCase.
    /// </summary>
    /// <param name="value">The value used to search.</param>
    /// <param name="sequence">The sequence to search.</param>
    /// <returns>True of the value is in the sequence.</returns>
    public static bool StringInSequence(string value, params string[] sequence)
    {
        return InSequence(value, sequence, StringComparer.OrdinalIgnoreCase);
    }


    /// <summary>
    /// Checks to see if a value is in a sequence using a binary search.  Assumes the sequence is already sorted.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="value">The value used to search.</param>
    /// <param name="sequence">The sequence to search.</param>
    /// <param name="comparer">An optional comparer used to compare items in the sequence to the value.</param>
    /// <returns>True of the value is in the list.</returns>
    /// <exception cref="ArgumentNullException">Sequence cannot be null.</exception>
    public static bool InSequenceBinary<TResult>(TResult value, List<TResult> sequence, IComparer<TResult>? comparer = null)
    {
        return sequence.BinarySearch(value, comparer) >= 0;
    }

    /// <summary>
    /// Checks to see if a string is in a sequence using a binary search.  Assumes the sequence is already sorted.
    /// </summary>
    /// <param name="value">The value used to search.</param>
    /// <param name="sequence">The sequence to search.</param>
    /// <param name="comparisonType">An optional comparison type used to compare strings in the sequence to the value.
    /// If none is provided, the comparison defaults to OrdinalIgnoreCase.</param>
    /// <returns>True of the value is in the sequence.</returns>
    /// <exception cref="ArgumentNullException">Sequence cannot be null.</exception>
    public static bool StringInSequenceBinary(string value, List<string> sequence, StringComparison? comparisonType = null)
    {
        return sequence.BinarySearch(value, GetStringComparer(comparisonType)) >= 0;
    }
}
