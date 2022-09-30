using Microsoft.Extensions.Options;

namespace Dapper.SimpleSqlBuilder.DependencyInjection;

internal sealed class InternalSimpleBuilder : ISimpleBuilder
{
    private readonly IOptions<SimpleBuilderOptions> options;

    public InternalSimpleBuilder(IOptions<SimpleBuilderOptions> options)
    {
        this.options = options;
    }

    public SimpleBuilderBase Create(FormattableString? formattable = null, string? parameterPrefix = null, bool? reuseParameters = null)
    {
        if (string.IsNullOrWhiteSpace(parameterPrefix))
        {
            parameterPrefix = options.Value.DatabaseParameterPrefix;
        }

        reuseParameters ??= options.Value.ReuseParameters;

        return new SqlBuilder(options.Value.DatabaseParameterNameTemplate, parameterPrefix, reuseParameters.Value, formattable);
    }
}
