using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// A static class that enables you create a builder instance.
/// </summary>
public static class SimpleBuilder
{
    /// <summary>
    /// A static method to create a builder instance.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <param name="parameterPrefix">The parameter prefix to override the <see cref="SimpleBuilderSettings.DatabaseParameterPrefix">default value</see>.</param>
    /// <param name="reuseParameters">The boolean value to override the <see cref="SimpleBuilderSettings.ReuseParameters"> default value</see>.</param>
    /// <returns>Returns a <see cref="SimpleBuilderBase"/>.</returns>
    public static SimpleBuilderBase Create(FormattableString? formattable = null, string? parameterPrefix = null, bool? reuseParameters = null)
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

    /// <summary>
    /// A static method to create a fluent builder instance.
    /// </summary>
    /// <param name="parameterPrefix">The parameter prefix to override the <see cref="SimpleBuilderSettings.DatabaseParameterPrefix">default value</see>.</param>
    /// <param name="reuseParameters">The boolean value to override the <see cref="SimpleBuilderSettings.ReuseParameters"> default value</see>.</param>
    /// <param name="useLowerCaseClauses">The boolean value to override the <see cref="SimpleBuilderSettings.UseLowerCaseClauses">default value</see>.</param>
    /// <returns>Returns a <see cref="ISimpleFluentBuilderEntry"/>.</returns>
    public static ISimpleFluentBuilderEntry CreateFluent(string? parameterPrefix = null, bool? reuseParameters = null, bool? useLowerCaseClauses = null)
    {
        if (string.IsNullOrWhiteSpace(parameterPrefix))
        {
            parameterPrefix = SimpleBuilderSettings.Instance.DatabaseParameterPrefix;
        }

        reuseParameters ??= SimpleBuilderSettings.Instance.ReuseParameters;
        useLowerCaseClauses ??= SimpleBuilderSettings.Instance.UseLowerCaseClauses;

        return new FluentSqlBuilder(
            SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate,
            parameterPrefix!,
            reuseParameters.Value,
            useLowerCaseClauses.Value);
    }
}