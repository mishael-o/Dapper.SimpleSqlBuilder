namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// Global settings for the builders.
/// </summary>
public sealed class SimpleBuilderSettings
{
    internal const string DefaultDatabaseParameterNameTemplate = "p";
    internal const string DefaultDatabaseParameterPrefix = "@";
    internal const string DefaultCollectionParameterTemplateFormat = "c{0}_";
    internal const bool DefaultReuseParameters = false;
    internal const bool DefaultUseLowerCaseClauses = false;

#if NET9_0_OR_GREATER
    private static readonly Lock LockObject = new();
#else
    private static readonly object LockObject = new();
#endif

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
    /// Gets the template format used to create collection parameter names for the generated SQL.
    /// </summary>
    public string CollectionParameterTemplateFormat { get; private set; } = DefaultCollectionParameterTemplateFormat;

    /// <summary>
    /// Gets the value indicating whether to reuse parameters or not.
    /// </summary>
    public bool ReuseParameters { get; private set; } = DefaultReuseParameters;

    /// <summary>
    /// Gets the value indicating whether SQL clauses should be in upper case or lower case.
    /// </summary>
    public bool UseLowerCaseClauses { get; private set; } = DefaultUseLowerCaseClauses;

#if NET8_0_OR_GREATER
    internal System.Text.CompositeFormat CollectionParameterFormat { get; private set; }
        = System.Text.CompositeFormat.Parse(DefaultDatabaseParameterNameTemplate + DefaultCollectionParameterTemplateFormat);
#else

    internal string CollectionParameterFormat { get; private set; }
        = DefaultDatabaseParameterNameTemplate + DefaultCollectionParameterTemplateFormat;

#endif

    /// <summary>
    /// Configures the builders settings. <see langword="null"/> or empty arguments will be ignored.
    /// </summary>
    /// <param name="parameterNameTemplate">
    /// The parameter name template used to create the parameter names for the generated SQL. The default is <c>p</c>, so the parameter names will be generated as <c>p0</c>, <c>p1</c>, etc.
    /// <para>Example: Setting the template to <c>param</c> will generate <c>param0</c>, <c>param1</c>, etc.</para>
    /// </param>
    /// <param name="parameterPrefix">
    /// The parameter prefix used in the rendered SQL. The default is <c>@</c>, so you will get <c>@p0</c>, <c>@p1</c>, etc.
    /// <para>Example: Setting the parameter prefix to <c>:</c> will generate <c>:p0</c>, <c>:p1</c>, etc.</para>
    /// </param>
    /// <param name="collectionParameterTemplateFormat">
    /// The template format used to create collection parameter names for the generated SQL.
    /// <para>
    /// This template is used in conjunction with <see cref="DatabaseParameterNameTemplate">parameterNameTemplate</see>  to create parameter names for collections in the generated SQL.<br/>
    /// The template must contain a single format placeholder <c>{0}</c> which will be replaced with an incrementing number.
    /// </para>
    /// <para>
    /// The default is <c>c{0}_</c>; so if the <see cref="DatabaseParameterNameTemplate">parameterNameTemplate</see>  is <c>p</c>, the collection parameter names will be generated as <c>pc0_</c>, <c>pc1_</c>, etc.<br/>
    /// When expanded into multiple parameters by Dapper, <c>pc0_</c> will be expanded to <c>pc0_1</c>, <c>pc0_2</c>, etc.
    /// </para>
    /// Example: Setting the template to <c>col{0}</c> will generate <c>pcol0</c>, <c>pcol1</c>, etc.
    /// </param>
    /// <param name="reuseParameters">
    /// The value indicating whether to reuse parameters or not. The default value is <see langword="false"/>.
    /// <para>Example: If set to <see langword="true"/>, parameters are reused; if set to <see langword="false"/>, they are not.</para>
    /// </param>
    /// <param name="useLowerCaseClauses">
    /// The value indicating whether to use lower case clauses for the fluent builder. The default value is <see langword="false"/>, meaning SQL clauses will be in upper case (e.g., <c>SELECT</c>, <c>UPDATE</c>, etc.).
    /// <para>Example: If set to <see langword="true"/>, SQL clauses will be in lower case (e.g., <c>select</c>, <c>update</c>, etc.).</para>
    /// <para>The <paramref name="useLowerCaseClauses"/> is only applicable to the fluent builder.</para>
    /// </param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="collectionParameterTemplateFormat"/> is missing format placeholder.</exception>
    public static void Configure(
        string? parameterNameTemplate = null,
        string? parameterPrefix = null,
        string? collectionParameterTemplateFormat = null,
        bool? reuseParameters = null,
        bool? useLowerCaseClauses = null)
    {
        lock (LockObject)
        {
            bool updateCollectionFormat = false;

            if (!string.IsNullOrWhiteSpace(parameterNameTemplate))
            {
                Instance.DatabaseParameterNameTemplate = parameterNameTemplate!;
                updateCollectionFormat = true;
            }

            if (!string.IsNullOrWhiteSpace(parameterPrefix))
            {
                Instance.DatabaseParameterPrefix = parameterPrefix!;
            }

            if (!string.IsNullOrWhiteSpace(collectionParameterTemplateFormat))
            {
#pragma warning disable CA2249 // Consider using 'string.Contains' instead of 'string.IndexOf'. Suppressing for .NET Framework, as it does not have a '.Contains' overload that takes StringComparison.
                if (collectionParameterTemplateFormat!.IndexOf("{0}", StringComparison.OrdinalIgnoreCase) < 0)
#pragma warning restore CA2249
                {
                    throw new ArgumentException($"'{nameof(collectionParameterTemplateFormat)}' must contain a format placeholder '{{0}}' for the index.", nameof(collectionParameterTemplateFormat));
                }

                Instance.CollectionParameterTemplateFormat = collectionParameterTemplateFormat!;
                updateCollectionFormat = true;
            }

            if (reuseParameters.HasValue)
            {
                Instance.ReuseParameters = reuseParameters.Value;
            }

            if (useLowerCaseClauses.HasValue)
            {
                Instance.UseLowerCaseClauses = useLowerCaseClauses.Value;
            }

            if (updateCollectionFormat)
            {
                UpdateCollectionParameterFormat();
            }
        }
    }

    private static void UpdateCollectionParameterFormat()
    {
#if NET8_0_OR_GREATER
        Instance.CollectionParameterFormat = System.Text.CompositeFormat.Parse(Instance.DatabaseParameterNameTemplate + Instance.CollectionParameterTemplateFormat);
#else
        Instance.CollectionParameterFormat = Instance.DatabaseParameterNameTemplate + Instance.CollectionParameterTemplateFormat;
#endif
    }
}
