using System.Collections;
using System.Data;
using System.Text;

namespace Dapper.SimpleSqlBuilder;

internal sealed class SqlFormatter : IFormatProvider, ICustomFormatter
{
    private readonly ParameterOptions parameterOptions;

    private int paramCount;
    private Dictionary<ParameterKey, string>? parameterDictionary;

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

    public string Format(string? format, object? value, IFormatProvider? formatProvider)
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

        if (value is SimpleParameterInfo parameterInfo)
        {
            return AddParameterInfo(parameterInfo);
        }

        if (value is null || !parameterOptions.ReuseParameters)
        {
            return AppendParameterPrefix(AddValue(value));
        }

        return AddReusableValue(value);
    }

    public void FormatTo<T>(StringBuilder destination, T? value, string? format = null)
    {
        if (value is FormattableString formattableString)
        {
            if (formattableString.ArgumentCount == 0)
            {
                destination.Append(formattableString.Format);
            }
            else
            {
                destination.AppendFormat(this, formattableString.Format, formattableString.GetArguments());
            }

            return;
        }

        if (Constants.RawFormat.Equals(format, StringComparison.OrdinalIgnoreCase))
        {
            destination.Append(value?.ToString());
            return;
        }

        if (value is SimpleParameterInfo parameterInfo)
        {
            destination.Append(AddParameterInfo(parameterInfo));
            return;
        }

        if (value is null || !parameterOptions.ReuseParameters)
        {
            var parameterName = AddValue(value);

            destination
                .Append(parameterOptions.ParameterPrefix)
                .Append(parameterName);

            return;
        }

        destination.Append(AddReusableValue(value));
    }

    public void Reset()
    {
        paramCount = 0;
        parameterDictionary?.Clear();
        Parameters = new();
    }

    private static bool IsEnumerableParameter<T>(T? value)
        => value is IEnumerable and not string;

    private string AddValue<T>(T? value)
    {
        var parameterName = GetNextParameterName(IsEnumerableParameter(value));
        Parameters.Add(parameterName, value, direction: ParameterDirection.Input);
        return parameterName;
    }

    private string AddReusableValue<T>(T value)
    {
        // Boxed once here so the key and Dapper share the same box.
        object boxedValue = value!;
        var key = ParameterKey.Create(boxedValue);

        if (parameterDictionary?.TryGetValue(key, out var prefixedParameterName) is true)
        {
            return prefixedParameterName;
        }

        var parameterName = GetNextParameterName(IsEnumerableParameter(value));
        Parameters.Add(parameterName, boxedValue, direction: ParameterDirection.Input);

        prefixedParameterName = AppendParameterPrefix(parameterName);
        parameterDictionary ??= [];
        parameterDictionary[key] = prefixedParameterName;

        return prefixedParameterName;
    }

    private string AddParameterInfo(SimpleParameterInfo parameterInfo)
    {
        // Reuse never applies to a parameter with no value, since such a parameter matches nothing.
        if (!parameterInfo.HasValue || !parameterInfo.Reuse)
        {
            return AddParameter(parameterInfo);
        }

        var key = parameterInfo.ToKey();

        if (parameterDictionary?.TryGetValue(key, out var prefixedParameterName) is true)
        {
            return prefixedParameterName;
        }

        prefixedParameterName = AddParameter(parameterInfo);
        parameterDictionary ??= [];
        parameterDictionary[key] = prefixedParameterName;

        return prefixedParameterName;

        string AddParameter(SimpleParameterInfo parameterInfo)
        {
            var parameterName = GetNextParameterName(IsEnumerableParameter(parameterInfo.Value));
            Parameters.Add(parameterName, parameterInfo.Value, parameterInfo.DbType, parameterInfo.Direction, parameterInfo.Size, parameterInfo.Precision, parameterInfo.Scale);
            return AppendParameterPrefix(parameterName);
        }
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
