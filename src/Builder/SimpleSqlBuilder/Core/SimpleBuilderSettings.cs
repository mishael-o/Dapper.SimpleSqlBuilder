namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// Global settings for the simple sql builder.
/// </summary>
public static class SimpleBuilderSettings
{
    internal const string DefaultDatabaseParameterNameTemplate = "p";
    internal const string DefaultDatabaseParameterPrefix = "@";
    internal const bool DefaultReuseParameters = false;
    internal const bool DefaultUseLowerCaseClauses = false;

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
    /// Get the value indicating whether sql clauses should be in upper case (default) or lower case.
    /// </summary>
    public static bool UseLowerCaseClauses { get; private set; } = DefaultUseLowerCaseClauses;

    /// <summary>
    /// Configures the Simple builder settings.
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
    /// <param name="useLowerCaseClauses">
    /// The value indicating whether to use lower case clauses for the fluent builder. The default value is <see langword="false"/> meaning sql clauses will be in upper cases. i.e. SELECT, UPDATE, etc.
    /// <para>Example: If set to <see langword="true"/> sql clauses will be in lower cases i.e. select, update, etc.</para>
    /// </param>
    /// <exception cref="ArgumentException">Throws an <see cref="ArgumentException"/> when new <paramref name="parameterNameTemplate"/> or <paramref name="parameterPrefix"/> <see langword="null"/>, <see cref="string.Empty"/> contains only white space.</exception>
    public static void Configure(
        string parameterNameTemplate = DefaultDatabaseParameterNameTemplate,
        string parameterPrefix = DefaultDatabaseParameterPrefix,
        bool reuseParameters = DefaultReuseParameters,
        bool useLowerCaseClauses = DefaultUseLowerCaseClauses)
    {
        if (string.IsNullOrWhiteSpace(parameterNameTemplate))
        {
            throw new ArgumentException($"'{nameof(parameterNameTemplate)}' cannot be null or whitespace.", nameof(parameterNameTemplate));
        }

        if (string.IsNullOrWhiteSpace(parameterPrefix))
        {
            throw new ArgumentException($"'{nameof(parameterPrefix)}' cannot be null or whitespace.", nameof(parameterPrefix));
        }

        DatabaseParameterNameTemplate = parameterNameTemplate;
        DatabaseParameterPrefix = parameterPrefix;
        ReuseParameters = reuseParameters;
        UseLowerCaseClauses = useLowerCaseClauses;
    }
}
