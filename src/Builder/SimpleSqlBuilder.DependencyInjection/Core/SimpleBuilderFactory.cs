using Dapper.SimpleSqlBuilder.FluentBuilder;
using Microsoft.Extensions.Options;

namespace Dapper.SimpleSqlBuilder.DependencyInjection;

internal sealed class SimpleBuilderFactory : ISimpleBuilder
{
    private readonly IOptions<SimpleBuilderOptions> options;

    public SimpleBuilderFactory(IOptions<SimpleBuilderOptions> options)
    {
        this.options = options;
    }

    public Builder Create(FormattableString? formattable = null, string? parameterPrefix = null, bool? reuseParameters = null)
    {
        if (string.IsNullOrWhiteSpace(parameterPrefix))
        {
            parameterPrefix = options.Value.DatabaseParameterPrefix;
        }

        reuseParameters ??= options.Value.ReuseParameters;

        return new SqlBuilder(options.Value.DatabaseParameterNameTemplate, parameterPrefix, reuseParameters.Value, formattable);
    }

    public ISimpleFluentBuilderEntry CreateFluent(string? parameterPrefix = null, bool? reuseParameters = null, bool? useLowerCaseClauses = null)
    {
        if (string.IsNullOrWhiteSpace(parameterPrefix))
        {
            parameterPrefix = options.Value.DatabaseParameterPrefix;
        }

        reuseParameters ??= options.Value.ReuseParameters;
        useLowerCaseClauses ??= options.Value.UseLowerCaseClauses;

        return new FluentSqlBuilder(options.Value.DatabaseParameterNameTemplate, parameterPrefix, reuseParameters.Value, useLowerCaseClauses.Value);
    }
}
