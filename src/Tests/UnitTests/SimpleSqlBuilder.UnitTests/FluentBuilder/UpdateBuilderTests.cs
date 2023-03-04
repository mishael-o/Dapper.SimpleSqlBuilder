using Dapper.SimpleSqlBuilder.Extensions;
using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder;

public class UpdateBuilderTests
{
    [Theory]
    [AutoData]
    public void Update_BuildsSql_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Id = @p0, Age = @p1, Type = @p2";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Id = {id}")
            .Set($"Age = {age}, Type = {type}");

        // Assert
        sut.Should().BeOfType<FluentSqlBuilder>();
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(3);
        sut.Parameters.Should().BeOfType<DynamicParameters>();
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<int>("p1").Should().Be(age);
        sut.GetValue<string>("p2").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlWithSetConditionalMethods_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Age = @p0, Type = @p1";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set(false, $"Id = {id}")
            .Set($"Age = {age}")
            .Set(true, $"Type = {type}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>("p0").Should().Be(age);
        sut.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlWithWhereMethods_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Age = @p0, Type = @p1{Environment.NewLine}WHERE Id = @p2 OR Type = @p3";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Age = {age}")
            .Set($"Type = {type}")
            .Where($"Id = {id}")
            .OrWhere($"Type = {type}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(4);
        sut.GetValue<int>("p0").Should().Be(age);
        sut.GetValue<string>("p1").Should().Be(type);
        sut.GetValue<int>("p2").Should().Be(id);
        sut.GetValue<string>("p3").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlWithWhereFilterMethods_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Age = @p0, Type = @p1{Environment.NewLine}WHERE (Id = @p2) OR (Type = @p3)";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Age = {age}")
            .Set($"Type = {type}")
            .WhereFilter($"Id = {id}")
            .OrWhereFilter($"Type = {type}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(4);
        sut.GetValue<int>("p0").Should().Be(age);
        sut.GetValue<string>("p1").Should().Be(type);
        sut.GetValue<int>("p2").Should().Be(id);
        sut.GetValue<string>("p3").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlWithWhereConditionalMethods_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Age = @p0{Environment.NewLine}" +
            "WHERE (Age = @p1 OR Type = @p2 AND Age IN (1, 2, 3)) AND Type LIKE '%Type' OR (Age > 10 AND Type = @p3) OR Id NOT IN (1, 2, 3)";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Age = {age}")
            .Where(false, $"Id = {id}")
            .WhereFilter().WithFilter(false, $"Id = {id}").WithOrFilter(false, $"Age > {age}")
            .OrWhereFilter().WithFilter(true, $"Age = {age}").WithOrFilter($"Type = {type}").WithFilter($"Age IN (1, 2, 3)")
            .Where($"Type LIKE '%Type'")
            .OrWhereFilter().WithFilter(false, $"Id = {id}").WithOrFilter(true, $"Age > 10").WithFilter($"Type = {type}")
            .OrWhere(true, $"Id NOT IN (1, 2, 3)");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(4);
        sut.GetValue<int>("p0").Should().Be(age);
        sut.GetValue<int>("p1").Should().Be(age);
        sut.GetValue<string>("p2").Should().Be(type);
        sut.GetValue<string>("p3").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlWithInnerFormattableString_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Id = @p0{Environment.NewLine}WHERE TypeId IN (SELECT TypeId WHERE Type = @p1)";
        FormattableString subQuery = $"SELECT TypeId WHERE Type = {type}";

        // Act
        var sut = SimpleBuilder.CreateFluent(reuseParameters: true)
            .Update($"Table")
            .Set($"Id = {id}")
            .Where($"TypeId IN ({subQuery})");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [InlineAutoData(null)]
    public void Update_BuildsSqlWithRawValues_ReturnsFluentSqlBuilder(string? group, string tableName, int typeId, string type)
    {
        // Arrange
        var expectedSql = $"UPDATE {tableName}{Environment.NewLine}SET Type = @p0, Group = ''{Environment.NewLine}WHERE TypeGroup IN (SELECT TypeGroup WHERE TypeId = {typeId})";
        FormattableString subQuery = $"SELECT TypeGroup WHERE TypeId = {typeId:raw}";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .Update($"{tableName:raw}")
            .Set($"Type = {type}")
            .Set($"Group = '{group:raw}'")
            .Where($"TypeGroup IN ({subQuery})");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(1);
        sut.GetValue<string>("p0").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlWithSimpleParameterInfoValues_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var idParam = id.DefineParam(System.Data.DbType.Int32, 1, 1, 1);
        var typeParam = type.DefineParam();
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Type = @p0, Id = @p1{Environment.NewLine}WHERE Id = @p1 AND Type = @p0";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Type = {typeParam}")
            .Set($"Id = {idParam}")
            .Where($"Id = {idParam}")
            .Where($"Type = {typeParam}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<string>("p0").Should().Be(type);
        sut.GetValue<int>("p1").Should().Be(id);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlAndAddParameter_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Type = @{nameof(type)}{Environment.NewLine}WHERE Id = @{nameof(id)}";

        var sut = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Type = @{nameof(type):raw}")
            .Where($"Id = @{nameof(id):raw}");

        // Act
        sut.AddParameter(nameof(id), id);
        sut.AddParameter(nameof(type), type);

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>(nameof(id)).Should().Be(id);
        sut.GetValue<string>(nameof(type)).Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlAndIgnoreNotAllowedMethods_ReturnsFluentSqlBuilder(int id, int age)
    {
        // Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Age = @p0{Environment.NewLine}WHERE Id = @p1";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Age = {age}")
            .Where($"Id = {id}")
            .GroupBy($"Type")
            .Having($"Id = {id}")
            .OrderBy($"Id");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>("p0").Should().Be(age);
        sut.GetValue<int>("p1").Should().Be(id);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlWithCustomParameterPrefix_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Id = :p0, Age = :p1, Type = :p2{Environment.NewLine}WHERE Id = :p3";

        // Act
        var sut = SimpleBuilder.CreateFluent(parameterPrefix: ":")
            .Update($"Table")
            .Set($"Id = {id}, Age = {age}, Type = {type}")
            .Where($"Id = {id}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(4);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<int>("p1").Should().Be(age);
        sut.GetValue<string>("p2").Should().Be(type);
        sut.GetValue<int>("p3").Should().Be(id);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlAndReuseParameters_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Id = @p0, Type = @p1{Environment.NewLine}WHERE Id = @p0 AND Type = @p1";

        // Act
        var sut = SimpleBuilder.CreateFluent(reuseParameters: true)
            .Update($"Table")
            .Set($"Id = {id}")
            .Set($"Type = {type}")
            .Where($"Id = {id}")
            .Where($"Type = {type}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(2);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildsSqlAndUseLowerCaseClauses_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"update Table{Environment.NewLine}set Id = @p0, Age = @p1{Environment.NewLine}where Id = @p2 or (Type = @p3 and Age = @p4)";

        // Act
        var sut = SimpleBuilder.CreateFluent(useLowerCaseClauses: true)
            .Update($"Table")
            .Set($"Id = {id}, Age = {age}")
            .Where($"Id = {id}")
            .OrWhereFilter($"Type = {type}").WithFilter($"Age = {age}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(5);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<int>("p1").Should().Be(age);
        sut.GetValue<int>("p2").Should().Be(id);
        sut.GetValue<string>("p3").Should().Be(type);
        sut.GetValue<int>("p4").Should().Be(age);
    }
}
