using Dapper.SimpleSqlBuilder.FluentBuilder;
using Microsoft.Extensions.Options;

namespace Dapper.SimpleSqlBuilder.DependencyInjection;

internal sealed class SimpleBuilderFactory : ISimpleBuilder
{
    private readonly IOptionsMonitor<SimpleBuilderOptions> options;

    public SimpleBuilderFactory(IOptionsMonitor<SimpleBuilderOptions> options)
    {
        this.options = options;
    }

    public Builder Create(FormattableString? formattable = null, string? parameterPrefix = null, bool? reuseParameters = null)
    {
        if (string.IsNullOrWhiteSpace(parameterPrefix))
        {
            parameterPrefix = options.CurrentValue.DatabaseParameterPrefix;
        }

        reuseParameters ??= options.CurrentValue.ReuseParameters;

        return new SqlBuilder(options.CurrentValue.DatabaseParameterNameTemplate, parameterPrefix, reuseParameters.Value, formattable);
    }

    public ISimpleFluentBuilderEntry CreateFluent(string? parameterPrefix = null, bool? reuseParameters = null, bool? useLowerCaseClauses = null)
    {
        if (string.IsNullOrWhiteSpace(parameterPrefix))
        {
            parameterPrefix = options.CurrentValue.DatabaseParameterPrefix;
        }

        reuseParameters ??= options.CurrentValue.ReuseParameters;
        useLowerCaseClauses ??= options.CurrentValue.UseLowerCaseClauses;

        return new FluentSqlBuilder(options.CurrentValue.DatabaseParameterNameTemplate, parameterPrefix, reuseParameters.Value, useLowerCaseClauses.Value);
    }
}
