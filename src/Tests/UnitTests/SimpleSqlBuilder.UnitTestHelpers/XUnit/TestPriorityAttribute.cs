namespace Dapper.SimpleSqlBuilder.UnitTestHelpers.XUnit;

[AttributeUsage(AttributeTargets.Method)]
public sealed class TestPriorityAttribute : Attribute
{
    public TestPriorityAttribute(int priority)
        => Priority = priority;

    public int Priority { get; }
}
