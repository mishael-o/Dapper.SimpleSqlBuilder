using System.Data;
using System.Reflection;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

public class ParameterKeyTests
{
    /// <summary>
    /// The <see cref="ParameterKey"/> fields that make two parameters distinct.
    /// </summary>
    private static readonly string[] KeyFields =
        ["value", "type", "dbType", "size", "precision", "scale"];

    /// <summary>
    /// The <see cref="SimpleParameterInfo"/> members that are deliberately not part of the key.
    /// </summary>
    /// <remarks>
    /// <c>Direction</c> is excluded because it is always <see cref="ParameterDirection.Input"/> and nothing
    /// can set it otherwise, so it could only ever compare equal. It has to be added back to the key if
    /// output parameters ever become expressible. <c>HasValue</c> is derived from the value, and
    /// <c>Reuse</c> is intent rather than parameter shape.
    /// </remarks>
    private static readonly string[] ExcludedProperties =
        ["Direction", "HasValue", "Reuse"];

    [Fact]
    public void ParameterKey_FieldsMatchTheDocumentedIdentity_ReturnsTrue()
    {
        // Arrange
        // Pins the key's own fields, so dropping one cannot silently widen what counts as the same
        // parameter. Without this, the test below would keep passing while describing a key that no
        // longer exists.
        var expected = KeyFields.OrderBy(name => name, StringComparer.OrdinalIgnoreCase);

        // Act
        var actual = typeof(ParameterKey)
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Select(field => field.Name)
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void ToKey_EveryPropertyIsAccountedFor_ReturnsTrue()
    {
        // Arrange
        // Fails when a property is added to SimpleParameterInfo, forcing a decision about whether it
        // forms part of the reuse identity. Leaving one out of the key would make different parameters
        // share a placeholder, which produces wrong SQL rather than an error.
        // The key names its fields in camel case and the properties are pascal case, so both sides are
        // upper cased before comparing.
        var expected = KeyFields
            .Select(name => name.ToUpperInvariant())
            .OrderBy(name => name, StringComparer.Ordinal);

        // Act
        var actual = typeof(SimpleParameterInfo)
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Select(property => property.Name)
            .Except(ExcludedProperties, StringComparer.Ordinal)
            .Select(name => name.ToUpperInvariant())
            .OrderBy(name => name, StringComparer.Ordinal);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(ParameterKeyTestCases.Equals_KeysAreEqual_TestCases), MemberType = typeof(ParameterKeyTestCases))]
    public void Equals_KeysAreEqual_ReturnsTrue(SimpleParameterInfo param1, SimpleParameterInfo param2)
    {
        // Arrange
        var key1 = param1.ToKey();
        var key2 = param2.ToKey();

        // Act
        var result = key1.Equals(key2);

        // Assert
        result.ShouldBeTrue();
        key1.GetHashCode().ShouldBe(key2.GetHashCode());
    }

    [Theory]
    [MemberData(nameof(ParameterKeyTestCases.Equals_KeysAreNotEqual_TestCases), MemberType = typeof(ParameterKeyTestCases))]
    public void Equals_KeysAreNotEqual_ReturnsFalse(SimpleParameterInfo param1, SimpleParameterInfo param2)
    {
        // Arrange
        var key1 = param1.ToKey();
        var key2 = param2.ToKey();

        // Act
        var result = key1.Equals(key2);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Equals_ValueKeyMatchesParameterInfoKeyWithoutMetadata_ReturnsTrue()
    {
        // Arrange
        const int value = 10;

        var key1 = ParameterKey.Create(value);
        var key2 = new SimpleParameterInfo(value).ToKey();

        // Act
        var result = key1.Equals(key2);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void Equals_ValueKeyDoesNotMatchParameterInfoKeyWithMetadata_ReturnsFalse()
    {
        // Arrange
        const int value = 10;

        var key1 = ParameterKey.Create(value);
        var key2 = new SimpleParameterInfo(value, DbType.Int32).ToKey();

        // Act
        var result = key1.Equals(key2);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Equals_BothParametersHaveNoValue_ReturnsFalse()
    {
        // Arrange
        // Valueless parameters must never share a placeholder, so their keys match nothing.
        var key1 = new SimpleParameterInfo(null).ToKey();
        var key2 = new SimpleParameterInfo(null).ToKey();

        // Act
        var result = key1.Equals(key2);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Equals_ParameterHasNoValueComparedToItself_ReturnsFalse()
    {
        // Arrange
        var key = new SimpleParameterInfo(null).ToKey();

        // Act
        var result = key.Equals(key);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Equals_DefaultKey_ReturnsFalse()
    {
        // Arrange
        // A default key must be inert rather than throwing when compared.
        var defaultKey = default(ParameterKey);
        var populatedKey = ParameterKey.Create(10);

        // Act & Assert
        defaultKey.Equals(default).ShouldBeFalse();
        defaultKey.Equals(populatedKey).ShouldBeFalse();
        populatedKey.Equals(defaultKey).ShouldBeFalse();
        Should.NotThrow(defaultKey.GetHashCode);
    }

    [Fact]
    public void Equals_ComparedToOtherType_ReturnsFalse()
    {
        // Arrange
        var key = ParameterKey.Create(10);

        // Act
        var result = key.Equals("not a key");

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_GetsHashCode_ReturnsCombinedComponents()
    {
        // Arrange
        var parameterInfo = new SimpleParameterInfo("value", DbType.String, 10, 2, 1);
        var expectedHashCode = HashCode.Combine(
            parameterInfo.Value,
            parameterInfo.Type,
            parameterInfo.DbType,
            parameterInfo.Size,
            parameterInfo.Precision,
            parameterInfo.Scale);

        // Act
        var hashCode = parameterInfo.ToKey().GetHashCode();

        // Assert
        hashCode.ShouldBe(expectedHashCode);
    }

    [Fact]
    public void Equals_ReferenceTypesComparedByValueEquality_ReturnsExpectedResult()
    {
        // Arrange
        // Arrays do not override Equals, so two equal-looking arrays are distinct parameters,
        // while the same instance is reused.
        var array = new[] { 1, 2, 3 };
        var equalArray = new[] { 1, 2, 3 };

        // Act
        var sameInstance = ParameterKey.Create(array).Equals(ParameterKey.Create(array));
        var equalContents = ParameterKey.Create(array).Equals(ParameterKey.Create(equalArray));

        // Assert
        sameInstance.ShouldBeTrue();
        equalContents.ShouldBeFalse();
    }

    private static class ParameterKeyTestCases
    {
        public static IEnumerable<object[]> Equals_KeysAreEqual_TestCases()
        {
            var fixture = new Fixture();
            var size = fixture.Create<int>();
            var precision = fixture.Create<byte>();
            var scale = fixture.Create<byte>();

            foreach (var (value, dbType) in Values())
            {
                yield return [new SimpleParameterInfo(value), new SimpleParameterInfo(value)];
                yield return [new SimpleParameterInfo(value, dbType), new SimpleParameterInfo(value, dbType)];
                yield return [new SimpleParameterInfo(value, dbType, size), new SimpleParameterInfo(value, dbType, size)];
                yield return [new SimpleParameterInfo(value, dbType, size, precision), new SimpleParameterInfo(value, dbType, size, precision)];
                yield return [new SimpleParameterInfo(value, dbType, size, precision, scale), new SimpleParameterInfo(value, dbType, size, precision, scale)];
            }
        }

        public static IEnumerable<object[]> Equals_KeysAreNotEqual_TestCases()
        {
            // Different value
            yield return [new SimpleParameterInfo(10), new SimpleParameterInfo(20)];
            yield return [new SimpleParameterInfo("value"), new SimpleParameterInfo("other")];

            // Same value, different type
            yield return [new SimpleParameterInfo(10), new SimpleParameterInfo(10L)];

            // Same value, different metadata
            yield return [new SimpleParameterInfo(10), new SimpleParameterInfo(10, DbType.Int32)];
            yield return [new SimpleParameterInfo(10, DbType.Int32), new SimpleParameterInfo(10, DbType.Int16)];
            yield return [new SimpleParameterInfo(10, DbType.Int32, 4), new SimpleParameterInfo(10, DbType.Int32, 8)];
            yield return [new SimpleParameterInfo(10, DbType.Int32, 4, 1), new SimpleParameterInfo(10, DbType.Int32, 4, 2)];
            yield return [new SimpleParameterInfo(10, DbType.Int32, 4, 1, 1), new SimpleParameterInfo(10, DbType.Int32, 4, 1, 2)];
        }

        private static (object Value, DbType DbType)[] Values()
        {
            return
            [
                (new object(), DbType.Object),
                (10, DbType.Int16),
                ("value", DbType.String),
                (Guid.NewGuid(), DbType.Guid),
                (DateTime.Now, DbType.DateTime),
                (200f, DbType.Single),
                (false, DbType.Boolean)
            ];
        }
    }
}
