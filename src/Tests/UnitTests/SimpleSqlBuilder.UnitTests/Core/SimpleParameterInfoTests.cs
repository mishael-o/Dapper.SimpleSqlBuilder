using System.Data;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

public class SimpleParameterInfoTests
{
    [Theory]
    [AutoData]
    public void Constructor_InitialisesSimpleParameterInfo_ReturnsSimpleParameterInfo(DbType dbType, int size, byte precision, byte scale)
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
        sut.Name.ShouldBeNull();
        sut.HasValue.ShouldBeFalse();
        sut.HasName.ShouldBeFalse();
    }

    [Theory]
    [AutoData]
    public void InternalConstructor_InitialisesSimpleParameterInfo_ReturnsSimpleParameterInfo(string name, object value, DbType dbType, int size, byte precision, byte scale)
    {
        // Act
        var sut = new SimpleParameterInfo(name, value, dbType, size, precision, scale);

        // Assert
        sut.Value.ShouldBe(value);
        sut.Direction.ShouldBe(ParameterDirection.Input);
        sut.DbType.ShouldBe(dbType);
        sut.Size.ShouldBe(size);
        sut.Precision.ShouldBe(precision);
        sut.Scale.ShouldBe(scale);
        sut.Type.ShouldBe(value.GetType());
        sut.Name.ShouldBe(name);
        sut.HasValue.ShouldBeTrue();
        sut.HasName.ShouldBeTrue();
    }

    [Theory]
    [AutoData]
    public void SetName_SetsName_ReturnsVoid(string name, SimpleParameterInfo sut)
    {
        // Act
        sut.SetName(name);

        // Assert
        sut.Name.ShouldBe(name);
    }

    [Theory]
    [AutoData]
    public void SetName_NameCannotBeChangedIfAlreadySet_ThrowsInvalidOperationException(string name, SimpleParameterInfo sut)
    {
        // Arrange
        sut.SetName(name);

        // Act
        var act = () => sut.SetName(name);

        // Assert
        act.ShouldThrow<InvalidOperationException>()
           .Message.ShouldBe($"{nameof(SimpleParameterInfo.Name)} has a value and cannot be changed.");
    }
}
