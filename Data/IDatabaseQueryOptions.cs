using System.Data;

namespace Alaveri.Data;

/// <summary>
/// Options used while querying the data source.
/// </summary>
public interface IDatabaseQueryOptions
{
    /// <summary>
    /// The timeout used for this query.
    /// </summary>
    TimeSpan CommandTimeout { get; set; }

    /// <summary>
    /// The text of the query.
    /// </summary>
    string Query { get; set; }

    /// <summary>
    /// The transaction used for this query.
    /// </summary>
    IDbTransaction? Transaction { get; set; }

    /// <summary>
    /// The type of command used for this query.
    /// </summary>
    CommandType CommandType { get; set; }

    /// <summary>
    /// The list of parameters to use in the query.
    /// </summary>
    IList<IDbDataParameter> Parameters { get; }

    /// <summary>
    /// If true, string parameters will be truncated to the parameter's size.
    /// </summary>
    bool TruncateStrings { get; set; }
}