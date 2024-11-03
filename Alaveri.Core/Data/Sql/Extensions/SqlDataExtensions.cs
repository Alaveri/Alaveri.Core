using Alaveri.Core.Extensions;
using System.Data;
using System.Data.SqlClient;

namespace Alaveri.Core.Data.Sql.Extensions;

/// <summary>
/// Provides extensions for Sql-related utilities.
/// </summary>
public static class SqlDataExtensions
{
    /// <summary>
    /// Converts a DbParameter into a SqlParameter.
    /// </summary>
    /// <param name="parameter">The parameter co convert.</param>
    /// <param name="truncateStringParameters">If true, truncates string parameters to the specified parameter size.</param>
    public static SqlParameter AsSqlParameter(this IDbDataParameter parameter, bool truncateStringParameters = true)
    {
        if (parameter is SqlParameter sqlParameter)
            return sqlParameter;

        var result = new SqlParameter()
        {
            DbType = parameter.DbType,
            Direction = parameter.Direction,
            ParameterName = parameter.ParameterName,
            Precision = parameter.Precision,
            Scale = parameter.Scale,
            Size = parameter.Size,
            SourceColumn = parameter.SourceColumn,
            SourceVersion = parameter.SourceVersion,
            Value = parameter.Value
        };

        if (truncateStringParameters && result.Size > 0 && result.Value is string stringValue && result.Size > 0)
            result.Value = stringValue.Truncate(parameter.Size);

        return result;
    }

}
