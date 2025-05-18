using System.Data;
using Dapper.SimpleSqlBuilder.Extensions;

namespace Dapper.SimpleSqlBuilder.UnitTests.Extensions;

public class SimpleParameterInfoExtensionsTests
{
    [Theory]
    [AutoData]
    public void DefineParam_GenericTypeIsSimpleParameterInfo_ThrowsInvalidOperations(SimpleParameterInfo value)
    {
        // Act
        Action act = () => value.DefineParam(DbType.Int64);

        // Assert
        var exception = act.ShouldThrow<ArgumentException>();
        exception.Message.ShouldStartWith($"Value is already a {nameof(ISimpleParameterInfo)}");
        exception.ParamName.ShouldBe(nameof(value));
    }

    [Theory]
    [AutoData]
    public void DefineParam_CreatesSimpleParameterInfo_ReturnsISimpleParameterInfo(object value, DbType dbType, int size, byte precision, byte scale)
    {
        // Act
        var valueParam = value.DefineParam(dbType, size, precision, scale);

        // Assert
        valueParam.ShouldBeOfType<SimpleParameterInfo>();
        valueParam.Value.ShouldBe(value);
        valueParam.DbType.ShouldBe(dbType);
        valueParam.Size.ShouldBe(size);
        valueParam.Precision.ShouldBe(precision);
        valueParam.Scale.ShouldBe(scale);
    }
}
