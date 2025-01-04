using System.Collections;

namespace Dapper.SimpleSqlBuilder;

internal sealed class SqlFormatter : IFormatProvider, ICustomFormatter
{
    private readonly ParameterOptions parameterOptions;

    private int paramCount;
    private Dictionary<SimpleParameterInfo, string>? parameterDictionary;

    public SqlFormatter(ParameterOptions parameterOptions)
    {
        this.parameterOptions = parameterOptions;
        Parameters = new();
    }

    public DynamicParameters Parameters { get; private set; }

    public object? GetFormat(Type? formatType)
    {
        return typeof(ICustomFormatter).IsAssignableFrom(formatType)
            ? this
            : null;
    }

    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        => Format(arg, format);

    public string Format<T>(T value, string? format = null)
    {
        if (value is FormattableString formattableString)
        {
            return formattableString.ArgumentCount == 0
                ? formattableString.Format
                : string.Format(this, formattableString.Format, formattableString.GetArguments());
        }

        if (Constants.RawFormat.Equals(format, StringComparison.OrdinalIgnoreCase))
        {
            return value?.ToString() ?? string.Empty;
        }

        if ((value is null or not SimpleParameterInfo) && !parameterOptions.ReuseParameters)
        {
            return AddValueToParameters(value);
        }

        var parameterInfo = value as SimpleParameterInfo ?? new(value);
        return AddParameterInfoToParameters(parameterInfo);
    }

    public void Reset()
    {
        paramCount = 0;
        parameterDictionary?.Clear();
        Parameters = new();
    }

    private static bool IsEnumerableParameter<T>(T? value)
        => value is IEnumerable and not string;

    private string AddValueToParameters<T>(T value)
    {
        var parameterName = GetNextParameterName(IsEnumerableParameter(value));
        Parameters.Add(parameterName, value, direction: System.Data.ParameterDirection.Input);
        return AppendParameterPrefix(parameterName);
    }

    private string AddParameterInfoToParameters(SimpleParameterInfo parameterInfo)
    {
        parameterDictionary ??= new(SimpleParameterInfoComparer.Instance);

        if (parameterDictionary.TryGetValue(parameterInfo, out var dbPrefixedParameterName))
        {
            return dbPrefixedParameterName;
        }

        if (!parameterInfo.HasName)
        {
            parameterInfo.SetName(GetNextParameterName(IsEnumerableParameter(parameterInfo.Value)));
        }

        Parameters.Add(parameterInfo.Name!, parameterInfo.Value, parameterInfo.DbType, parameterInfo.Direction, parameterInfo.Size, parameterInfo.Precision, parameterInfo.Scale);

        dbPrefixedParameterName = AppendParameterPrefix(parameterInfo.Name!);

        if (parameterInfo.HasValue)
        {
            parameterDictionary[parameterInfo] = dbPrefixedParameterName;
        }

        return dbPrefixedParameterName;
    }

    private string GetNextParameterName(bool isEnumerable)
    {
        return isEnumerable
            ? string.Format(System.Globalization.CultureInfo.InvariantCulture, parameterOptions.CollectionParameterFormat, paramCount++)
            : $"{parameterOptions.ParameterNameTemplate}{paramCount++}";
    }

    private string AppendParameterPrefix(string parameterName)
        => parameterOptions.ParameterPrefix + parameterName;
}
