using Dapper.SimpleSqlBuilder.UnitTestHelpers.XUnit;
using Xunit.Sdk;
using Xunit.v3;

[assembly: TestCollectionOrderer(typeof(DisplayNameOrderer))]
[assembly: Parallelization(Mode = ParallelMode.None)]
