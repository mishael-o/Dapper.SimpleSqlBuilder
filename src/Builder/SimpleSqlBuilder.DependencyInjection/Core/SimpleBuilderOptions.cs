﻿namespace Dapper.SimpleSqlBuilder.DependencyInjection;

/// <summary>
/// An options class for configuring the Simple Builder settings.
/// </summary>
public sealed class SimpleBuilderOptions
{
    private string databaseParameterNameTemplate = SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate;
    private string databaseParameterPrefix = SimpleBuilderSettings.DefaultDatabaseParameterPrefix;

    /// <summary>
    /// Gets or sets the parameter name template used to create the parameter names for the generated SQL. The default is "p" so the parameter names will be generated as p0, p1, etc.
    /// <para>Example: If you set the template to "param" it will generate param0, param1, etc.</para>
    /// </summary>
    /// <exception cref="ArgumentException">Throws an <see cref="ArgumentException"/> when new value is <see langword="null"/>, <see cref="string.Empty"/> contains only white space.</exception>
    public string DatabaseParameterNameTemplate
    {
        get => databaseParameterNameTemplate;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));
            }

            databaseParameterNameTemplate = value;
        }
    }

    /// <summary>
    /// Gets or sets the parameter prefix used in the rendered SQL. The default is "@", so you will get @p0, @p1, etc.
    /// <para>Example: If you set the parameter prefix to ":" it will generate :p0, :p1, etc.</para>
    /// </summary>
    /// <exception cref="ArgumentException">Throws an <see cref="ArgumentException"/> when new value is <see langword="null"/>, <see cref="string.Empty"/> contains only white space.</exception>
    public string DatabaseParameterPrefix
    {
        get => databaseParameterPrefix;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));
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
    /// Get the value indicating whether SQL clauses should be in upper case or lower case. The default value is <see langword="false"/> meaning SQL clauses will be in upper cases. i.e. SELECT, UPDATE, etc.
    /// <para>Example: If set to <see langword="true"/>, SQL clauses will be in lower cases i.e. select, update, etc.</para>
    /// </summary>
    public bool UseLowerCaseClauses { get; set; } = SimpleBuilderSettings.DefaultUseLowerCaseClauses;
}
