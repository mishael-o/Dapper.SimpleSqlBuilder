using System.Data;

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// A class that defines a parameter and its properties.
/// </summary>
public sealed class SimpleParameterInfo : ISimpleParameterInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleParameterInfo"/> class.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    /// <param name="dbType">The parameter <see cref="System.Data.DbType"/>.</param>
    /// <param name="size">The parameter size.</param>
    /// <param name="precision">The parameter precision.</param>
    /// <param name="scale">The parameter scale.</param>
    public SimpleParameterInfo(object? value, DbType? dbType = null, int? size = null, byte? precision = null, byte? scale = null)
    : this(null, value, dbType, size: size, precision: precision, scale: scale)
    {
    }

    internal SimpleParameterInfo(string? name, object? value, DbType? dbType = null, ParameterDirection direction = ParameterDirection.Input, int? size = null, byte? precision = null, byte? scale = null, Type? type = null)
    {
        Value = value;
        Name = name;
        DbType = dbType;
        Direction = direction;
        Size = size;
        Precision = precision;
        Scale = scale;
        Type = type ?? value?.GetType();
    }

    /// <summary>
    /// Gets the parameter value.
    /// </summary>
    public object? Value { get; }

    internal string? Name { get; private set; }
    internal ParameterDirection Direction { get; }

    /// <summary>
    /// Gets the parameter <see cref="System.Data.DbType"/>.
    /// </summary>
    public DbType? DbType { get; }

    /// <summary>
    /// Gets the parameter precision.
    /// </summary>
    public byte? Precision { get; }

    /// <summary>
    /// Gets the parameter size.
    /// </summary>
    public int? Size { get; }

    /// <summary>
    /// Gets the parameter scale.
    /// </summary>
    public byte? Scale { get; }

    internal Type? Type { get; }

#if NET6_0_OR_GREATER
    [MemberNotNullWhen(true, nameof(Value), nameof(Type))]
#endif
    internal bool HasValue => Value is not null;

#if NET6_0_OR_GREATER
    [MemberNotNullWhen(true, nameof(Name))]
#endif
    internal bool HasName => !string.IsNullOrWhiteSpace(Name);

    internal void SetName(string name)
    {
        if (HasName)
        {
            throw new InvalidOperationException($"{nameof(Name)} has a value and cannot be changed.");
        }

        Name = name;
    }
}
