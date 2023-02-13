using System.Data;

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the fluent SQL builder type.
/// </summary>
public interface IFluentSqlBuilder : ISqlBuilder, IFluentBuilder
{
    /// <summary>
    /// Adds a parameter to the dynamic <see cref="ISqlBuilder.Parameters">parameters</see> list.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <param name="dbType">The <see cref="DbType"/> of the parameter.</param>
    /// <param name="direction">The in or out <see cref="ParameterDirection"/> of the parameter.</param>
    /// <param name="size">The size of the parameter.</param>
    /// <param name="precision">The precision of the parameter.</param>
    /// <param name="scale">The scale of the parameter.</param>
    void AddParameter(string name, object? value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null);

    /// <summary>
    /// Gets the value of a parameter.
    /// </summary>
    /// <typeparam name="T">The type to cast the value to.</typeparam>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <returns>The value. Note <see cref="DBNull.Value"/> is not returned, instead the value is returned as <see langword="null"/>.</returns>
    T GetValue<T>(string parameterName);
}
