using System.Data;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

public class SimpleParameterInfoTests
{
    [Theory]
    [AutoData]
    public void Constructor_InitialisesSimpleParameterInfo_ReturnsSimpleParameterInfo(DbType dbType, int size, byte precision, byte scale)
    {
        //Act
        var sut = new SimpleParameterInfo(null, dbType, size, precision, scale);

        //Assert
        sut.Value.Should().BeNull();
        sut.Direction.Should().Be(ParameterDirection.Input);
        sut.DbType.Should().Be(dbType);
        sut.Size.Should().Be(size);
        sut.Precision.Should().Be(precision);
        sut.Scale.Should().Be(scale);
        sut.Type.Should().BeNull();
        sut.Name.Should().BeNull();
        sut.HasValue.Should().BeFalse();
        sut.HasName.Should().BeFalse();
    }

    [Theory]
    [AutoData]
    public void InternalConstructor_InitialisesSimpleParameterInfo_ReturnsSimpleParameterInfo(string name, object value, DbType dbType, ParameterDirection direction, int size, byte precision, byte scale)
    {
        //Act
        var sut = new SimpleParameterInfo(name, value, dbType, direction, size, precision, scale);

        //Assert
        sut.Value.Should().Be(value);
        sut.Direction.Should().Be(direction);
        sut.DbType.Should().Be(dbType);
        sut.Size.Should().Be(size);
        sut.Precision.Should().Be(precision);
        sut.Scale.Should().Be(scale);
        sut.Type.Should().Be(value.GetType());
        sut.Name.Should().Be(name);
        sut.HasValue.Should().BeTrue();
        sut.HasName.Should().BeTrue();
    }

    [Theory]
    [AutoData]
    public void SetName_SetsName_ReturnsVoid(string name, SimpleParameterInfo sut)
    {
        //Act
        sut.SetName(name);

        //Assert
        sut.Name.Should().Be(name);
    }

    [Theory]
    [AutoData]
    public void SetName_NameCannotBeChangedIfAlreadySet_ThrowsInvalidOperationException(string name, SimpleParameterInfo sut)
    {
        //Arrange
        sut.SetName(name);

        //Act
        var act = () => sut.SetName(name);

        //Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"{nameof(SimpleParameterInfo.Name)} has a value and cannot be changed.");
    }
}
