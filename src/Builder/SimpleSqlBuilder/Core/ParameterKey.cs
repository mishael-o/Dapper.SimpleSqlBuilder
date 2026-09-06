using System.Data;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// A key used to look up already rendered parameters when parameters are reused.
/// </summary>
internal readonly struct ParameterKey : IEquatable<ParameterKey>
{
    private readonly object? value;
    private readonly Type? type;
    private readonly DbType? dbType;
    private readonly int? size;
    private readonly byte? precision;
    private readonly byte? scale;

    internal ParameterKey(object? value, Type? type, DbType? dbType, int? size, byte? precision, byte? scale)
    {
        this.value = value;
        this.type = type;
        this.dbType = dbType;
        this.size = size;
        this.precision = precision;
        this.scale = scale;
    }

    /// <summary>
    /// Creates a key for a plain interpolated value, which carries no explicit parameter metadata.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    /// <returns>A new <see cref="ParameterKey"/>.</returns>
    internal static ParameterKey Create(object value)
        => new(value, value.GetType(), null, null, null, null);

    public bool Equals(ParameterKey other)
    {
        // A parameter with no value never matches anything, including another valueless parameter, so each
        // one gets its own placeholder. This also makes a default(ParameterKey) harmless rather than a
        // null reference waiting to be dereferenced.
        return value is not null
            && other.value is not null
            && type == other.type
            && value.Equals(other.value)
            && dbType == other.dbType
            && size == other.size
            && precision == other.precision
            && scale == other.scale;
    }

    public override bool Equals(object? obj)
        => obj is ParameterKey other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(value, type, dbType, size, precision, scale);
}
