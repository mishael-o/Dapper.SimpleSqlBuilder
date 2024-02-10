namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// Global settings for the builders.
/// </summary>
public sealed class SimpleBuilderSettings
{
    internal const string DefaultDatabaseParameterNameTemplate = "p";
    internal const string DefaultDatabaseParameterPrefix = "@";
    internal const bool DefaultReuseParameters = false;
    internal const bool DefaultUseLowerCaseClauses = false;

    private static readonly object LockObject = new();

    /// <summary>
    /// Initializes static members of the <see cref="SimpleBuilderSettings"/> class.
    /// Explicit static constructor for beforefieldinit.
    /// </summary>
    static SimpleBuilderSettings()
    {
    }

    private SimpleBuilderSettings()
    {
    }

    /// <summary>
    /// Singleton instance to access the builders settings.
    /// </summary>
    public static SimpleBuilderSettings Instance { get; } = new();

    /// <summary>
    /// Gets the parameter name template used to create the parameter names for the generated SQL.
    /// </summary>
    public string DatabaseParameterNameTemplate { get; private set; } = DefaultDatabaseParameterNameTemplate;

    /// <summary>
    /// Gets the parameter prefix used in the rendered SQL.
    /// </summary>
    public string DatabaseParameterPrefix { get; private set; } = DefaultDatabaseParameterPrefix;

    /// <summary>
    /// Gets the value indicating whether to reuse parameters or not.
    /// </summary>
    public bool ReuseParameters { get; private set; } = DefaultReuseParameters;

    /// <summary>
    /// Get the value indicating whether SQL clauses should be in upper case or lower case.
    /// </summary>
    public bool UseLowerCaseClauses { get; private set; } = DefaultUseLowerCaseClauses;

    /// <summary>
    /// Configures the builders settings. Null or empty arguments will be ignored.
    /// </summary>
    /// <param name="parameterNameTemplate">
    /// The parameter name template used to create the parameter names for the generated SQL. The default is "p" so the parameter names will be generated as p0, p1, etc.
    /// <para>Example: If you set the template to "param" it will generate param0, param1, etc.</para>
    /// </param>
    /// <param name="parameterPrefix">
    /// The parameter prefix used in the rendered SQL. The default is "@", so you will get @p0, @p1, etc.
    /// <para>Example: If you set the parameter prefix to ":" it will generate :p0, :p1, etc.</para>
    /// </param>
    /// <param name="reuseParameters">
    /// The value indicating whether to reuse parameters or not. The default value is <see langword="false"/>.
    /// <para>Example: If set to <see langword="true"/> parameters are reused and if set <see langword="false"/> to they are not.</para>
    /// </param>
    /// <param name="useLowerCaseClauses">
    /// The value indicating whether to use lower case clauses for the fluent builder. The default value is <see langword="false"/> meaning SQL clauses will be in upper cases. i.e. SELECT, UPDATE, etc.
    /// <para>Example: If set to <see langword="true"/>, SQL clauses will be in lower cases i.e. select, update, etc.</para>
    /// <para>The <paramref name="useLowerCaseClauses"/> is only applicable to the fluent builder.</para>
    /// </param>
    public static void Configure(string? parameterNameTemplate = null, string? parameterPrefix = null, bool? reuseParameters = null, bool? useLowerCaseClauses = null)
    {
        lock (LockObject)
        {
            if (!string.IsNullOrWhiteSpace(parameterNameTemplate))
            {
                Instance.DatabaseParameterNameTemplate = parameterNameTemplate!;
            }

            if (!string.IsNullOrWhiteSpace(parameterPrefix))
            {
                Instance.DatabaseParameterPrefix = parameterPrefix!;
            }

            if (reuseParameters.HasValue)
            {
                Instance.ReuseParameters = reuseParameters.Value;
            }

            if (useLowerCaseClauses.HasValue)
            {
                Instance.UseLowerCaseClauses = useLowerCaseClauses.Value;
            }
        }
    }
}
