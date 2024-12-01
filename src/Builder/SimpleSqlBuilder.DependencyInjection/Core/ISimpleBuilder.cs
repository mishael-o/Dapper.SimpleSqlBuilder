using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.DependencyInjection;

/// <summary>
/// An interface that defines the builder factory type.
/// </summary>
public interface ISimpleBuilder
{
    /// <summary>
    /// A method to create a builder instance.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <param name="parameterPrefix">The value to override the <see cref="SimpleBuilderOptions.DatabaseParameterPrefix"/> value.</param>
    /// <param name="reuseParameters">The value to override the <see cref="SimpleBuilderOptions.ReuseParameters"/> value.</param>
    /// <returns>A new instance of <see cref="Builder"/>.</returns>
    Builder Create(FormattableString? formattable = null, string? parameterPrefix = null, bool? reuseParameters = null);

#if NET6_0_OR_GREATER
    /// <summary>
    /// A method to create a builder instance.
    /// </summary>
    /// <param name="handler">The <see cref="BuilderFactoryInterpolatedStringHandler"/> to use for creating the builder.</param>
    /// <returns>A new instance of <see cref="Builder"/>.</returns>
    Builder Create([System.Runtime.CompilerServices.InterpolatedStringHandlerArgument("")] ref BuilderFactoryInterpolatedStringHandler handler);
#endif

    /// <summary>
    /// A method to create a fluent builder instance.
    /// </summary>
    /// <param name="parameterPrefix">The value to override the <see cref="SimpleBuilderOptions.DatabaseParameterPrefix"/> value.</param>
    /// <param name="reuseParameters">The value to override the <see cref="SimpleBuilderOptions.ReuseParameters"/> value.</param>
    /// <param name="useLowerCaseClauses">The value to override the <see cref="SimpleBuilderOptions.UseLowerCaseClauses"/> value.</param>
    /// <returns>A new instance of <see cref="ISimpleFluentBuilderEntry"/>.</returns>
    ISimpleFluentBuilderEntry CreateFluent(string? parameterPrefix = null, bool? reuseParameters = null, bool? useLowerCaseClauses = null);
}
