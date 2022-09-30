using System.Data;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// An interface that defines the simple parameter info type or contract.
/// </summary>
public interface ISimpleParameterInfo
{
    /// <summary>
    /// Gets the parameter value.
    /// </summary>
    object? Value { get; }

    /// <summary>
    /// Gets the parameter <see cref="System.Data.DbType"/>.
    /// </summary>
    DbType? DbType { get; }

    /// <summary>
    /// Gets the parameter precision.
    /// </summary>
    byte? Precision { get; }

    /// <summary>
    /// Gets the parameter scale.
    /// </summary>
    byte? Scale { get; }

    /// <summary>
    /// Gets the parameter size.
    /// </summary>
    int? Size { get; }
}
