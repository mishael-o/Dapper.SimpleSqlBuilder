namespace Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoMoqDataAttribute(params object?[] values)
        : this(false, false, values)
    {
    }

    public InlineAutoMoqDataAttribute(bool configureMembers, bool generateDelegates, params object?[] values)
        : base(new AutoMoqDataAttribute(configureMembers, generateDelegates), values)
    {
        ConfigureMembers = configureMembers;
        GenerateDelegates = generateDelegates;
    }

    public bool ConfigureMembers { get; }

    public bool GenerateDelegates { get; }
}
