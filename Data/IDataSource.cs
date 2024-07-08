namespace Alaveri.Data;

/// <summary>
/// Represents a generic data source.
/// </summary>
/// <typeparam name="TDataItem">The type of the data source.</typeparam>
/// <typeparam name="TDataItemId">The type of the data item ID.</typeparam>
public interface IDataSource<TDataItemId, TDataItem>
    where TDataItemId : IEquatable<TDataItemId>
    where TDataItem : class
{
    /// <summary>
    /// Reads a data item from the data source.
    /// </summary>
    /// <param name="id">The ID of the item to read.</param>
    /// <returns>A TDataItem object encapsulating the item's data.</returns>
    TDataItem ReadItem(TDataItemId id);

    /// <summary>
    /// Reads all data items from the data source.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{TDataItem}"/> object containing all data items.</returns>
    IDictionary<TDataItemId, TDataItem> ReadAllItems();

    /// <summary>
    /// Writes a data item to the data source.
    /// </summary>
    /// <param name="id">The ID of the item to write.</param>
    /// <param name="item">The item to write.</param>
    void WriteItem(TDataItemId id, TDataItem item);
}