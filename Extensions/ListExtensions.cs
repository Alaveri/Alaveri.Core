using Alaveri.Core;

namespace Alaveri.Extensions;

/// <summary>
/// Provides a set of static methods for querying objects that implement <see cref="IList{TListItem}"/>.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Performs the specified action on each element of the <see cref="IList{TListItem}"/>.
    /// </summary>
    /// <typeparam name="TListItem">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The <see cref="IList{TListItem}"/> to iterate.</param>
    /// <param name="action">The <see cref="IndexAction{TListItem}"/> delegate to perform on each element of the <see cref="IList{TListItem}"/>.</param>
    public static void ForEachIndex<TListItem>(this IList<TListItem> source, IndexAction<TListItem> action)
    {
        for (var index = 0; index < source.Count; index++)
            action(source[index], index);
    }
}