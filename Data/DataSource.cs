namespace Alaveri.Data;

/// <summary>
/// Represents a generic data source.
/// </summary>
/// <typeparam name="TDataItem">The type of the data source.</typeparam>
/// <typeparam name="TDataItemId">The type of the data item ID.</typeparam>
public abstract class DataSource<TDataItemId, TDataItem> : IDataSource<TDataItemId, TDataItem>
    where TDataItem : class
    where TDataItemId : IEquatable<TDataItemId>
{
    /// <summary>
    /// Reads a data item from the data source.
    /// </summary>
    /// <param name="id">The ID of the item to read.</param>
    /// <returns>A TDataItem object encapsulating the item's data.</returns>
    public abstract TDataItem ReadItem(TDataItemId id);

    /// <summary>
    /// Reads all data items from the data source.
    /// </summary>
    /// <returns>An <see cref="IDictionary{TDataItemId, TDataItem}"/> object containing all data items.</returns>
    public abstract IDictionary<TDataItemId, TDataItem> ReadAllItems();

    /// <summary>
    /// Writes a data item to the data source.
    /// </summary>
    /// <param name="id">The ID of the item to write.</param>
    /// <param name="item">The item to write.</param>
    public abstract void WriteItem(TDataItemId id, TDataItem item);
}
