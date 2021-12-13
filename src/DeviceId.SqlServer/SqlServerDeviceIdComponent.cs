using System;
using System.Data.SqlClient;

namespace DeviceId.SqlServer;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/> that executes a SQL Server command.
/// </summary>
public class SqlDeviceIdComponent : IDeviceIdComponent
{
    /// <summary>
    /// A factory used to get a connection to the SQL Server database.
    /// </summary>
    private readonly Func<SqlConnection> _connectionFactory;

    /// <summary>
    /// SQL query that returns a single value to be added to the device identifier.
    /// </summary>
    private readonly string _sql;

    /// <summary>
    /// A function that transforms the result of the query into a string.
    /// </summary>
    private readonly Func<object, string> _valueTransformer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlDeviceIdComponent"/> class.
    /// </summary>
    /// <param name="connectionFactory">A factory used to get a connection to the SQL Server database.</param>
    /// <param name="sql">SQL query that returns a single value to be added to the device identifier.</param>
    /// <param name="valueTransformer">A function that transforms the result of the query into a string.</param>
    public SqlDeviceIdComponent(Func<SqlConnection> connectionFactory, string sql, Func<object, string> valueTransformer)
    {
        _connectionFactory = connectionFactory;
        _sql = sql;
        _valueTransformer = valueTransformer;
    }

    /// <summary>
    /// Gets the component value.
    /// </summary>
    /// <returns>The component value.</returns>
    public string GetValue()
    {
        try
        {
            using var connection = _connectionFactory.Invoke();

            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = _sql;

            var result = command.ExecuteScalar();
            if (result != null)
            {
                var value = _valueTransformer?.Invoke(result);
                return value;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }
}
