using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// A static class that enables creation of builder instances.
/// </summary>
public static class SimpleBuilder
{
    /// <summary>
    /// A static method to create a builder instance.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <param name="parameterPrefix">The value to override the <see cref="SimpleBuilderSettings.DatabaseParameterPrefix"/> value.</param>
    /// <param name="reuseParameters">The value to override the <see cref="SimpleBuilderSettings.ReuseParameters"/> value.</param>
    /// <returns>A new instance of <see cref="Builder"/>.</returns>
    public static Builder Create(FormattableString? formattable = null, string? parameterPrefix = null, bool? reuseParameters = null)
    {
        if (string.IsNullOrWhiteSpace(parameterPrefix))
        {
            parameterPrefix = SimpleBuilderSettings.Instance.DatabaseParameterPrefix;
        }

        reuseParameters ??= SimpleBuilderSettings.Instance.ReuseParameters;

        return new SqlBuilder(
            SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate,
            parameterPrefix!,
            reuseParameters.Value,
            formattable);
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// A static method to create a builder instance.
    /// </summary>
    /// <param name="handler">The <see cref="BuilderInterpolatedStringHandler"/> to use for creating the builder.</param>
    /// <returns>A new instance of <see cref="Builder"/>.</returns>
    public static Builder Create(ref BuilderInterpolatedStringHandler handler)
        => handler.GetBuilder();
#endif

    /// <summary>
    /// A static method to create a fluent builder instance.
    /// </summary>
    /// <param name="parameterPrefix">The value to override the <see cref="SimpleBuilderSettings.DatabaseParameterPrefix"/> value.</param>
    /// <param name="reuseParameters">The value to override the <see cref="SimpleBuilderSettings.ReuseParameters"/> value.</param>
    /// <param name="useLowerCaseClauses">The value to override the <see cref="SimpleBuilderSettings.UseLowerCaseClauses"/> value.</param>
    /// <returns>A new instance of <see cref="ISimpleFluentBuilderEntry"/>.</returns>
    public static ISimpleFluentBuilderEntry CreateFluent(string? parameterPrefix = null, bool? reuseParameters = null, bool? useLowerCaseClauses = null)
    {
        if (string.IsNullOrWhiteSpace(parameterPrefix))
        {
            parameterPrefix = SimpleBuilderSettings.Instance.DatabaseParameterPrefix;
        }

        reuseParameters ??= SimpleBuilderSettings.Instance.ReuseParameters;
        useLowerCaseClauses ??= SimpleBuilderSettings.Instance.UseLowerCaseClauses;

        return new FluentSqlBuilder(
            SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate,
            parameterPrefix!,
            reuseParameters.Value,
            useLowerCaseClauses.Value);
    }
}
