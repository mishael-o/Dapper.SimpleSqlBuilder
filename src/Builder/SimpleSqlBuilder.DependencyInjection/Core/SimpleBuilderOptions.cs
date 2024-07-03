namespace Dapper.SimpleSqlBuilder.DependencyInjection;

/// <summary>
/// An options class for configuring the builders settings.
/// </summary>
public sealed class SimpleBuilderOptions
{
    /// <summary>
    /// The configuration section name for the builders settings.
    /// </summary>
    public const string ConfigurationSectionName = "SimpleSqlBuilder";

    private string databaseParameterNameTemplate = SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate;
    private string databaseParameterPrefix = SimpleBuilderSettings.DefaultDatabaseParameterPrefix;

    /// <summary>
    /// Gets or sets the parameter name template used to create the parameter names for the generated SQL. The default is <c>p</c>, so the parameter names will be generated as <c>p0</c>, <c>p1</c>, etc.
    /// <para>Example: If you set the template to <c>param</c>, it will generate <c>param0</c>, <c>param1</c>, etc.</para>
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when new value is <see langword="null"/>, <see cref="string.Empty"/>, or contains only white-space.</exception>
    public string DatabaseParameterNameTemplate
    {
        get => databaseParameterNameTemplate;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"'{nameof(DatabaseParameterNameTemplate)}' cannot be null, empty, or white-space.", nameof(DatabaseParameterNameTemplate));
            }

            databaseParameterNameTemplate = value;
        }
    }

    /// <summary>
    /// Gets or sets the parameter prefix used in the rendered SQL. The default is <c>@</c>, so you will get <c>@p0</c>, <c>@p1</c>, etc.
    /// <para>Example: If you set the parameter prefix to <c>:</c>, it will generate <c>:p0</c>, <c>:p1</c>, etc.</para>
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when new value is <see langword="null"/>, <see cref="string.Empty"/>, or contains only white-space.</exception>
    public string DatabaseParameterPrefix
    {
        get => databaseParameterPrefix;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"'{nameof(DatabaseParameterPrefix)}' cannot be null, empty, or white-space.", nameof(DatabaseParameterPrefix));
            }

            databaseParameterPrefix = value;
        }
    }

    /// <summary>
    /// Gets or sets the value indicating whether to reuse parameters or not. The default value is <see langword="false"/>.
    /// <para>Example: If set to <see langword="true"/> parameters are reused and if set <see langword="false"/> to they are not.</para>
    /// </summary>
    public bool ReuseParameters { get; set; } = SimpleBuilderSettings.DefaultReuseParameters;

    /// <summary>
    /// Get the value indicating whether SQL clauses should be in upper case or lower case. The default value is <see langword="false"/> meaning SQL clauses will be in upper cases. i.e. <c>SELECT</c>, <c>UPDATE</c>, etc.
    /// <para>Example: If set to <see langword="true"/>, SQL clauses will be in lower cases i.e. <c>select</c>, <c>update</c>, etc.</para>
    /// </summary>
    public bool UseLowerCaseClauses { get; set; } = SimpleBuilderSettings.DefaultUseLowerCaseClauses;
}
