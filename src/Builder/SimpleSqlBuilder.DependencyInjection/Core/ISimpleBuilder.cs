using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.DependencyInjection;

/// <summary>
/// An interface that defines the simple builder factory type.
/// </summary>
public interface ISimpleBuilder
{
    /// <summary>
    /// A method to create a builder instance.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <param name="parameterPrefix">The value to override the <see cref="SimpleBuilderOptions.DatabaseParameterPrefix"/> value.</param>
    /// <param name="reuseParameters">The value to override the <see cref="SimpleBuilderOptions.ReuseParameters"/> value.</param>
    /// <returns><see cref="Builder"/>.</returns>
    Builder Create(FormattableString? formattable = null, string? parameterPrefix = null, bool? reuseParameters = null);

    /// <summary>
    /// A method to create a fluent builder instance.
    /// </summary>
    /// <param name="parameterPrefix">The value to override the <see cref="SimpleBuilderOptions.DatabaseParameterPrefix"/> value.</param>
    /// <param name="reuseParameters">The value to override the <see cref="SimpleBuilderOptions.ReuseParameters"/> value.</param>
    /// <param name="useLowerCaseClauses">The value to override the <see cref="SimpleBuilderOptions.UseLowerCaseClauses"/> value.</param>
    /// <returns><see cref="ISimpleFluentBuilderEntry"/>.</returns>
    ISimpleFluentBuilderEntry CreateFluent(string? parameterPrefix = null, bool? reuseParameters = null, bool? useLowerCaseClauses = null);
}
