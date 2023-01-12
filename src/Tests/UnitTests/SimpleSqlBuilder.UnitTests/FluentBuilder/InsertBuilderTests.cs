using Dapper.SimpleSqlBuilder.Extensions;
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
        builder.Parameters.Should().BeOfType<DynamicParameters>();
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
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
    }

    [Theory]
    [InlineAutoData(null)]
    public void InsertInto_BuildSqlAndAddRawValues_ReturnsFluentSqlBuilder(int? groupId, string tableName, int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"INSERT INTO {tableName}{Environment.NewLine}VALUES ({id}, '', @p0, '{type}')";
        FormattableString values = $"{age}, '{type:raw}'";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .InsertInto($"{tableName:raw}")
            .Values($"{id:raw}")
            .Values($"'{groupId:raw}'")
            .Values($"{values}");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(1);
        builder.GetValue<int>("p0").Should().Be(age);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildSqlAndAddSimpleParameterInfoValues_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var idParam = id.DefineParam(System.Data.DbType.Int32, 1, 1, 1);
        var typeParam = type.DefineParam();
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (@p0, @p1, @p0, @p1)";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Values($"{idParam}")
            .Values($"{typeParam}")
            .Values($"{idParam}")
            .Values($"{typeParam}");

        //Assert
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("@p0").Should().Be(id);
        builder.GetValue<string>("@p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildSqlAndAddParameter_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (@{nameof(id)}, @{nameof(type)})";

        var builder = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Values($"@{nameof(id):raw}")
            .Values($"@{nameof(type):raw}");

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
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
    }
}
