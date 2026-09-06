using System.Data;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

public class SimpleParameterInfoTests
{
    [Theory]
    [AutoData]
    public void Constructor_InitialisesSimpleParameterInfoWithNoValue_ReturnsSimpleParameterInfo(DbType dbType, int size, byte precision, byte scale)
    {
        // Act
        var sut = new SimpleParameterInfo(null, dbType, size, precision, scale);

        // Assert
        sut.Value.ShouldBeNull();
        sut.Direction.ShouldBe(ParameterDirection.Input);
        sut.DbType.ShouldBe(dbType);
        sut.Size.ShouldBe(size);
        sut.Precision.ShouldBe(precision);
        sut.Scale.ShouldBe(scale);
        sut.Type.ShouldBeNull();
        sut.HasValue.ShouldBeFalse();
        sut.Reuse.ShouldBeTrue();
    }

    [Theory]
    [AutoData]
    public void Constructor_InitialisesSimpleParameterInfo_ReturnsSimpleParameterInfo(object value, DbType dbType, int size, byte precision, byte scale)
    {
        // Act
        var sut = new SimpleParameterInfo(value, dbType, size, precision, scale);

        // Assert
        sut.Value.ShouldBe(value);
        sut.Direction.ShouldBe(ParameterDirection.Input);
        sut.DbType.ShouldBe(dbType);
        sut.Size.ShouldBe(size);
        sut.Precision.ShouldBe(precision);
        sut.Scale.ShouldBe(scale);
        sut.Type.ShouldBe(value.GetType());
        sut.HasValue.ShouldBeTrue();
        sut.Reuse.ShouldBeTrue();
    }

    [Theory]
    [AutoData]
    public void Constructor_InitialisesSimpleParameterInfoWithReuseDisabled_ReturnsSimpleParameterInfo(object value, DbType dbType)
    {
        // Act
        var sut = new SimpleParameterInfo(value, dbType, reuse: false);

        // Assert
        sut.Value.ShouldBe(value);
        sut.DbType.ShouldBe(dbType);
        sut.Reuse.ShouldBeFalse();
    }
}
