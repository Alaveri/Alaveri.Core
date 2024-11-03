using System.Data;
using System.Xml;

namespace Alaveri.Core.Data;

/// <summary>
/// Represents a database query.
/// </summary>
public interface IDatabaseQuery
{
    /// <summary>
    /// The connection used to access the database.
    /// </summary>
    IDbConnection Connection { get; }

    /// <summary>
    /// The options used to execute the query.
    /// </summary>
    IDatabaseQueryOptions Options { get; set; }

    /// <summary>
    /// Executes the query and returns an IDataReader.
    /// </summary>
    /// <returns>an IDataReader that can be used to read the result set.</returns>
    IDataReader ExecuteReader();

    /// <summary>
    /// Executes the query asynchronously and returns an IDataReader.
    /// </summary>
    /// <returns>an IDataReader that can be used to read the result set.</returns>
    Task<IDataReader> ExecuteReaderAsync();

    /// <summary>
    /// Executes the query and returns a DataSet.
    /// </summary>
    /// <returns>a DataSet that contains the result set.</returns>
    DataSet ExecuteDataSet();

    /// <summary>
    /// Executes the query asynchronously and returns a DataSet.
    /// </summary>
    /// <returns>a DataSet that contains the result set.</returns>
    Task<DataSet> ExecuteDataSetAsync();

    /// <summary>
    /// Executes the query and returns a DataSet.
    /// </summary>
    /// <returns>a DataTable that contains the result set.</returns>
    DataTable ExecuteDataTable();

    /// <summary>
    /// Executes the query asynchronously as a DataSet and returns the first table in the set.
    /// </summary>
    /// <returns>a DataTable that contains the result set.</returns>
    Task<DataTable> ExecuteDataTableAsync();

    /// <summary>
    /// Executes the query and returns the first item in the result set as type TResult.
    /// </summary>
    /// <param name="defaultValue">The default value if the returned value cannot be converted.</param>
    /// <typeparam name="TResult">The type of the value to be returned.</typeparam>
    /// <returns>The first item in the result set.</returns>
    TResult ExecuteScalar<TResult>(TResult defaultValue);

    /// <summary>
    /// Executes the query and returns the first item in the result set as type TResult.
    /// </summary>
    /// <param name="defaultValue">The default value if the returned value cannot be converted.</param>
    /// <typeparam name="TResult">The type of the value to be returned.</typeparam>
    /// <returns>The first item in the result set.</returns>
    Task<TResult> ExecuteScalarAsync<TResult>(TResult defaultValue);

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    void Execute();

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    void ExecuteAsync();

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    XmlReader ExecuteXmlReader();

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    Task<XmlReader> ExecuteXmlReaderAsync();

    /// <summary>
    /// Sets the transaction for the query to the specified transaction.
    /// </summary>
    /// <param name="transaction">The transaction to use for queries.</param>
    /// <returns>this query as an IDatabaseQuery.</returns>
    IDatabaseQuery WithTransaction(IDbTransaction transaction);

    /// <summary>
    /// Sets the timeout for this query.
    /// </summary>
    /// <param name="timeout">The timeout to use for queries.</param>
    /// <returns>this query as an IDatabaseQuery.</returns>
    IDatabaseQuery WithTimeout(TimeSpan timeout);

    /// <summary>
    /// Sets the parameters for this query.
    /// </summary>
    /// <param name="parameters">The parameters to use for this query.</param>
    /// <returns>this query as an IDatabaseQuery.</returns>
    IDatabaseQuery WithParameters(params IDbDataParameter[] parameters);

    /// <summary>
    /// Sets the parameters for this query.
    /// </summary>
    /// <param name="parameters">The parameters to use for this query.</param>
    /// <returns>this query as an IDatabaseQuery.</returns>
    IDatabaseQuery WithParameters(IEnumerable<IDbDataParameter> parameters);

    /// <summary>
    /// Adds a new parameter to the query.
    /// </summary>
    /// <param name="parameter">A new parameter to use for this query.</param>
    /// <returns>this query as an IDatabaseQuery.</returns>
    IDatabaseQuery AddParameter(IDbDataParameter parameter);

    /// <summary>
    /// Truncates all string parameters to the size of the parameter.
    /// </summary>
    void TruncateStringParameters();
}
