namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// An interface that defines the SQL builder type.
/// </summary>
public interface ISqlBuilder
{
    /// <summary>
    /// Gets the generated the SQL.
    /// </summary>
    string Sql { get; }

    /// <summary>
    /// Gets the <see cref="DynamicParameters"/> list.
    /// </summary>
    object Parameters { get; }

    /// <summary>
    /// Gets the parameter names.
    /// </summary>
    IEnumerable<string> ParameterNames { get; }
}
