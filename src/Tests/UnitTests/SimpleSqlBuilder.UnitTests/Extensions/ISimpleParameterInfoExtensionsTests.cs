using System.Data;
using Dapper.SimpleSqlBuilder.Extensions;

namespace Dapper.SimpleSqlBuilder.UnitTests.Extensions;

public class ISimpleParameterInfoExtensionsTests
{
    [Theory]
    [AutoData]
    public void DefineParam_DefineParamCalledOnSimpleParameterInfo_ThrowsInvalidOperations(SimpleParameterInfo value)
    {
        //Act
        Action act = () => value.DefineParam(DbType.Int64);

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Value is already a {nameof(ISimpleParameterInfo)}*")
            .WithParameterName(nameof(value));
    }

    [Theory]
    [AutoData]
    public void DefineParam_CreateSimpleParameterInfo_ReturnsISimpleParameterInfo(
        object value, DbType dbType, int size, byte precision, byte scale)
    {
        //Act
        var valueParam = value.DefineParam(dbType, size, precision, scale);

        //Assert
        valueParam.Should().BeOfType<SimpleParameterInfo>();
        valueParam.Value.Should().Be(value);
        valueParam.DbType.Should().Be(dbType);
        valueParam.Size.Should().Be(size);
        valueParam.Precision.Should().Be(precision);
        valueParam.Scale.Should().Be(scale);
    }
}