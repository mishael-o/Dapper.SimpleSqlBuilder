﻿using System.Data;

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the fluent sql builder type or contract.
/// </summary>
public interface IFluentSqlBuilder : ISqlBuilder, IFluentBuilder
{
    /// <summary>
    /// Adds a parameter to the <see cref="ISqlBuilder.Parameters">dynamic parameter</see> list.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <param name="dbType">The <see cref="DbType"/> of the parameter.</param>
    /// <param name="direction">The in or out <see cref="ParameterDirection"/> of the parameter.</param>
    /// <param name="size">The size of the parameter.</param>
    /// <param name="precision">The precision of the parameter.</param>
    /// <param name="scale">The scale of the parameter.</param>
    /// <returns>Returns <see cref="IFluentSqlBuilder"/>.</returns>
    IFluentSqlBuilder AddParameter(string name, object? value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null);

    /// <summary>
    /// Get the value of a parameter.
    /// </summary>
    /// <typeparam name="T">The type to cast the value to.</typeparam>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <returns>The value, note <see cref="DBNull.Value"/> is not returned, instead the value is returned as <see langword="null"/>.</returns>
    T GetValue<T>(string parameterName);
}