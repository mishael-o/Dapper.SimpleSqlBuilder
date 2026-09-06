namespace Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class InlineAutoNSubstituteDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoNSubstituteDataAttribute(params object?[] values)
        : this(false, false, values)
    {
    }

    public InlineAutoNSubstituteDataAttribute(bool configureMembers, bool generateDelegates, params object?[] values)
        : base(new AutoNSubstituteDataAttribute(configureMembers, generateDelegates), values)
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
