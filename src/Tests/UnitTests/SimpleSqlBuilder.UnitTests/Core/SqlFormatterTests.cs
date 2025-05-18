using System.Data;
using Dapper.SimpleSqlBuilder.Extensions;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

public class SqlFormatterTests
{
    [Fact]
    public void GetFormat_FormatterMatched_ReturnsObject()
    {
        // Arrange
        var sut = CreateSqlFormatter();
        var formatType = typeof(SqlFormatter);

        // Act
        var result = sut.GetFormat(formatType);

        // Assert
        result.ShouldBe(sut);
    }

    [Fact]
    public void GetFormat_NoFormtterMatched_ReturnsObject()
    {
        // Arrange
        var sut = CreateSqlFormatter();
        var formatType = typeof(int);

        // Act
        var result = sut.GetFormat(formatType);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void Format_FormatsFormattableString_ReturnsString()
    {
        // Arrange
        var model = new { Id = 10, TypeIds = new int[] { 1, 2, 3, 4 } };

        FormattableString innerFormattableString1 = $"SELECT Description FROM TYPE_TABLE WHERE Id IN {model.TypeIds}";
        FormattableString innerFormattableString2 = $"SELECT Id FROM TYPE_TABLE WHERE EXPIRED_DATE IS NULL";
        FormattableString formattableString = $@"
            SELECT *, ({innerFormattableString1})
            FROM TABLE x WHERE Id = {model.Id}
            AND TypeId IN {model.TypeIds}
            AND TypeId NOT IN ({innerFormattableString2})";

        var expectedResult = $@"
            SELECT *, (SELECT Description FROM TYPE_TABLE WHERE Id IN @pc0_)
            FROM TABLE x WHERE Id = @p1
            AND TypeId IN @pc2_
            AND TypeId NOT IN ({innerFormattableString2.Format})";

        var sut = CreateSqlFormatter();

        // Act
        var result = sut.Format(null, formattableString, sut);

        // Assert
        result.ShouldBe(expectedResult);
        sut.Parameters.ParameterNames.Count().ShouldBe(3);
        sut.Parameters.Get<int[]>("pc0_").ShouldBe(model.TypeIds);
        sut.Parameters.Get<int>("p1").ShouldBe(model.Id);
        sut.Parameters.Get<int[]>("pc2_").ShouldBe(model.TypeIds);
    }

    [Theory]
    [InlineData("TABLE", "TABLE")]
    [InlineData(10, "10")]
    [InlineData(null, "")]
    public void Format_FormatsArgumentWithRaw_ReturnsString(object? argument, string expectedResult)
    {
        // Arrange
        var sut = CreateSqlFormatter();

        // Act
        var result = sut.Format(Constants.RawFormat, argument, sut);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("John")]
    [InlineData(10)]
    [InlineData(null)]
    public void Format_FormatsArgument_ReturnsString(object? argument)
    {
        // Arrange
        var sut = CreateSqlFormatter();

        // Act
        var result = sut.Format(null, argument, sut);

        // Assert
        result.ShouldBe("@p0");
        sut.Parameters.ParameterNames.Count().ShouldBe(1);
        sut.Parameters.Get<object?>("p0").ShouldBe(argument);
    }

    [Theory]
    [InlineData("Mike")]
    [InlineData(20)]
    [InlineData(null)]
    public void Format_FormatSimpleParameterInfo_ReturnsString(object? value)
    {
        // Arrange
        var parameterInfo = new SimpleParameterInfo(value);

        var sut = CreateSqlFormatter();

        // Act
        var result = sut.Format(null, parameterInfo, sut);

        // Assert
        result.ShouldBe("@p0");
        sut.Parameters.ParameterNames.Count().ShouldBe(1);
        sut.Parameters.Get<object?>("p0").ShouldBe(parameterInfo.Value);
    }

    [Fact]
    public void Format_FormatsFormattableStringAndReuseParameters_ReturnsString()
    {
        // Arrange
        var model = new { Id = 10, ProductName = "Product", Price = 10.2.DefineParam(DbType.Double), IsActive = true, SecondName = default(string) };

        FormattableString formattableString = $@"
            INSERT INTO TABLE
            VALUES ({model.Id}, {model.ProductName}, {model.Price}, {model.IsActive}, {model.SecondName})
            INSERT INTO TABLE
            VALUES ({model.Id}, {model.ProductName}, {model.Price}, {model.IsActive}, {model.SecondName})";

        const string expectedResult = @"
            INSERT INTO TABLE
            VALUES (@p0, @p1, @p2, @p3, @p4)
            INSERT INTO TABLE
            VALUES (@p0, @p1, @p2, @p3, @p5)";

        var sut = CreateSqlFormatter(true);

        // Act
        var result = sut.Format(null, formattableString, sut);

        // Assert
        result.ShouldBe(expectedResult);
        sut.Parameters.ParameterNames.Count().ShouldBe(6);
        sut.Parameters.Get<int>("p0").ShouldBe(model.Id);
        sut.Parameters.Get<string>("p1").ShouldBe(model.ProductName);
        sut.Parameters.Get<double>("p2").ShouldBe(model.Price.Value);
        sut.Parameters.Get<bool>("p3").ShouldBe(model.IsActive);
        sut.Parameters.Get<string?>("p4").ShouldBe(model.SecondName);
        sut.Parameters.Get<string?>("p5").ShouldBe(model.SecondName);
    }

    [Fact]
    public void Reset_ResetsSqlFormatter_ReturnsVoid()
    {
        // Arrange
        var sut = CreateSqlFormatter();
        sut.Format(null, 5, sut);

        var formattableString = $"SELECT * FROM TABLE WHERE Id = {10}";
        sut.Format(null, formattableString, sut);

        var parameterInfo = new SimpleParameterInfo("parameterValue");
        sut.Format(null, parameterInfo, sut);

        // Act
        sut.Reset();

        // Assert
        sut.Parameters.ParameterNames.ShouldBeEmpty();
    }

    private static SqlFormatter CreateSqlFormatter(bool reuseParameters = false)
    {
        var parameterOptions = new ParameterOptions(
            SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate,
            SimpleBuilderSettings.Instance.DatabaseParameterPrefix,
            SimpleBuilderSettings.Instance.CollectionParameterFormat,
            reuseParameters);

        return new SqlFormatter(parameterOptions);
    }
}
