using System.Data;
using Dapper.SimpleSqlBuilder.Extensions;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

public class SqlFormatterTests
{
    [Fact]
    public void GetFormat_FormatterMatched_ReturnsObject()
    {
        //Arrange
        var sut = CreateSqlFormatter();
        var formatType = typeof(SqlFormatter);

        //Act
        var result = sut.GetFormat(formatType);

        //Assert
        result.Should().Be(sut);
    }

    [Fact]
    public void GetFormat_NoFormtterMatched_ReturnsObject()
    {
        //Arrange
        var sut = CreateSqlFormatter();
        var formatType = typeof(int);

        //Act
        var result = sut.GetFormat(formatType);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Format_FormatFormattableString_ReturnsString()
    {
        //Arrange
        var model = new { Id = 10, TypeId = 20 };
        const int expectedParameterCount = 3;
        var parameters = new DynamicParameters();
        var parameterNamePrefix = SimpleBuilderSettings.DatabaseParameterPrefix + SimpleBuilderSettings.DatabaseParameterNameTemplate;

        FormattableString innerFormattableString1 = $"SELECT Description FROM TYPE_TABLE WHERE Id = {model.TypeId}";
        FormattableString innerFormattableString2 = $"SELECT Id FROM TYPE_TABLE WHERE EXPIRED_DATE IS NULL";
        FormattableString formattableString = $@"
            SELECT *, ({innerFormattableString1})
            FROM TABLE x WHERE Id = {model.Id}
            AND TypeId = {model.TypeId}
            AND TypeId NOT IN {innerFormattableString2}";

        var expectedResult = $@"
            SELECT *, (SELECT Description FROM TYPE_TABLE WHERE Id = {parameterNamePrefix}0)
            FROM TABLE x WHERE Id = {parameterNamePrefix}1
            AND TypeId = {parameterNamePrefix}2
            AND TypeId NOT IN {innerFormattableString2.Format}";

        var sut = CreateSqlFormatter(parameters);

        //Act
        var result = sut.Format(null, formattableString, sut);

        //Assert
        result.Should().Be(expectedResult);
        parameters.ParameterNames.Should().HaveCount(expectedParameterCount);
        parameters.Get<int>($"{parameterNamePrefix}0").Should().Be(model.TypeId);
        parameters.Get<int>($"{parameterNamePrefix}1").Should().Be(model.Id);
        parameters.Get<int>($"{parameterNamePrefix}2").Should().Be(model.TypeId);
    }

    [Theory]
    [InlineData("TABLE", "TABLE")]
    [InlineData(10, "10")]
    [InlineData(null, "")]
    public void Format_FormatRaw_ReturnsString(object? argument, string expectedResult)
    {
        //Arrange
        var sut = CreateSqlFormatter();

        //Act
        var result = sut.Format(SqlFormatter.RawFormat, argument, sut);

        //Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("John")]
    [InlineData(10)]
    [InlineData(null)]
    public void Format_FormatArgument_ReturnsString(object? argument)
    {
        //Arrange
        var parameters = new DynamicParameters();
        var parameterName = $"{SimpleBuilderSettings.DatabaseParameterNameTemplate}0";

        var sut = CreateSqlFormatter(parameters);

        //Act
        var result = sut.Format(null, argument, sut);

        //Assert
        result.Should().Be(SimpleBuilderSettings.DatabaseParameterPrefix + parameterName);
        parameters.Get<object?>(parameterName).Should().Be(argument);
    }

    [Theory]
    [InlineData("Mike")]
    [InlineData(20)]
    [InlineData(null)]
    public void Format_FormatSimpleParameterInfo_ReturnsString(object? value)
    {
        //Arrange
        var parameterInfo = new SimpleParameterInfo(value);
        var parameters = new DynamicParameters();
        var parameterName = $"{SimpleBuilderSettings.DatabaseParameterNameTemplate}0";

        var sut = CreateSqlFormatter(parameters);

        //Act
        var result = sut.Format(null, parameterInfo, sut);

        //Assert
        result.Should().Be(SimpleBuilderSettings.DatabaseParameterPrefix + parameterName);
        parameters.Get<object?>(parameterName).Should().Be(parameterInfo.Value);
    }

    [Fact]
    public void Format_FormatFormattableStringAndReuseParameter_ReturnsString()
    {
        //Arrange
        var model = new { Id = 10, ProductName = "Product", Price = 10.2.DefineParam(DbType.Double), IsActive = true, SecondName = default(string) };
        const int expectedParameterCount = 6;
        var parameters = new DynamicParameters();
        var parameterNamePrefix = SimpleBuilderSettings.DatabaseParameterPrefix + SimpleBuilderSettings.DatabaseParameterNameTemplate;

        FormattableString formattableString = $@"
            INSERT INTO TABLE
            VALUES ({model.Id}, {model.ProductName}, {model.Price}, {model.IsActive}, {model.SecondName})
            INSERT INTO TABLE
            VALUES ({model.Id}, {model.ProductName}, {model.Price}, {model.IsActive}, {model.SecondName})";

        var expectedResult = $@"
            INSERT INTO TABLE
            VALUES ({parameterNamePrefix}0, {parameterNamePrefix}1, {parameterNamePrefix}2, {parameterNamePrefix}3, {parameterNamePrefix}4)
            INSERT INTO TABLE
            VALUES ({parameterNamePrefix}0, {parameterNamePrefix}1, {parameterNamePrefix}2, {parameterNamePrefix}3, {parameterNamePrefix}5)";

        var sut = CreateSqlFormatter(parameters, true);

        //Act
        var result = sut.Format(null, formattableString, sut);

        //Assert
        result.Should().Be(expectedResult);
        parameters.ParameterNames.Should().HaveCount(expectedParameterCount);
        parameters.Get<int>($"{parameterNamePrefix}0").Should().Be(model.Id);
        parameters.Get<string>($"{parameterNamePrefix}1").Should().Be(model.ProductName);
        parameters.Get<double>($"{parameterNamePrefix}2").Should().Be(model.Price.Value.As<double>());
        parameters.Get<bool>($"{parameterNamePrefix}3").Should().Be(model.IsActive);
        parameters.Get<string?>($"{parameterNamePrefix}4").Should().Be(model.SecondName);
        parameters.Get<string?>($"{parameterNamePrefix}5").Should().Be(model.SecondName);
    }

    private static SqlFormatter CreateSqlFormatter(DynamicParameters? parameters = null, bool reuseParameters = false)
    {
        return new SqlFormatter(
            parameters ?? new DynamicParameters(),
            SimpleBuilderSettings.DatabaseParameterNameTemplate,
            SimpleBuilderSettings.DatabaseParameterPrefix,
            reuseParameters);
    }
}
