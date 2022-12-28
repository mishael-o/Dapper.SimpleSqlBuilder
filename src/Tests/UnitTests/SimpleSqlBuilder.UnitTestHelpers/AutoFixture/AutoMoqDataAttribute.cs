namespace Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute(bool configureMembers = false, bool generateDelegates = false)
        : base(() => new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = configureMembers, GenerateDelegates = generateDelegates }))
    {
        ConfigureMembers = configureMembers;
        GenerateDelegates = generateDelegates;
    }

    public bool ConfigureMembers { get; }

    public bool GenerateDelegates { get; }
}
