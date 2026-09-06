using System.Data;
using System.Text;
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

    [Theory]
    [InlineData(null, "John")]
    [InlineData(null, 10)]
    [InlineData(null, null)]
    [InlineData(Constants.RawFormat, "TABLE")]
    [InlineData(Constants.RawFormat, 10)]
    [InlineData(Constants.RawFormat, null)]
    public void FormatTo_MatchesFormat_ReturnsSameSql(string? format, object? argument)
    {
        // Arrange
        // The two methods are separate dispatch chains over the same rules, so they are pinned together.
        var formatSut = CreateSqlFormatter();
        var formatToSut = CreateSqlFormatter();
        var destination = new StringBuilder();

        // Act
        var expected = formatSut.Format(format, argument, formatSut);
        formatToSut.FormatTo(destination, argument, format);

        // Assert
        destination.ToString().ShouldBe(expected);
        formatToSut.Parameters.ParameterNames.ShouldBe(formatSut.Parameters.ParameterNames);
    }

    [Fact]
    public void FormatTo_FormatsSimpleParameterInfo_MatchesFormat()
    {
        // Arrange
        var formatSut = CreateSqlFormatter();
        var formatToSut = CreateSqlFormatter();
        var destination = new StringBuilder();

        // Act
        var expected = formatSut.Format(null, new SimpleParameterInfo(10, DbType.Int32), formatSut);
        formatToSut.FormatTo(destination, new SimpleParameterInfo(10, DbType.Int32));

        // Assert
        destination.ToString().ShouldBe(expected);
        formatToSut.Parameters.Get<int>("p0").ShouldBe(10);
    }

    [Fact]
    public void FormatTo_FormatsFormattableString_MatchesFormat()
    {
        // Arrange
        var formatSut = CreateSqlFormatter();
        var formatToSut = CreateSqlFormatter();
        var destination = new StringBuilder();

        FormattableString formattableString = $"SELECT * FROM TABLE WHERE Id = {10} AND Type = {"Type"}";
        FormattableString noArguments = $"SELECT * FROM TABLE";

        // Act
        var expected = formatSut.Format(null, formattableString, formatSut)
            + formatSut.Format(null, noArguments, formatSut);
        formatToSut.FormatTo(destination, formattableString);
        formatToSut.FormatTo(destination, noArguments);

        // Assert
        destination.ToString().ShouldBe(expected);
        formatToSut.Parameters.ParameterNames.ShouldBe(formatSut.Parameters.ParameterNames);
    }

    [Fact]
    public void FormatTo_ReusesParameters_MatchesFormat()
    {
        // Arrange
        var formatSut = CreateSqlFormatter(true);
        var formatToSut = CreateSqlFormatter(true);
        var destination = new StringBuilder();

        // Act
        var expected = formatSut.Format(null, 10, formatSut)
            + formatSut.Format(null, 10, formatSut)
            + formatSut.Format(null, 20, formatSut);
        formatToSut.FormatTo(destination, 10);
        formatToSut.FormatTo(destination, 10);
        formatToSut.FormatTo(destination, 20);

        // Assert
        destination.ToString().ShouldBe(expected);
        destination.ToString().ShouldBe("@p0@p0@p1");
    }

    [Fact]
    public void Format_SimpleParameterInfoUsedInTwoFormatters_NamesAreIndependent()
    {
        // Arrange
        // The parameter name belongs to the formatter, not the parameter, so one formatter must never
        // adopt a name another formatter generated. Doing so silently overwrote the value.
        var parameterInfo = new SimpleParameterInfo(99);

        var firstSut = CreateSqlFormatter();
        var secondSut = CreateSqlFormatter();

        // Act
        var firstResult = firstSut.Format(null, parameterInfo, firstSut);
        var secondResult = secondSut.Format(null, parameterInfo, secondSut);
        var otherResult = secondSut.Format(null, 5, secondSut);

        // Assert
        firstResult.ShouldBe("@p0");
        secondResult.ShouldBe("@p0");
        otherResult.ShouldBe("@p1");
        secondSut.Parameters.Get<int>("p0").ShouldBe(99);
        secondSut.Parameters.Get<int>("p1").ShouldBe(5);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Format_SameParameterInfoFormattedTwice_ReturnsSameParameterName(bool reuseParameters)
    {
        // Arrange
        // A defined parameter reuses itself by default, independently of the builder's reuseParameters setting.
        var parameterInfo = new SimpleParameterInfo(10, DbType.Int32);

        var sut = CreateSqlFormatter(reuseParameters);

        // Act
        var firstResult = sut.Format(null, parameterInfo, sut);
        var secondResult = sut.Format(null, parameterInfo, sut);
        var equivalentResult = sut.Format(null, new SimpleParameterInfo(10, DbType.Int32), sut);
        var otherResult = sut.Format(null, new SimpleParameterInfo(10, DbType.Int64), sut);

        // Assert
        firstResult.ShouldBe("@p0");
        secondResult.ShouldBe("@p0");
        equivalentResult.ShouldBe("@p0");
        otherResult.ShouldBe("@p1");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Format_SameParameterInfoFormattedTwiceAndReuseDisabled_ReturnsNewParameterName(bool reuseParameters)
    {
        // Arrange
        // Opting a parameter out of reuse holds even when the builder has reuseParameters enabled,
        // as the two settings govern different things.
        var parameterInfo = new SimpleParameterInfo(10, DbType.Int32, reuse: false);

        var sut = CreateSqlFormatter(reuseParameters);

        // Act
        var firstResult = sut.Format(null, parameterInfo, sut);
        var secondResult = sut.Format(null, parameterInfo, sut);

        // Assert
        firstResult.ShouldBe("@p0");
        secondResult.ShouldBe("@p1");
        sut.Parameters.Get<int>("p0").ShouldBe(10);
        sut.Parameters.Get<int>("p1").ShouldBe(10);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Format_ValuelessParameterInfoFormattedTwice_ReturnsNewParameterName(bool reuseParameters)
    {
        // Arrange
        // Reuse never applies to a parameter with no value, whether it is enabled or not.
        var parameterInfo = new SimpleParameterInfo(null, DbType.Int32);

        var sut = CreateSqlFormatter(reuseParameters);

        // Act
        var firstResult = sut.Format(null, parameterInfo, sut);
        var secondResult = sut.Format(null, parameterInfo, sut);

        // Assert
        firstResult.ShouldBe("@p0");
        secondResult.ShouldBe("@p1");
        sut.Parameters.ParameterNames.Count().ShouldBe(2);
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
