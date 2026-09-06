using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests;

internal static class UnitTestSubstitutes
{
    public static Builder CreateBuilder(out IBuilderFormatter builderFormatter)
    {
        var builder = Substitute.For<Builder, IBuilderFormatter>();
        builderFormatter = (IBuilderFormatter)builder;
        return builder;
    }

    public static IFluentBuilder CreateFluentBuilder(out IFluentBuilderFormatter fluentBuilderFormatter)
    {
        var fluentBuilder = Substitute.For<IFluentBuilder, IFluentBuilderFormatter>();
        fluentBuilderFormatter = (IFluentBuilderFormatter)fluentBuilder;
        return fluentBuilder;
    }
}
