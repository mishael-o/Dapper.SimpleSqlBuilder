using System.Data;

namespace Dapper.SimpleSqlBuilder.Extensions;

/// <summary>
/// An extension class for <see cref="ISimpleParameterInfo"/>.
/// </summary>
public static class ISimpleParameterInfoExtensions
{
    /// <summary>
    /// An extension method to create a <see cref="ISimpleParameterInfo"/>.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="value">The parameter value.</param>
    /// <param name="dbType">The parameter <see cref="DbType"/>.</param>
    /// <param name="size">The parameter size.</param>
    /// <param name="precision">The parameter precision.</param>
    /// <param name="scale">The parameter scale.</param>
    /// <returns>Returns a <see cref="ISimpleParameterInfo"/>.</returns>
    /// <exception cref="ArgumentException">Throws an <see cref="ArgumentException"/> when called on <see cref="ISimpleParameterInfo"/>.</exception>
    public static ISimpleParameterInfo DefineParam<T>(this T value, DbType? dbType = null, int? size = null, byte? precision = null, byte? scale = null)
    {
        if (value is ISimpleParameterInfo)
        {
            throw new ArgumentException($"Value is already a {nameof(ISimpleParameterInfo)}.", nameof(value));
        }

        return new SimpleParameterInfo(value, dbType: dbType, size: size, precision: precision, scale: scale);
    }
}
