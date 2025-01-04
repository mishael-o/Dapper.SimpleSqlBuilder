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
        var parameterOptions = CreateParameterOptions(options.CurrentValue, parameterPrefix, reuseParameters);
        return new SqlBuilder(parameterOptions, formattable);
    }

#if NET6_0_OR_GREATER
    public Builder Create([System.Runtime.CompilerServices.InterpolatedStringHandlerArgument("")] ref BuilderFactoryInterpolatedStringHandler handler)
        => handler.GetBuilder();
#endif

    public ISimpleFluentBuilderEntry CreateFluent(string? parameterPrefix = null, bool? reuseParameters = null, bool? useLowerCaseClauses = null)
    {
        var parameterOptions = CreateParameterOptions(options.CurrentValue, parameterPrefix, reuseParameters);
        return new FluentSqlBuilder(
            parameterOptions,
            useLowerCaseClauses ?? options.CurrentValue.UseLowerCaseClauses);
    }

    private static ParameterOptions CreateParameterOptions(SimpleBuilderOptions options, string? parameterPrefix, bool? reuseParameters)
    {
        if (string.IsNullOrWhiteSpace(parameterPrefix))
        {
            parameterPrefix = options.DatabaseParameterPrefix;
        }

        return new(
            options.DatabaseParameterNameTemplate,
            parameterPrefix!,
            options.CollectionParameterFormat,
            reuseParameters ?? options.ReuseParameters);
    }
}
