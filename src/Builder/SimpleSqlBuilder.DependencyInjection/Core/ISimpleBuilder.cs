using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.DependencyInjection;

/// <summary>
/// An interface that defines the simple builder type or contract.
/// </summary>
public interface ISimpleBuilder
{
    /// <summary>
    /// A method to create a builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString">formattable string</see>.</param>
    /// <param name="parameterPrefix">The parameter prefix to override the <see cref="SimpleBuilderSettings.DatabaseParameterPrefix">default value</see>.</param>
    /// <param name="reuseParameters">The boolean value to override the <see cref="SimpleBuilderSettings.ReuseParameters"> default value</see>/>.</param>
    /// <returns>Returns a <see cref="SimpleBuilderBase"/>.</returns>
    SimpleBuilderBase Create(FormattableString? formattable = null, string? parameterPrefix = null, bool? reuseParameters = null);

    /// <summary>
    /// A static method to create a fluent builder instance.
    /// </summary>
    /// <param name="parameterPrefix">The parameter prefix to override the <see cref="SimpleBuilderSettings.DatabaseParameterPrefix">default value</see>.</param>
    /// <param name="reuseParameters">The boolean value to override the <see cref="SimpleBuilderSettings.ReuseParameters"> default value</see>.</param>
    /// <param name="useLowerCaseClauses">The boolean value to override the <see cref="SimpleBuilderSettings.UseLowerCaseClauses">default value</see>.</param>
    /// <returns>Returns a <see cref="ISimpleFluentBuilderEntry"/>.</returns>
    ISimpleFluentBuilderEntry CreateFluent(string? parameterPrefix = null, bool? reuseParameters = null, bool? useLowerCaseClauses = null);
}
