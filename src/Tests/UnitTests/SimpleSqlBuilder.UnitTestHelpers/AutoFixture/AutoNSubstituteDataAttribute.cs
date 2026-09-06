namespace Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;

[AttributeUsage(AttributeTargets.Method)]
public sealed class AutoNSubstituteDataAttribute : AutoDataAttribute
{
    public AutoNSubstituteDataAttribute(bool configureMembers = false, bool generateDelegates = false)
        : base(() => new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = configureMembers, GenerateDelegates = generateDelegates }))
    {
        ConfigureMembers = configureMembers;
        GenerateDelegates = generateDelegates;
    }

    /// <summary>
    /// Exposes the constructor value, which is applied during construction to AutoNSubstituteCustomization.
    /// </summary>
    public bool ConfigureMembers { get; }

    /// <summary>
    /// Exposes the constructor value, which is applied during construction to AutoNSubstituteCustomization.
    /// </summary>
    public bool GenerateDelegates { get; }
}
