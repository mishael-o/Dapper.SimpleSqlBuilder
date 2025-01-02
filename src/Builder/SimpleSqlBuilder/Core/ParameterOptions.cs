namespace Dapper.SimpleSqlBuilder;

internal sealed class ParameterOptions
{
    public ParameterOptions(
        string parameterNameTemplate,
        string parameterPrefix,
#if NET8_0_OR_GREATER
        System.Text.CompositeFormat collectionParameterFormat,
#else
        string collectionParameterFormat,
#endif
        bool reuseParameters)
    {
        ParameterNameTemplate = parameterNameTemplate;
        ParameterPrefix = parameterPrefix;
        CollectionParameterFormat = collectionParameterFormat;
        ReuseParameters = reuseParameters;
    }

    public string ParameterNameTemplate { get; }

    public string ParameterPrefix { get; }

#if NET8_0_OR_GREATER
    public System.Text.CompositeFormat CollectionParameterFormat { get; }
#else
    public string CollectionParameterFormat { get; }
#endif
    public bool ReuseParameters { get; }
}
