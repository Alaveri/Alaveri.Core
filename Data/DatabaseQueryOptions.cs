using System.Data;

namespace Alaveri.Data;

/// <summary>
/// Options used while querying the data source.
/// </summary>
public class DatabaseQueryOptions : IDatabaseQueryOptions
{
    /// <summary>
    /// The timeout used for this query.
    /// </summary>
    public virtual TimeSpan CommandTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// The text of the query.
    /// </summary>
    public virtual string Query { get; set; } = string.Empty;

    /// <summary>
    /// The type of command used for this query.
    /// </summary>
    public virtual CommandType CommandType { get; set; }

    /// <summary>
    /// The list of parameters to use in the query.
    /// </summary>
    public virtual IList<IDbDataParameter> Parameters { get; } = [];

    /// <summary>
    /// The transaction used for this query.
    /// </summary>
    public virtual IDbTransaction? Transaction { get; set; }

    /// <summary>
    /// If true, string parameters will be truncated to the parameter's size.
    /// </summary>
    public virtual bool TruncateStrings { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the QueryableDataSourceOptions class.
    /// </summary>
    public DatabaseQueryOptions()
    {
    }
}
