using System;
using System.Data.SqlClient;

namespace DeviceId.SqlServer;

/// <summary>
/// Provides a fluent interface for adding SQL Server components to a device identifier.
/// </summary>
public class SqlServerDeviceIdBuilder
{
    /// <summary>
    /// The base device identifier builder.
    /// </summary>
    private readonly DeviceIdBuilder _baseBuilder;

    /// <summary>
    /// A factory used to get a connection to the SQL Server database.
    /// </summary>
    private readonly Func<SqlConnection> _connectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlServerDeviceIdBuilder"/> class.
    /// </summary>
    /// <param name="baseBuilder">The base device identifier builder.</param>
    /// <param name="connection">A connection to the SQL Server database.</param>
    public SqlServerDeviceIdBuilder(DeviceIdBuilder baseBuilder, SqlConnection connection)
        : this(baseBuilder, () => connection) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlServerDeviceIdBuilder"/> class.
    /// </summary>
    /// <param name="baseBuilder">The base device identifier builder.</param>
    /// <param name="connectionFactory">A factory used to get a connection to the SQL Server database.</param>
    public SqlServerDeviceIdBuilder(DeviceIdBuilder baseBuilder, Func<SqlConnection> connectionFactory)
    {
        _baseBuilder = baseBuilder ?? throw new ArgumentNullException(nameof(baseBuilder));
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    /// <summary>
    /// Adds the result of a SQL query to the device identifier.
    /// </summary>
    /// <param name="componentName">The name of the component.</param>
    /// <param name="sql">SQL query that returns a single value to be added to the device identifier.</param>
    /// <returns>The <see cref="SqlServerDeviceIdBuilder"/> instance.</returns>
    public SqlServerDeviceIdBuilder AddQueryResult(string componentName, string sql)
    {
        return AddQueryResult(componentName, sql, x => x.ToString());
    }

    /// <summary>
    /// Adds the result of a SQL query to the device identifier.
    /// </summary>
    /// <param name="componentName">The name of the component.</param>
    /// <param name="sql">SQL query that returns a single value to be added to the device identifier.</param>
    /// <param name="valueTransformer">A function that transforms the result of the query into a string.</param>
    /// <returns>The <see cref="SqlServerDeviceIdBuilder"/> instance.</returns>
    public SqlServerDeviceIdBuilder AddQueryResult(string componentName, string sql, Func<object, string> valueTransformer)
    {
        _baseBuilder.Components.Add(componentName, new SqlDeviceIdComponent(_connectionFactory, sql, valueTransformer));

        return this;
    }
}
