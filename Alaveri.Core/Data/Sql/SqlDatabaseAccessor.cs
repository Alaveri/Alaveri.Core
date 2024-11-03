using Alaveri.Core.Data;
using System.Data;
using System.Data.SqlClient;

namespace Alaveri.Core.Data.Sql;

/// <summary>
/// Represents an object that accesses a SQL Server database.
/// </summary>
public class SqlDatabaseAccessor : DatabaseAccessor
{
    /// <summary>
    /// The credential used for this connection.
    /// </summary>
    public SqlCredential? Credential { get; }

    /// <summary>
    /// Starts a transaction.
    /// </summary>
    /// <param name="name">The name of this transaction.</param>
    /// <param name="isolationLevel">The isolation level of the transaction.</param>
    /// <returns>the new transaction.</returns>
    public override IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified, string name = "")
    {
        if (Connection is not SqlConnection connection)
            throw new InvalidOperationException("Connection is not a SqlConnection.");
        return connection.BeginTransaction(isolationLevel, name);
    }

    /// <summary>
    /// Creates a new database query as a stored procedure call.
    /// </summary>
    /// <param name="procedureName">The name of the stored procedure to execute.</param>
    /// <returns>An IDatabaseQuery configured as a stored procedure.</returns>
    public override IDatabaseQuery StoredProcedure(string procedureName)
    {
        return new SqlDatabaseQuery(Connection)
        {
            Options = new DatabaseQueryOptions
            {
                CommandType = CommandType.StoredProcedure,
                Query = procedureName
            }
        };
    }

    /// <summary>
    /// Creates a new database query as a SQL statement.
    /// </summary>
    /// <param name="sql">The sql statement to execute.</param>
    /// <returns>An IDatabaseQuery configured as a SQL statement.</returns>
    public override IDatabaseQuery SqlStatement(string sql)
    {
        return new SqlDatabaseQuery(Connection)
        {
            Options = new DatabaseQueryOptions
            {
                CommandType = CommandType.Text,
                Query = sql
            }
        };
    }

    /// <summary>
    /// Creates a new database query as a direct table access.
    /// </summary>
    /// <param name="sql">The sql statement to execute.</param>
    /// <returns>An IDatabaseQuery configured as direct table access.</returns>
    public override IDatabaseQuery TableDirect(string name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates the connection object for this accessor.
    /// </summary>
    /// <param name="connectionString">The connection string to use for the connection.</param>
    /// <returns>An <see cref="IDbConnection"/> object for this accessor.</returns>
    protected override IDbConnection CreateConnection(string connectionString)
    {
        return new SqlConnection(connectionString);
    }

    /// <summary>
    /// Initializes a new instance of the SqlDatabaseAccessor class using the specified connection.
    /// </summary>
    /// <param name="connection">The connection used to access the database.</param>
    public SqlDatabaseAccessor(SqlConnection connection) : base(connection)
    {
        Credential = connection.Credential;
    }

    /// <summary>
    /// Initializes a new instance of the SqlDatabaseAccessor class using the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string used to connect to the database.</param>
    /// <param name="credential">The credential used for this connection.</param>
    public SqlDatabaseAccessor(string connectionString = "", SqlCredential? credential = null) : base(connectionString)
    {
        Credential = credential;
        ((SqlConnection)Connection).Credential = credential;
    }

}
