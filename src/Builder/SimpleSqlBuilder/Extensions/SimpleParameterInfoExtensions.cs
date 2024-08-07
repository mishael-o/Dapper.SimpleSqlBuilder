﻿using System.Data;

namespace Dapper.SimpleSqlBuilder.Extensions;

/// <summary>
/// An extension class for <see cref="SimpleParameterInfo"/>.
/// </summary>
public static class SimpleParameterInfoExtensions
{
    /// <summary>
    /// An extension method to create an instance of <see cref="ISimpleParameterInfo"/> with the specified <paramref name="dbType"/>, <paramref name="size"/>, <paramref name="precision"/>, and <paramref name="scale"/>.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="value">The parameter value.</param>
    /// <param name="dbType">The parameter <see cref="DbType"/>.</param>
    /// <param name="size">The parameter size.</param>
    /// <param name="precision">The parameter precision.</param>
    /// <param name="scale">The parameter scale.</param>
    /// <returns>An new instance of <see cref="ISimpleParameterInfo"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when called on <see cref="ISimpleParameterInfo"/>.</exception>
    public static ISimpleParameterInfo DefineParam<T>(this T value, DbType? dbType = null, int? size = null, byte? precision = null, byte? scale = null)
    {
        return value is ISimpleParameterInfo
            ? throw new ArgumentException($"Value is already a {nameof(ISimpleParameterInfo)}.", nameof(value))
            : new SimpleParameterInfo(value, dbType, size, precision, scale);
    }
}
