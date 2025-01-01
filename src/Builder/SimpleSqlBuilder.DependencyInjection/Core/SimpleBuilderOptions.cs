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
    private string collectionParameterTemplateFormat = SimpleBuilderSettings.DefaultCollectionParameterTemplateFormat;

    /// <summary>
    /// Gets or sets the parameter name template used to create the parameter names for the generated SQL. The default is <c>p</c>, so the parameter names will be generated as <c>p0</c>, <c>p1</c>, etc.
    /// <para>Example: Setting the template to <c>param</c> will generate <c>param0</c>, <c>param1</c>, etc.</para>
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the new value is <see langword="null"/>, <see cref="string.Empty"/>, or contains only white-space.</exception>
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
            UpdateCollectionParameterFormat();
        }
    }

    /// <summary>
    /// Gets or sets the parameter prefix used in the rendered SQL. The default is <c>@</c>, so you will get <c>@p0</c>, <c>@p1</c>, etc.
    /// <para>Example: Setting the parameter prefix to <c>:</c> will generate <c>:p0</c>, <c>:p1</c>, etc.</para>
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the new value is <see langword="null"/>, <see cref="string.Empty"/>, or contains only white-space.</exception>
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
    /// Gets or sets the template format used to create collection parameter names for the generated SQL.
    /// <para>
    /// This template is used in conjunction with <see cref="DatabaseParameterNameTemplate">parameterNameTemplate</see> to create parameter names for collections in the generated SQL.<br/>
    /// The template must contain a single format placeholder <c>{0}</c> which will be replaced with an incrementing number.
    /// </para>
    /// <para>
    /// The default is <c>c{0}_</c>; so if the <see cref="DatabaseParameterNameTemplate">parameterNameTemplate</see> is <c>p</c>, the collection parameter names will be generated as <c>pc0_</c>, <c>pc1_</c>, etc.<br/>
    /// When expanded into multiple parameters by Dapper, <c>pc0_</c> will be expanded to <c>pc0_1</c>, <c>pc0_2</c>, etc.
    /// </para>
    /// Example: Setting the template to <c>col{0}</c> will generate <c>pcol0</c>, <c>pcol1</c>, etc.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the new value is <see langword="null"/>, <see cref="string.Empty"/>, white-space, or missing format placeholder.</exception>
    public string CollectionParameterTemplateFormat
    {
        get => collectionParameterTemplateFormat;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"'{nameof(CollectionParameterTemplateFormat)}' cannot be null, empty, or white-space.", nameof(CollectionParameterTemplateFormat));
            }

            if (!value.Contains("{0}", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"'{nameof(CollectionParameterTemplateFormat)}' must contain a format placeholder '{{0}}' for the index.", nameof(CollectionParameterTemplateFormat));
            }

            collectionParameterTemplateFormat = value;
            UpdateCollectionParameterFormat();
        }
    }

    /// <summary>
    /// Gets or sets the value indicating whether to reuse parameters or not. The default value is <see langword="false"/>.
    /// <para>Example: If set to <see langword="true"/>, parameters are reused; if set to <see langword="false"/>, they are not.</para>
    /// </summary>
    public bool ReuseParameters { get; set; } = SimpleBuilderSettings.DefaultReuseParameters;

    /// <summary>
    /// Gets or sets the value indicating whether SQL clauses should be in upper case or lower case. The default value is <see langword="false"/>, meaning SQL clauses will be in upper case (e.g., <c>SELECT</c>, <c>UPDATE</c>, etc.).
    /// <para>Example: If set to <see langword="true"/>, SQL clauses will be in lower case (e.g., <c>select</c>, <c>update</c>, etc.).</para>
    /// </summary>
    public bool UseLowerCaseClauses { get; set; } = SimpleBuilderSettings.DefaultUseLowerCaseClauses;

#if NET8_0_OR_GREATER
    internal System.Text.CompositeFormat CollectionParameterFormat { get; private set; }
        = System.Text.CompositeFormat.Parse(SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate + SimpleBuilderSettings.DefaultCollectionParameterTemplateFormat);
#else

    internal string CollectionParameterFormat { get; private set; }
        = SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate + SimpleBuilderSettings.DefaultCollectionParameterTemplateFormat;

#endif

    private void UpdateCollectionParameterFormat()
    {
#if NET8_0_OR_GREATER
        CollectionParameterFormat = System.Text.CompositeFormat.Parse(DatabaseParameterPrefix + CollectionParameterTemplateFormat);
#else
        CollectionParameterFormat = DatabaseParameterPrefix + CollectionParameterTemplateFormat;
#endif
    }
}
