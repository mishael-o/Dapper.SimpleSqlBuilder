namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// Global settings for the simple sql builder.
/// </summary>
public static class SimpleBuilderSettings
{
    internal const string DefaultDatabaseParameterNameTemplate = "p";
    internal const string DefaultDatabaseParameterPrefix = "@";
    internal const bool DefaultReuseParameters = false;

    /// <summary>
    /// Gets the parameter name template used to create the parameter names for the generated sql.
    /// </summary>
    public static string DatabaseParameterNameTemplate { get; private set; } = DefaultDatabaseParameterNameTemplate;

    /// <summary>
    /// Gets the parameter prefix used in the rendered sql.
    /// </summary>
    public static string DatabaseParameterPrefix { get; private set; } = DefaultDatabaseParameterPrefix;

    /// <summary>
    /// Gets the value indicating whether to reuse parameters or not.
    /// </summary>
    public static bool ReuseParameters { get; private set; } = DefaultReuseParameters;

    /// <summary>
    /// Configures the Simple builder settings. Null or empty arguments will be ignored.
    /// </summary>
    /// <param name="parameterNameTemplate">
    /// The parameter name template used to create the parameter names for the generated sql. The default is "p" so the parameter names will be generated as p0, p1, etc.
    /// <para>Example: If you set the template to "param" it will generate param0, param1, etc.</para>
    /// </param>
    /// <param name="parameterPrefix">
    /// The parameter prefix used in the rendered sql. The default is "@", so you will get @p0, @p1, etc.
    /// <para>Example: If you set the parameter prefix to ":" it will generate :p0, :p1, etc.</para>
    /// </param>
    /// <param name="reuseParameters">
    /// The value indicating whether to reuse parameters or not. The default value is <see langword="false"/>.
    /// <para>Example: If set to <see langword="true"/> parameters are reused and if set <see langword="false"/> to they are not.</para>
    /// </param>
    public static void Configure(string? parameterNameTemplate = null, string? parameterPrefix = null, bool? reuseParameters = null)
    {
        if (!string.IsNullOrWhiteSpace(parameterNameTemplate))
        {
            DatabaseParameterNameTemplate = parameterNameTemplate!;
        }

        if (!string.IsNullOrWhiteSpace(parameterPrefix))
        {
            DatabaseParameterPrefix = parameterPrefix!;
        }

        if (reuseParameters.HasValue)
        {
            ReuseParameters = reuseParameters.Value;
        }
    }
}
