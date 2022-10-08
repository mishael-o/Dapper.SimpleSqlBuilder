namespace Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoMoqDataAttribute(params object[] values)
        : base(new AutoMoqDataAttribute(), values)
    {
    }
}
