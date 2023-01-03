using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder;

public class DeleteBuilderTests
{
    [Theory]
    [AutoData]
    public void DeleteFrom_BuildSql_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = @p0 OR (Age = @p1 AND Type = @p2) OR Age >= @p3";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .OrWhereFilter($"Age = {age}").WithFilter($"Type = {type}")
            .OrWhere($"Age >= {age}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(4);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
        builder.GetValue<int>("p3").Should().Be(age);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildSqlWithCustomParameterPrefix_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = :p0 OR (Age = :p1 AND Type = :p2) OR Age >= :p3";

        //Act
        var builder = SimpleBuilder.CreateFluent(parameterPrefix: ":")
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .OrWhereFilter($"Age = {age}").WithFilter($"Type = {type}")
            .OrWhere($"Age >= {age}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(4);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
        builder.GetValue<int>("p3").Should().Be(age);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildSqlAndReuseParameters_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = @p0 OR (Age = @p1 AND Type = @p2) OR Age >= @p1";

        //Act
        var builder = SimpleBuilder.CreateFluent(reuseParameters: true)
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .OrWhereFilter($"Age = {age}").WithFilter($"Type = {type}")
            .OrWhere($"Age >= {age}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
        builder.GetValue<int>("p1").Should().Be(age);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildSqlAndUseLowerCaseClauses_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"delete from Table{Environment.NewLine}where Id = @p0 or (Age = @p1 and Type = @p2) or Age >= @p3";

        //Act
        var builder = SimpleBuilder.CreateFluent(useLowerCaseClauses: true)
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .OrWhereFilter($"Age = {age}").WithFilter($"Type = {type}")
            .OrWhere($"Age >= {age}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(4);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
        builder.GetValue<int>("p3").Should().Be(age);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildSqlWithInnerFormattableString_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = @p0 AND TypeId IN (SELECT TypeId WHERE Type = @p1)";
        FormattableString subQuery = $"SELECT TypeId WHERE Type = {type}";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .Where($"TypeId IN ({subQuery})");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildSqlWithWhereConditonalStatements_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE (Age = @p0 OR Type = @p1 AND Age IN (1, 2, 3)) AND Type LIKE '%Type' AND (Age > 10 AND Type = @p2) OR Id NOT IN (1, 2, 3)";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .Where(false, $"Id = {id}")
            .WhereFilter().WithFilter(false, $"Id = {id}").WithOrFilter(false, $"Age > {age}")
            .OrWhereFilter().WithFilter($"Age = {age}").WithOrFilter(false, $"Type = {type}").WithOrFilter($"Type = {type}").WithFilter($"Age IN (1, 2, 3)")
            .Where($"Type LIKE '%Type'")
            .WhereFilter().WithFilter(false, $"Id = {id}").WithOrFilter(true, $"Age > 10").WithFilter($"Type = {type}")
            .OrWhere(true, $"Id NOT IN (1, 2, 3)");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.GetValue<int>("p0").Should().Be(age);
        builder.GetValue<string>("p1").Should().Be(type);
        builder.GetValue<string>("p2").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void DeleteFrom_BuildSqlIgnoreNotAllowedClauses_ReturnsFluentSqlBuilder(int id)
    {
        //Arrange
        var expectedSql = $"DELETE FROM Table{Environment.NewLine}WHERE Id = @p0";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .DeleteFrom($"Table")
            .Where($"Id = {id}")
            .GroupBy($"Id")
            .Having($"Id = {id}")
            .OrderBy($"Type");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(1);
        builder.GetValue<int>("p0").Should().Be(id);
    }
}
