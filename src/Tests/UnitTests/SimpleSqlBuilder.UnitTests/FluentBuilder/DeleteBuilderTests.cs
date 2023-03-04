using Dapper.SimpleSqlBuilder.Extensions;
using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder;

public class DeleteBuilderTests
{
    [Fact]
    public void DeleteFrom_BuildsSql_ReturnsFluentSqlBuilder()
    {
        // Arrange
        const string expectedSql = "DELETE FROM Table";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table");

        // Assert
        sut.Should().BeOfType<FluentSqlBuilder>();
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(0);
        sut.Parameters.Should().BeOfType<DynamicParameters>();
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildsSqlWithWhereMethods_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = @p0 OR Type = @p1";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .OrWhere($"Type = {type}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildsSqlWithWhereFilterMethods_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE (Id = @p0) OR (Type = @p1)";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .WhereFilter($"Id = {id}")
            .OrWhereFilter($"Type = {type}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildsSqlWithWhereConditionalMethods_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}" +
            "WHERE (Age < 100 OR Age = @p0 OR Age IN (1, 2, 3)) AND Type LIKE '%Type' AND (Age > 10 AND Type = @p1) OR Id NOT IN (1, 2, 3)";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .Where(false, $"Id = {id}")
            .WhereFilter().WithFilter(false, $"Id = {id}").WithOrFilter(false, $"Age > {age}")
            .OrWhereFilter().WithFilter(true, $"Age < 100").WithOrFilter($"Age = {age}").WithOrFilter($"Age IN (1, 2, 3)")
            .Where($"Type LIKE '%Type'")
            .WhereFilter().WithFilter(false, $"Id = {id}").WithOrFilter(true, $"Age > 10").WithFilter($"Type = {type}")
            .OrWhere(true, $"Id NOT IN (1, 2, 3)");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>("p0").Should().Be(age);
        sut.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildsSqlAndIgnoreNotAllowedMethods_ReturnsFluentSqlBuilder(int id)
    {
        // Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = @p0";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .GroupBy($"Id")
            .Having($"Id = {id}")
            .OrderBy($"Type");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(1);
        sut.GetValue<int>("p0").Should().Be(id);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildsSqlWithInnerFormattableString_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = @p0 AND TypeId IN (SELECT TypeId WHERE Type = @p1)";
        FormattableString subQuery = $"SELECT TypeId WHERE Type = {type}";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .Where($"TypeId IN ({subQuery})");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [InlineAutoData(null)]
    public void DeleteFrom_BuildsSqlWithRawValues_ReturnsFluentSqlBuilder(string? group, string tableName, int typeId, string type)
    {
        // Arrange
        var expectedSql = $"DELETE FROM {tableName}{Environment.NewLine}WHERE Type = @p0 AND Group = '' AND TypeGroup IN (SELECT TypeGroup WHERE TypeId = {typeId})";
        FormattableString subQuery = $"SELECT TypeGroup WHERE TypeId = {typeId:raw}";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .DeleteFrom($"{tableName:raw}")
            .Where($"Type = {type}")
            .Where($"Group = '{group:raw}'")
            .Where($"TypeGroup IN ({subQuery})");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(1);
        sut.GetValue<string>("p0").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildsSqlWithSimpleParameterInfoValues_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var idParam = id.DefineParam(System.Data.DbType.Int32, 1, 1, 1);
        var typeParam = type.DefineParam();
        string expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = @p0 AND Type = @p1 OR (Id = @p0 AND Type = @p1)";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .Where($"Id = {idParam}")
            .Where($"Type = {typeParam}")
            .OrWhereFilter($"Id = {idParam}").WithFilter($"Type = {typeParam}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildsSqlAndAddParameter_ReturnsFluentSqlBuilder(int id)
    {
        // Arrange
        string expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = @{nameof(id)}";

        var sut = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .Where($"Id = @{nameof(id):raw}");

        // Act
        sut.AddParameter(nameof(id), id);

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(1);
        sut.GetValue<int>(nameof(id)).Should().Be(id);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildsSqlWithCustomParameterPrefix_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = :p0 OR Age = :p1 AND Type = :p2";

        // Act
        var sut = SimpleBuilder.CreateFluent(parameterPrefix: ":")
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .OrWhere($"Age = {age}")
            .Where($"Type = {type}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(3);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<int>("p1").Should().Be(age);
        sut.GetValue<string>("p2").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildsSqlAndReuseParameters_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE (Id = @p0 AND Type = @p1) OR Id = @p0 OR Type = @p1";

        // Act
        var sut = SimpleBuilder.CreateFluent(reuseParameters: true)
            .DeleteFrom($"Table")
            .WhereFilter($"Id = {id}").WithFilter($"Type = {type}")
            .OrWhere($"Id = {id}")
            .OrWhere($"Type = {type}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildsSqlAndUseLowerCaseClauses_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"delete from Table{Environment.NewLine}where Id = @p0 or (Age = @p1 and Type = @p2)";

        // Act
        var sut = SimpleBuilder.CreateFluent(useLowerCaseClauses: true)
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .OrWhereFilter($"Age = {age}").WithFilter($"Type = {type}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(3);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<int>("p1").Should().Be(age);
        sut.GetValue<string>("p2").Should().Be(type);
    }
}
