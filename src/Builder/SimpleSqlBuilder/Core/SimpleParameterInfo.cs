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
        : this(null, value, dbType, null, size, precision, scale)
    {
    }

    internal SimpleParameterInfo(string? name, object? value, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null)
    {
        Value = value;
        Name = name;
        DbType = dbType;
        Direction = direction ?? ParameterDirection.Input;
        Size = size;
        Precision = precision;
        Scale = scale;
        Type = value?.GetType();
    }

    /// <inheritdoc/>
    public object? Value { get; }

    internal string? Name { get; private set; }

    internal ParameterDirection Direction { get; }

    /// <inheritdoc/>
    public DbType? DbType { get; }

    /// <inheritdoc/>
    public byte? Precision { get; }

    /// <inheritdoc/>
    public int? Size { get; }

    /// <inheritdoc/>
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
