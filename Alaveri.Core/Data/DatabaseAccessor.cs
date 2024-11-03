using System.Data;

namespace Alaveri.Core.Data;

/// <summary>
/// Represents an object that accesses a database.
/// </summary>
public abstract class DatabaseAccessor : IDatabaseAccessor
{
    /// <summary>
    /// If true, the accessor will truncate the length of string parameters to the specified
    /// parameter size.
    /// </summary>
    public bool TruncateStringParameters { get; set; } = true;

    /// <summary>
    /// True of the database accessor has been disposed.
    /// </summary>
    protected bool Disposed { get; set; } = false;

    /// <summary>
    /// The connection to the database.
    /// </summary>
    public IDbConnection Connection { get; protected set; }

    /// <summary>
    /// The connection string used to access the database.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the wait time (in seconds) before terminating the attempt to execute a command and generating an error.
    /// </summary>
    public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// The transaction used for queries. Note: use CreateTransaction() to create a transaction
    /// appropriate to this accessor type.
    /// </summary>
    public virtual IDbTransaction? Transaction { get; set; }

    /// <summary>
    /// Starts a transaction.
    /// </summary>
    /// <param name="name">The name of this transaction.</param>
    /// <param name="isolationLevel">The isolation level of the transaction.</param>
    /// <returns>the new transaction.</returns>
    public abstract IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified, string name = "");

    /// <summary>
    /// Perform disposal of managed resources.
    /// </summary>
    /// <param name="disposing">True if the object is being disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Connection?.Close();
            Connection?.Dispose();
        }
    }

    /// <summary>
    /// Creates the connection object for this accessor.
    /// </summary>
    /// <param name="connectionString">The connection string to use for the connection.</param>
    /// <returns>An <see cref="IDbConnection"/> object for this accessor.</returns>
    protected abstract IDbConnection CreateConnection(string connectionString);

    /// <summary>
    /// Creates a new database query as a stored procedure call.
    /// </summary>
    /// <param name="procedureName">The name of the stored procedure to execute.</param>
    /// <returns>An IDatabaseQuery configured as a stored procedure.</returns>
    public abstract IDatabaseQuery StoredProcedure(string procedureName);

    /// <summary>
    /// Creates a new database query as a SQL statement.
    /// </summary>
    /// <param name="sql">The sql statement to execute.</param>
    /// <returns>An IDatabaseQuery configured as a SQL statement.</returns>
    public abstract IDatabaseQuery SqlStatement(string sql);

    /// <summary>
    /// Creates a new database query as a direct table access.
    /// </summary>
    /// <param name="name">The name of the table.</param>
    /// <returns>An IDatabaseQuery configured as direct table access.</returns>
    public abstract IDatabaseQuery TableDirect(string name);

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Initializes a new instance of the DatabaseAccessor class.
    /// </summary>
    public DatabaseAccessor(IDbConnection connection)
    {
        Connection = connection;
        ConnectionString = connection.ConnectionString;
    }

    /// <summary>
    /// Initializes a new instance of the DatabaseAccessor class using the specified connection string.
    /// </summary>
    public DatabaseAccessor(string connectionString)
    {
        ConnectionString = connectionString;
        Connection = CreateConnection(connectionString);
    }
}
