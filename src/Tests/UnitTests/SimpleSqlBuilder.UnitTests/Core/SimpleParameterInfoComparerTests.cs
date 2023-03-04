using System.Data;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

public class SimpleParameterInfoComparerTests
{
    [Theory]
    [MemberData(nameof(SimpleParameterInfoComparerTestCases.Equals_ParametersAreNotEqual_TestCases), MemberType = typeof(SimpleParameterInfoComparerTestCases))]
    public void Equals_ParametersAreNotEqual_ReturnsFalse(SimpleParameterInfo? param1, SimpleParameterInfo? param2)
    {
        // Arrange
        var sut = SimpleParameterInfoComparer.Instance;

        // Act
        var result = sut.Equals(param1, param2);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(SimpleParameterInfoComparerTestCases.Equals_ParametersAreEqual_TestCases), MemberType = typeof(SimpleParameterInfoComparerTestCases))]
    public void Equals_ParametersAreEqual_ReturnsTrue(SimpleParameterInfo? param1, SimpleParameterInfo? param2)
    {
        // Arrange
        var sut = SimpleParameterInfoComparer.Instance;

        // Act
        var result = sut.Equals(param1, param2);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [AutoData]
    public void GetHashCode_GetsHashCode_ReturnsInteger(SimpleParameterInfo parameterInfo)
    {
        // Arrange
        var sut = SimpleParameterInfoComparer.Instance;
        var expectedHashCode = HashCode.Combine(parameterInfo.Value, parameterInfo.Type, parameterInfo.DbType, parameterInfo.Direction, parameterInfo.Size, parameterInfo.Precision, parameterInfo.Scale);

        // Act
        var hashCode = sut.GetHashCode(parameterInfo);

        // Assert
        hashCode.Should().Be(expectedHashCode);
    }

    private static class SimpleParameterInfoComparerTestCases
    {
        public static IEnumerable<object?[]> Equals_ParametersAreNotEqual_TestCases()
        {
            yield return new object?[] { null, null };
            yield return new object?[] { new SimpleParameterInfo(null), null };
            yield return new object?[] { null, new SimpleParameterInfo(null) };
            yield return new object?[] { new SimpleParameterInfo(null), new SimpleParameterInfo(null) };
        }

        public static IEnumerable<object[]> Equals_ParametersAreEqual_TestCases()
        {
            var fixture = new Fixture();
            var size = fixture.Create<int>();
            var precision = fixture.Create<byte>();
            var scale = fixture.Create<byte>();

            var values = new (object Value, DbType DbType)[]
            {
                (new object(), DbType.Object),
                (10, DbType.Int16),
                ("value", DbType.String),
                (Guid.NewGuid(), DbType.Guid),
                (DateTime.Now, DbType.DateTime),
                (200f, DbType.Single),
                (false, DbType.Boolean)
            };

            for (int i = 0; i < values.Length; i++)
            {
                yield return new object[] { new SimpleParameterInfo(values[i].Value), new SimpleParameterInfo(values[i].Value) };
                yield return new object[] { new SimpleParameterInfo(values[i].Value, values[i].DbType), new SimpleParameterInfo(values[i].Value, values[i].DbType) };
                yield return new object[] { new SimpleParameterInfo(values[i].Value, values[i].DbType, size), new SimpleParameterInfo(values[i].Value, values[i].DbType, size) };
                yield return new object[] { new SimpleParameterInfo(values[i].Value, values[i].DbType, size, precision), new SimpleParameterInfo(values[i].Value, values[i].DbType, size, precision) };
                yield return new object[] { new SimpleParameterInfo(values[i].Value, values[i].DbType, size, precision, scale), new SimpleParameterInfo(values[i].Value, values[i].DbType, size, precision, scale) };
                yield return new object[] { new SimpleParameterInfo(null, values[i].Value, values[i].DbType, ParameterDirection.Input, size, precision, scale), new SimpleParameterInfo(null, values[i].Value, values[i].DbType, ParameterDirection.Input, size, precision, scale) };
            }
        }
    }
}
