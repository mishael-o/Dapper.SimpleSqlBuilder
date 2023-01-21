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
}
