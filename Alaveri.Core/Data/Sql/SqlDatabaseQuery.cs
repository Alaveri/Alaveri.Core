using Alaveri.Core.Data.Sql.Extensions;
using Alaveri.Core.Extensions;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace Alaveri.Core.Data.Sql;

/// <summary>
/// Represents a SQL Server database query.
/// </summary>
/// <remarks>
/// Initializes a new instance of the DatabaseQuery class using the specified connection.
/// </remarks>
/// <param name="connection">The connection used to access the database.</param>
public class SqlDatabaseQuery(IDbConnection connection) : DatabaseQuery(connection)
{
    /// <summary>
    /// Prepares a SqlCommand for execution.
    /// </summary>
    /// <param name="options">The database query options to use.</param>
    /// <returns>a SqlCommand instance ready for execution.</returns>
    protected virtual SqlCommand PrepareCommand(IDatabaseQueryOptions options)
    {
        if (Connection == null)
            throw new InvalidOperationException("Connection was not set.");
        if (Connection.State == ConnectionState.Closed)
            Connection.Open();

        TruncateStringParameters();
        var command = new SqlCommand(options.Query, Connection as SqlConnection, options.Transaction as SqlTransaction)
        {
            CommandTimeout = options.CommandTimeout.TotalSeconds.AsInt32()
        };
        command.Parameters.AddRange(options.Parameters.Select(parameter => parameter.AsSqlParameter()).ToArray());
        command.CommandType = options.CommandType;
        return command;
    }

    /// <summary>
    /// Executes the query and returns an IDataReader.
    /// </summary>
    /// <returns>an IDataReader that can be used to read the result set.</returns>
    public override IDataReader ExecuteReader()
    {
        var command = PrepareCommand(Options);
        return command.ExecuteReader();
    }

    /// <summary>
    /// Executes the query asynchronously and returns an IDataReader.
    /// </summary>
    /// <returns>an IDataReader that can be used to read the result set.</returns>
    public override async Task<IDataReader> ExecuteReaderAsync()
    {
        var command = PrepareCommand(Options);
        return await command.ExecuteReaderAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the query and returns a DataSet.
    /// </summary>
    /// <returns>a DataSet that contains the result set.</returns>
    public override DataSet ExecuteDataSet()
    {
        using var command = PrepareCommand(Options);
        using var da = new SqlDataAdapter(command);
        using var ds = new DataSet();
        da.Fill(ds);
        return ds;
    }

    /// <summary>
    /// Executes the query asynchronously and returns a DataSet.
    /// </summary>
    /// <returns>a DataSet that contains the result set.</returns>
    public async override Task<DataSet> ExecuteDataSetAsync()
    {
        using var command = PrepareCommand(Options);
        using var da = new SqlDataAdapter(command);
        using var ds = new DataSet();
        await Task.Run(() => da.Fill(ds)).ConfigureAwait(false);
        return ds;
    }

    /// <summary>
    /// Executes the query as a DataSet and returns the first table in the set.
    /// </summary>
    /// <returns>a DataSet that contains the result set.</returns>
    public override DataTable ExecuteDataTable()
    {
        var ds = ExecuteDataSet();
        if (ds.Tables.Count == 0)
            return new DataTable();
        return ds.Tables[0];
    }

    /// <summary>
    /// Executes the query asynchronously and returns the first table in the set.
    /// </summary>
    /// <returns>a DataSet that contains the result set.</returns>
    public async override Task<DataTable> ExecuteDataTableAsync()
    {
        var ds = await ExecuteDataSetAsync().ConfigureAwait(false);
        if (ds.Tables.Count == 0)
            return new DataTable();
        return ds.Tables[0];
    }

    /// <summary>
    /// Executes the query and returns the first item in the result set as type TResult.
    /// </summary>
    /// <typeparam name="TResult">The type of the value to be returned.</typeparam>
    /// <returns>The first item in the result set.</returns>
    public override TResult ExecuteScalar<TResult>(TResult defaultValue)
    {
        using var command = PrepareCommand(Options);
        var value = command.ExecuteScalar();
        if (value is TResult result)
            return result;
        if (defaultValue is IConvertible def && value is IConvertible convertible)
            return (TResult)SafeConvert.ConvertObject(value, def);
        return defaultValue;
    }

    /// <summary>
    /// Executes the query and returns the first item in the result set as type TResult.
    /// </summary>
    /// <param name="defaultValue">The default value if the returned value cannot be converted.</param>
    /// <typeparam name="TResult">The type of the value to be returned.</typeparam>
    /// <returns>The first item in the result set.</returns>
    /// 
    public override async Task<TResult> ExecuteScalarAsync<TResult>(TResult defaultValue)
    {
        using var command = PrepareCommand(Options);
        var value = await command.ExecuteScalarAsync().ConfigureAwait(false);
        if (value is TResult result)
            return result;
        if (defaultValue is IConvertible def && value is IConvertible convertible)
            return (TResult)SafeConvert.ConvertObject(value, def);
        return defaultValue;
    }

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    public override void Execute()
    {
        using var command = PrepareCommand(Options);
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    public override async void ExecuteAsync()
    {
        using var command = PrepareCommand(Options);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    public override XmlReader ExecuteXmlReader()
    {
        using var command = PrepareCommand(Options);
        return command.ExecuteXmlReader();
    }

    /// <summary>
    /// Executes the query with no result set returned.
    /// </summary>
    public override async Task<XmlReader> ExecuteXmlReaderAsync()
    {
        using var command = PrepareCommand(Options);
        return await command.ExecuteXmlReaderAsync().ConfigureAwait(false);
    }
}
