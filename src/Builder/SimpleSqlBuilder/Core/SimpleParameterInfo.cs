using System.Data;

#if NET8_0_OR_GREATER
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
    /// <param name="reuse">
    /// <see langword="true"/> to reuse this parameter for every use within the builder;
    /// <see langword="false"/> to create a separate parameter for each use. The default is
    /// <see langword="true"/>.
    /// <para>
    /// This setting affects only this parameter. The builder's <c>reuseParameters</c> option continues to govern interpolated values.
    /// </para>
    /// </param>
    public SimpleParameterInfo(object? value, DbType? dbType = null, int? size = null, byte? precision = null, byte? scale = null, bool reuse = true)
    {
        Value = value;
        DbType = dbType;
        Direction = ParameterDirection.Input;
        Size = size;
        Precision = precision;
        Scale = scale;
        Reuse = reuse;
        Type = value?.GetType();
    }

    /// <inheritdoc/>
    public object? Value { get; }

    /// <inheritdoc/>
    public DbType? DbType { get; }

    /// <inheritdoc/>
    public byte? Precision { get; }

    /// <inheritdoc/>
    public int? Size { get; }

    /// <inheritdoc/>
    public byte? Scale { get; }

    internal ParameterDirection Direction { get; }

    internal Type? Type { get; }

    internal bool Reuse { get; }

#if NET8_0_OR_GREATER
    [MemberNotNullWhen(true, nameof(Value), nameof(Type))]
#endif
    internal bool HasValue => Value is not null;

    /// <summary>
    /// Gets the identity used to match this parameter against others when parameters are reused.
    /// </summary>
    /// <returns>The <see cref="ParameterKey"/> for this parameter.</returns>
    /// <remarks>
    /// Every property that affects parameter equivalence must be included. Omitting one could cause
    /// differently configured parameters to share the same placeholder. A unit test ensures that every
    /// property is either included in the key or explicitly excluded from parameter identity.
    /// </remarks>
    internal ParameterKey ToKey()
        => new(Value, Type, DbType, Size, Precision, Scale);
}
