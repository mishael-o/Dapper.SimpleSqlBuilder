namespace Dapper.SimpleSqlBuilder;

internal sealed class SqlFormatter : IFormatProvider, ICustomFormatter
{
    private static readonly SimpleParameterInfoComparer Comparer = new();

    private readonly DynamicParameters parameters;
    private readonly string parameterNameTemplate;
    private readonly string parameterPrefix;
    private readonly bool reuseParameters;

    private int paramCount;
    private Dictionary<SimpleParameterInfo, string>? parameterDictionary;

    public SqlFormatter(DynamicParameters parameters, string parameterNameTemplate, string parameterPrefix, bool reuseParameters)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(parameters);
#else
        if (parameters is null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }
#endif

        this.parameters = parameters;
        this.parameterNameTemplate = parameterNameTemplate;
        this.parameterPrefix = parameterPrefix;
        this.reuseParameters = reuseParameters;
    }

    public object? GetFormat(Type? formatType)
    {
        return typeof(ICustomFormatter).IsAssignableFrom(formatType)
            ? this
            : null;
    }

    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        if (arg is FormattableString formattableString)
        {
            return formattableString.ArgumentCount == 0
                ? formattableString.Format
                : string.Format(this, formattableString.Format, formattableString.GetArguments());
        }

        if (Constants.RawFormat.Equals(format, StringComparison.OrdinalIgnoreCase))
        {
            return arg?.ToString() ?? string.Empty;
        }

        if ((arg is null or not SimpleParameterInfo) && !reuseParameters)
        {
            return AddValueToParameters(arg);
        }

        if (arg is SimpleParameterInfo parameterInfo)
        {
            return AddParameterInfoToParameters(parameterInfo);
        }

        parameterInfo = new SimpleParameterInfo(arg);
        return AddParameterInfoToParameters(parameterInfo);
    }

    private string AddValueToParameters(object? value)
    {
        var parameterName = GetNextParameterName();
        parameters.Add(parameterName, value, direction: System.Data.ParameterDirection.Input);
        return AppendParameterPrefix(parameterName);
    }

    private string AddParameterInfoToParameters(SimpleParameterInfo parameterInfo)
    {
        parameterDictionary ??= new(Comparer);

        if (parameterDictionary.TryGetValue(parameterInfo, out var dbPrefixedParameterName))
        {
            return dbPrefixedParameterName;
        }

        if (!parameterInfo.HasName)
        {
            parameterInfo.SetName(GetNextParameterName());
        }

        parameters.Add(parameterInfo.Name, parameterInfo.Value, parameterInfo.DbType, parameterInfo.Direction, parameterInfo.Size, parameterInfo.Precision, parameterInfo.Scale);

        dbPrefixedParameterName = AppendParameterPrefix(parameterInfo.Name!);

        if (parameterInfo.HasValue)
        {
            parameterDictionary[parameterInfo] = dbPrefixedParameterName;
        }

        return dbPrefixedParameterName;
    }

    private string GetNextParameterName()
        => $"{parameterNameTemplate}{paramCount++}";

    private string AppendParameterPrefix(string parameterName)
        => parameterPrefix + parameterName;
}
