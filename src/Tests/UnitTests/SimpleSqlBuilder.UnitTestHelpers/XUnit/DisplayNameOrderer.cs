using Xunit.Sdk;
using Xunit.v3;

namespace Dapper.SimpleSqlBuilder.UnitTestHelpers.XUnit;

public class DisplayNameOrderer : ITestCollectionOrderer
{
    public IReadOnlyCollection<TTestCollection> OrderTestCollections<TTestCollection>(IReadOnlyCollection<TTestCollection> testCollections)
        where TTestCollection : ITestCollection
        => [.. testCollections
            .OrderBy(collection => collection.TestCollectionDisplayName.IndexOf("~ Run Last", StringComparison.Ordinal) >= 0)
            .ThenBy(collection => collection.TestCollectionDisplayName, StringComparer.OrdinalIgnoreCase)];
}
