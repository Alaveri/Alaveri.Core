using System.Data;
using System.Xml;

namespace Alaveri.Data;

/// <summary>
/// Represents a database query.
/// </summary>
/// <remarks>
/// Initializes a new instance of the DatabaseQuery class using the specified connection.
/// </remarks>
/// <param name="connection">The connection used to access the database.</param>
public abstract class DatabaseQuery(IDbConnection connection) : IDatabaseQuery
{
    /// <summary>
    /// The connection used to access the database.
    /// </summary>
    public virtual IDbConnection Connection { get; protected set; } = connection;

    /// <summary>
    /// The options used to execute the query.
    /// </summary>
    public virtual IDatabaseQueryOptions Options { get; set; } = new DatabaseQueryOptions();

    /// <summary>
    /// Executes the query and returns an IDataReader.
    /// </summary>
    /// <returns>an IDataReader that can be used to read the result set.</returns>
    public abstract IDataReader ExecuteReader();

    /// <summary>
    /// Executes the query asynchronously and returns an IDataReader.
    /// </summary>
    /// <returns>an IDataReader that can be used to read the result set.</returns>
    public abstract Task<IDataReader> ExecuteReaderAsync();

    /// <summary>
    /// Executes the query and returns a DataSet.
    /// </summary>
    /// <returns>a DataSet that contains the result set.</returns>
    public abstract DataSet ExecuteDataSet();

    /// <summary>
    /// Executes the query asynchronously and returns a DataSet.
    /// </summary>
    /// <returns>a DataSet that contains the result set.</returns>
    public abstract Task<DataSet> ExecuteDataSetAsync();

    /// <summary>
    /// Executes the query and returns a DataSet.
    /// </summary>
    /// <returns>a DataTable that contains the result set.</returns>
    public abstract DataTable ExecuteDataTable();

    /// <summary>
    /// Executes the query asynchronously as a DataSet and returns the first table in the set.
    /// </summary>
    /// <returns>a DataTable that contains the result set.</returns>
    public abstract Task<DataTable> ExecuteDataTableAsync();

    /// <summary>
    /// Executes the query and returns the first item in the result set as type TResult.
    /// </summary>
    /// <param name="defaultValue">The default value if the returned value cannot be converted.</param>
    /// <typeparam name="TResult">The type of the value to be returned.</typeparam>
    /// <returns>The first item in the result set.</returns>
    public abstract TResult ExecuteScalar<TResult>(TResult defaultValue);

    /// <summary>
    /// Executes the query and returns the first item in the result set as type TResult.
    /// </summary>
    /// <param name="defaultValue">The default value if the returned value cannot be converted.</param>
    /// <typeparam name="TResult">The type of the value to be returned.</typeparam>
    /// <returns>The first item in the result set.</returns>
    public abstract Task<TResult> ExecuteScalarAsync<TResult>(TResult defaultValue); 

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    public abstract void ExecuteAsync();

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    public abstract XmlReader ExecuteXmlReader();

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    public abstract Task<XmlReader> ExecuteXmlReaderAsync();

    /// <summary>
    /// Truncates all string parameters to the size of the parameter.
    /// </summary>
    public virtual void TruncateStringParameters()
    {
        if (!Options.TruncateStrings || Options == null || Options.Parameters == null)
            return;
        foreach (IDbDataParameter parameter in Options.Parameters)
        {
            if (parameter.Value is string paramValue)
            {
                if (paramValue != null && parameter.Size > 0)
                    parameter.Value = paramValue[..Math.Min(parameter.Size, paramValue.Length)];
            } 
        }
    }

    /// <summary>
    /// Sets the transaction for the query to the specified transaction.
    /// </summary>
    /// <param name="transaction">The transaction to use for queries.</param>
    /// <returns>this query as an IDatabaseQuery.</returns>
    public virtual IDatabaseQuery WithTransaction(IDbTransaction transaction)
    {
        Options.Transaction = transaction;
        return this;
    }

    /// <summary>
    /// Sets the timeout for this query.
    /// </summary>
    /// <param name="timeout">The timeout to use for queries.</param>
    /// <returns>this query as an IDatabaseQuery.</returns>
    public virtual IDatabaseQuery WithTimeout(TimeSpan timeout)
    {
        Options.CommandTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Sets the parameters for this query.
    /// </summary>
    /// <param name="parameters">The parameters to use for this query.</param>
    /// <returns>this query as an IDatabaseQuery.</returns>
    public virtual IDatabaseQuery WithParameters(params IDbDataParameter[] parameters)
    {
        Options.Parameters.Clear();
        parameters.ToList().ForEach(Options.Parameters.Add);
        return this;
    }

    /// <summary>
    /// Sets the parameters for this query.
    /// </summary>
    /// <param name="parameters">The parameters to use for this query.</param>
    /// <returns>this query as an IDatabaseQuery.</returns>
    public virtual IDatabaseQuery WithParameters(IEnumerable<IDbDataParameter> parameters)
    {
        Options.Parameters.Clear();
        parameters.ToList().ForEach(Options.Parameters.Add);
        return this;
    }

    /// <summary>
    /// Adds a new parameter to the query.
    /// </summary>
    /// <param name="parameter">A new parameter to use for this query.</param>
    /// <returns>this query as an IDatabaseQuery.</returns>
    public virtual IDatabaseQuery AddParameter(IDbDataParameter parameter)
    {
        Options.Parameters.Add(parameter);
        return this;
    }
}
