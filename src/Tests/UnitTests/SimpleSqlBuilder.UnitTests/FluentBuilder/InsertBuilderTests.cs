using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder;

public class InsertBuilderTests
{
    [Theory]
    [AutoData]
    public void InsertInto_BuildSql_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (@p0, @p1, @p2)";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Values($"{id}")
            .Values($"{age}, {type}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildSqlWithColumns_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"INSERT INTO Table (Id, Age, Type){Environment.NewLine}VALUES (@p0, @p1, @p2)";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Columns($"Id, Age")
            .Columns($"Type")
            .Values($"{id}, {age}")
            .Values($"{type}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildSqlWithInnerFormattableString_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"INSERT INTO Table (Id, Age, Type){Environment.NewLine}VALUES (@p0, @p1, @p2)";
        FormattableString values = $"{id}, {age}, {type}";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Columns($"Id").Columns($"Age, Type")
            .Values($"{values}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildSqlWithCustomParameterPrefix_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (:p0, :p1, :p2)";

        //Act
        var builder = SimpleBuilder.CreateFluent(parameterPrefix: ":")
            .InsertInto($"Table")
            .Values($"{id}")
            .Values($"{age}, {type}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildSqlAndReuseParameters_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (@p0, @p1, @p0, @p1)";

        //Act
        var builder = SimpleBuilder.CreateFluent(reuseParameters: true)
            .InsertInto($"Table")
            .Values($"{id}, {type}, {id}, {type}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildSqlAndUseLowerCaseClauses_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"insert into Table{Environment.NewLine}values (@p0, @p1, @p2)";

        //Act
        var builder = SimpleBuilder.CreateFluent(useLowerCaseClauses: true)
            .InsertInto($"Table")
            .Values($"{id}")
            .Values($"{age}")
            .Values($"{type}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
    }
}
