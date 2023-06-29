using Dapper.SimpleSqlBuilder.FluentBuilder;
using Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;
using Microsoft.Extensions.Options;

namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Core;

public class SimpleBuilderFactoryTests
{
    [Theory]
    [AutoMoqData(true)]
    internal void Create_CreatesBuilder_ReturnsSqlBuilder(SimpleBuilderFactory sut)
    {
        // Act
        var result = sut.Create();

        // Assert
        result.Should().BeOfType<SqlBuilder>();
        result.ParameterNames.Should().HaveCount(0);
    }

    [Theory]
    [AutoMoqData]
    internal void Create_CreatesBuilderWithInterpolatedString_ReturnsSqlBuilder(
        [Frozen] Mock<IOptionsMonitor<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut,
        [NoAutoProperties] SimpleBuilderOptions options,
        int id,
        string type)
    {
        // Arrange
        string expectedSql = $"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = @p0) FROM TABLE WHERE Id = {id} AND Type = @p1";

        optionsMock.SetupGet(x => x.CurrentValue).Returns(options);

        // Act
        var result = sut.Create($"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {id}) FROM TABLE WHERE Id = {id:raw} AND Type = {type}");

        // Assert
        result.Should().BeOfType<SqlBuilder>();
        result.Sql.Should().Be(expectedSql);
        result.ParameterNames.Should().HaveCount(2);
        result.GetValue<int>("p0").Should().Be(id);
        result.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoMoqData]
    internal void Create_CreatesBuilderWithAllArguments_ReturnsSqlBuilder(
        [Frozen] Mock<IOptionsMonitor<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut,
        [NoAutoProperties] SimpleBuilderOptions options,
        int id,
        string type)
    {
        // Arrange
        string expectedSql = $"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = :p0 AND Type = :p1) FROM TABLE WHERE Id = {id} AND Type = :p1";

        optionsMock.SetupGet(x => x.CurrentValue).Returns(options);

        // Act
        var result = sut.Create(
            $"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {id} AND Type = {type}) FROM TABLE WHERE Id = {id:raw} AND Type = {type}",
            parameterPrefix: ":",
            reuseParameters: true);

        // Assert
        result.Should().BeOfType<SqlBuilder>();
        result.Sql.Should().Be(expectedSql);
        result.GetValue<int>("p0").Should().Be(id);
        result.GetValue<string>("p1").Should().Be(type);
        result.ParameterNames.Should().HaveCount(2);
    }

    [Theory]
    [AutoMoqData(true)]
    [InlineAutoMoqData(configureMembers: true, generateDelegates: false, null, null, null)]
    internal void CreateFluent_CreatesFluentBuilder_ReturnsFluentSqlBuilder(string? parameterPrefix, bool? reuseParameters, bool? useLowerCaseClauses, SimpleBuilderFactory sut)
    {
        // Act
        var result = sut.CreateFluent(parameterPrefix, reuseParameters, useLowerCaseClauses);

        // Assert
        result.Should().BeOfType<FluentSqlBuilder>();
    }

    [Theory]
    [AutoMoqData]
    internal void CreateFluent_CreatesFluentBuilderWithInterpolatedString_ReturnsFluentSqlBuilder(
        [Frozen] Mock<IOptionsMonitor<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut,
        [NoAutoProperties] SimpleBuilderOptions options,
        int id,
        string type)
    {
        // Arrange
        FormattableString subQuery = $"SELECT DESC FROM DESC_TABLE WHERE Id = {id}";
        string expectedSql = "SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = @p0)" +
            $"{Environment.NewLine}FROM TABLE" +
            $"{Environment.NewLine}WHERE Id = {id} AND Type = @p1";

        optionsMock.SetupGet(x => x.CurrentValue).Returns(options);

        // Act
        var result = sut.CreateFluent()
            .Select($"x.*")
            .Select($"({subQuery})")
            .From($"TABLE")
            .Where($"Id = {id:raw}")
            .Where($"Type = {type}");

        // Assert
        result.Should().BeOfType<FluentSqlBuilder>();
        result.Sql.Should().Be(expectedSql);
        result.ParameterNames.Should().HaveCount(2);
        result.GetValue<int>("p0").Should().Be(id);
        result.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoMoqData]
    internal void CreateFluent_CreatesFluentBuilderWithAllArguments_ReturnsFluentSqlBuilder(
        [Frozen] Mock<IOptionsMonitor<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut,
        [NoAutoProperties] SimpleBuilderOptions options,
        int id,
        string type)
    {
        // Arrange
        FormattableString subQuery = $"select DESC from DESC_TABLE where Id = {id} and Type = {type}";
        string expectedSql = "select x.*, (select DESC from DESC_TABLE where Id = :p0 and Type = :p1)" +
            $"{Environment.NewLine}from TABLE" +
            $"{Environment.NewLine}where Id = {id} and Type = :p1";

        optionsMock.SetupGet(x => x.CurrentValue).Returns(options);

        // Act
        var result = sut.CreateFluent(":", true, true)
            .Select($"x.*")
            .Select($"({subQuery})")
            .From($"TABLE")
            .Where($"Id = {id:raw}")
            .Where($"Type = {type}");

        // Assert
        result.Should().BeOfType<FluentSqlBuilder>();
        result.Sql.Should().Be(expectedSql);
        result.ParameterNames.Should().HaveCount(2);
        result.GetValue<int>("p0").Should().Be(id);
        result.GetValue<string>("p1").Should().Be(type);
    }
}
