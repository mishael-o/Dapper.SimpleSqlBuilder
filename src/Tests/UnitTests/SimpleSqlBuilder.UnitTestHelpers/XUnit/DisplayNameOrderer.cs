using Xunit.Abstractions;

namespace Dapper.SimpleSqlBuilder.UnitTestHelpers.XUnit;

public class DisplayNameOrderer : ITestCollectionOrderer
{
    public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
        => testCollections.OrderBy(collection => collection.DisplayName, StringComparer.OrdinalIgnoreCase);
}
