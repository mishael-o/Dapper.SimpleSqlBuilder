using Dapper.SimpleSqlBuilder.Extensions;
using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder;

public class UpdateBuilderTests
{
    [Theory]
    [AutoData]
    public void Update_BuildSql_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Id = @p0, Age = @p1, Type = @p2";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Id = {id}")
            .Set($"Age = {age}, Type = {type}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.Parameters.Should().BeOfType<DynamicParameters>();
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlWithSetConditionalMethods_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Age = @p0, Type = @p1";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set(false, $"Id = {id}")
            .Set($"Age = {age}")
            .Set(true, $"Type = {type}");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("p0").Should().Be(age);
        builder.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlWithWhereMethods_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Age = @p0, Type = @p1{Environment.NewLine}WHERE Id = @p2 OR Type = @p3";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Age = {age}")
            .Set($"Type = {type}")
            .Where($"Id = {id}")
            .OrWhere($"Type = {type}");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(4);
        builder.GetValue<int>("p0").Should().Be(age);
        builder.GetValue<string>("p1").Should().Be(type);
        builder.GetValue<int>("p2").Should().Be(id);
        builder.GetValue<string>("p3").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlWithWhereFilterMethods_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Age = @p0, Type = @p1{Environment.NewLine}WHERE (Id = @p2) OR (Type = @p3)";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Age = {age}")
            .Set($"Type = {type}")
            .WhereFilter($"Id = {id}")
            .OrWhereFilter($"Type = {type}");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(4);
        builder.GetValue<int>("p0").Should().Be(age);
        builder.GetValue<string>("p1").Should().Be(type);
        builder.GetValue<int>("p2").Should().Be(id);
        builder.GetValue<string>("p3").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlWithWhereConditionalMethods_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Age = @p0{Environment.NewLine}" +
            "WHERE (Age = @p1 OR Type = @p2 AND Age IN (1, 2, 3)) AND Type LIKE '%Type' AND (Age > 10 AND Type = @p3) OR Id NOT IN (1, 2, 3)";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Age = {age}")
            .Where(false, $"Id = {id}")
            .WhereFilter().WithFilter(false, $"Id = {id}").WithOrFilter(false, $"Age > {age}")
            .OrWhereFilter().WithFilter($"Age = {age}").WithOrFilter(false, $"Type = {type}").WithOrFilter($"Type = {type}").WithFilter($"Age IN (1, 2, 3)")
            .Where($"Type LIKE '%Type'")
            .WhereFilter().WithFilter(false, $"Id = {id}").WithOrFilter(true, $"Age > 10").WithFilter($"Type = {type}")
            .OrWhere(true, $"Id NOT IN (1, 2, 3)");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(4);
        builder.GetValue<int>("p0").Should().Be(age);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
        builder.GetValue<string>("p3").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlWithInnerFormattableString_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Id = @p0{Environment.NewLine}WHERE TypeId IN (SELECT TypeId WHERE Type = @p1)";
        FormattableString subQuery = $"SELECT TypeId WHERE Type = {type}";

        //Act
        var builder = SimpleBuilder.CreateFluent(reuseParameters: true)
            .Update($"Table")
            .Set($"Id = {id}")
            .Where($"TypeId IN ({subQuery})");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [InlineAutoData(null)]
    public void Update_BuildSqlAndAddRawValues_ReturnsFluentSqlBuilder(string? group, string tableName, int typeId, string type)
    {
        //Arrange
        var expectedSql = $"UPDATE {tableName}{Environment.NewLine}SET Type = @p0, Group = ''{Environment.NewLine}WHERE TypeGroup IN (SELECT TypeGroup WHERE TypeId = {typeId})";
        FormattableString subQuery = $"SELECT TypeGroup WHERE TypeId = {typeId:raw}";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Update($"{tableName:raw}")
            .Set($"Type = {type}")
            .Set($"Group = '{group:raw}'")
            .Where($"TypeGroup IN ({subQuery})");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(1);
        builder.GetValue<string>("p0").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlAndAddSimpleParameterInfoValues_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var idParam = id.DefineParam(System.Data.DbType.Int32, 1, 1, 1);
        var typeParam = type.DefineParam();
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Type = @p0, Id = @p1{Environment.NewLine}WHERE Id = @p1 AND Type = @p0";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Type = {typeParam}")
            .Set($"Id = {idParam}")
            .Where($"Id = {idParam}")
            .Where($"Type = {typeParam}");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<string>("p0").Should().Be(type);
        builder.GetValue<int>("p1").Should().Be(id);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlAndAddParameter_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Type = @{nameof(type)}{Environment.NewLine}WHERE Id = @{nameof(id)}";

        var builder = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Type = @{nameof(type):raw}")
            .Where($"Id = @{nameof(id):raw}");

        //Act
        builder.AddParameter(nameof(id), id);
        builder.AddParameter(nameof(type), type);

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>(nameof(id)).Should().Be(id);
        builder.GetValue<string>(nameof(type)).Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlIgnoreNotAllowedMethods_ReturnsFluentSqlBuilder(int id, int age)
    {
        //Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Age = @p0{Environment.NewLine}WHERE Id = @p1";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Update($"Table")
            .Set($"Age = {age}")
            .Where($"Id = {id}")
            .GroupBy($"Type")
            .Having($"Id = {id}")
            .OrderBy($"Id");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("p0").Should().Be(age);
        builder.GetValue<int>("p1").Should().Be(id);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlWithCustomParameterPrefix_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Id = :p0, Age = :p1, Type = :p2{Environment.NewLine}WHERE Id = :p3";

        //Act
        var builder = SimpleBuilder.CreateFluent(parameterPrefix: ":")
            .Update($"Table")
            .Set($"Id = {id}, Age = {age}, Type = {type}")
            .Where($"Id = {id}");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(4);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
        builder.GetValue<int>("p3").Should().Be(id);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlAndReuseParameters_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"UPDATE Table{Environment.NewLine}SET Id = @p0, Type = @p1{Environment.NewLine}WHERE Id = @p0 AND Type = @p1";

        //Act
        var builder = SimpleBuilder.CreateFluent(reuseParameters: true)
            .Update($"Table")
            .Set($"Id = {id}")
            .Set($"Type = {type}")
            .Where($"Id = {id}")
            .Where($"Type = {type}");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Update_BuildSqlAndUseLowerCaseClauses_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"update Table{Environment.NewLine}set Id = @p0, Age = @p1{Environment.NewLine}where Id = @p2 or (Type = @p3 and Age = @p4)";

        //Act
        var builder = SimpleBuilder.CreateFluent(useLowerCaseClauses: true)
            .Update($"Table")
            .Set($"Id = {id}, Age = {age}")
            .Where($"Id = {id}")
            .OrWhereFilter($"Type = {type}").WithFilter($"Age = {age}");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(5);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<int>("p2").Should().Be(id);
        builder.GetValue<string>("p3").Should().Be(type);
        builder.GetValue<int>("p4").Should().Be(age);
    }
}
