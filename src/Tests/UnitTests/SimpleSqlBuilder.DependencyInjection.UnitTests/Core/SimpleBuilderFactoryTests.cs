using Dapper.SimpleSqlBuilder.FluentBuilder;
using Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;
using Microsoft.Extensions.Options;

namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Core;

public class SimpleBuilderFactoryTests
{
    [Theory]
    [AutoMoqData]
    internal void Create_CreatesBuilder_ReturnsSqlBuilder(
        [NoAutoProperties] SimpleBuilderOptions options,
        [Frozen] Mock<IOptionsMonitor<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut)
    {
        // Arrange
        optionsMock.SetupGet(x => x.CurrentValue).Returns(options);

        // Act
        var result = sut.Create();

        // Assert
        result.Should().BeOfType<SqlBuilder>();
        result.ParameterNames.Should().HaveCount(0);
    }

    [Theory]
    [AutoMoqData]
    internal void Create_CreatesBuilderWithInterpolatedString_ReturnsSqlBuilder(
        int id,
        string[] types,
        [NoAutoProperties] SimpleBuilderOptions options,
        [Frozen] Mock<IOptionsMonitor<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut)
    {
        // Arrange
        string expectedSql = $"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = @p0) FROM TABLE WHERE Id = {id} AND Type IN @pc1_";

        optionsMock.SetupGet(x => x.CurrentValue).Returns(options);

        // Act
        var result = sut.Create($"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {id}) FROM TABLE WHERE Id = {id:raw} AND Type IN {types}");

        // Assert
        result.Should().BeOfType<SqlBuilder>();
        result.Sql.Should().Be(expectedSql);
        result.ParameterNames.Should().HaveCount(2);
        result.GetValue<int>("p0").Should().Be(id);
        result.GetValue<string[]>("pc1_").Should().BeEquivalentTo(types);
    }

    [Theory]
    [AutoMoqData]
    internal void Create_CreatesBuilderWithCustomPrefixAndReuseParameters_ReturnsSqlBuilder(
        int id,
        string[] types,
        [NoAutoProperties] SimpleBuilderOptions options,
        [Frozen] Mock<IOptionsMonitor<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut)
    {
        // Arrange
        string expectedSql = $"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = :p0 AND Type IN :pc1_) FROM TABLE WHERE Id = {id} AND Type IN :pc1_";

        optionsMock.SetupGet(x => x.CurrentValue).Returns(options);

        // Act
        var result = sut.Create(
            $"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {id} AND Type IN {types}) FROM TABLE WHERE Id = {id:raw} AND Type IN {types}",
            parameterPrefix: ":",
            reuseParameters: true);

        // Assert
        result.Should().BeOfType<SqlBuilder>();
        result.Sql.Should().Be(expectedSql);
        result.GetValue<int>("p0").Should().Be(id);
        result.GetValue<string[]>("pc1_").Should().BeEquivalentTo(types);
        result.ParameterNames.Should().HaveCount(2);
    }

    [Theory]
    [AutoMoqData]
    [InlineAutoMoqData(null, null, null)]
    internal void CreateFluent_CreatesFluentBuilder_ReturnsFluentSqlBuilder(
        string? parameterPrefix,
        bool? reuseParameters,
        bool? useLowerCaseClauses,
        [NoAutoProperties] SimpleBuilderOptions options,
        [Frozen] Mock<IOptionsMonitor<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut)
    {
        // Arrange
        optionsMock.SetupGet(x => x.CurrentValue).Returns(options);

        // Act
        var result = sut.CreateFluent(parameterPrefix, reuseParameters, useLowerCaseClauses);

        // Assert
        result.Should().BeOfType<FluentSqlBuilder>();
    }

    [Theory]
    [AutoMoqData]
    internal void CreateFluent_CreatesFluentBuilderWithInterpolatedString_ReturnsFluentSqlBuilder(
        int id,
        string[] types,
        [NoAutoProperties] SimpleBuilderOptions options,
        [Frozen] Mock<IOptionsMonitor<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut)
    {
        // Arrange
        FormattableString subQuery = $"SELECT DESC FROM DESC_TABLE WHERE Id = {id}";
        string expectedSql = "SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = @p0)" +
            $"{Environment.NewLine}FROM TABLE" +
            $"{Environment.NewLine}WHERE Id = {id} AND Type IN @pc1_";

        optionsMock.SetupGet(x => x.CurrentValue).Returns(options);

        // Act
        var result = sut.CreateFluent()
            .Select($"x.*")
            .Select($"({subQuery})")
            .From($"TABLE")
            .Where($"Id = {id:raw}")
            .Where($"Type IN {types}");

        // Assert
        result.Should().BeOfType<FluentSqlBuilder>();
        result.Sql.Should().Be(expectedSql);
        result.ParameterNames.Should().HaveCount(2);
        result.GetValue<int>("p0").Should().Be(id);
        result.GetValue<string[]>("pc1_").Should().BeEquivalentTo(types);
    }

    [Theory]
    [AutoMoqData]
    internal void CreateFluent_CreatesFluentBuilderWithAllArguments_ReturnsFluentSqlBuilder(
        int id,
        string[] types,
        [NoAutoProperties] SimpleBuilderOptions options,
        [Frozen] Mock<IOptionsMonitor<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut)
    {
        // Arrange
        FormattableString subQuery = $"select DESC from DESC_TABLE where Id = {id} and Type IN {types}";
        string expectedSql = "select x.*, (select DESC from DESC_TABLE where Id = :p0 and Type IN :pc1_)" +
            $"{Environment.NewLine}from TABLE" +
            $"{Environment.NewLine}where Id = {id} and Type IN :pc1_";

        optionsMock.SetupGet(x => x.CurrentValue).Returns(options);

        // Act
        var result = sut.CreateFluent(":", true, true)
            .Select($"x.*")
            .Select($"({subQuery})")
            .From($"TABLE")
            .Where($"Id = {id:raw}")
            .Where($"Type IN {types}");

        // Assert
        result.Should().BeOfType<FluentSqlBuilder>();
        result.Sql.Should().Be(expectedSql);
        result.ParameterNames.Should().HaveCount(2);
        result.GetValue<int>("p0").Should().Be(id);
        result.GetValue<string[]>("pc1_").Should().BeEquivalentTo(types);
    }
}
