using Dapper.SimpleSqlBuilder.Extensions;
using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder;

public class InsertBuilderTests
{
    [Theory]
    [AutoData]
    public void InsertInto_BuildsSql_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (@p0, @p1, @p2)";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Values($"{id}")
            .Values($"{age}, {type}");

        // Assert
        sut.ShouldBeOfType<FluentSqlBuilder>();
        sut.Sql.ShouldBe(expectedSql);
        sut.ParameterNames.Count().ShouldBe(3);
        sut.Parameters.ShouldBeOfType<DynamicParameters>();
        sut.GetValue<int>("p0").ShouldBe(id);
        sut.GetValue<int>("p1").ShouldBe(age);
        sut.GetValue<string>("p2").ShouldBe(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildsSqlWithColumns_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"INSERT INTO Table (Id, Age, Type){Environment.NewLine}VALUES (@p0, @p1, @p2)";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Columns($"Id, Age")
            .Columns($"Type")
            .Values($"{id}, {age}")
            .Values($"{type}");

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.ParameterNames.Count().ShouldBe(3);
        sut.GetValue<int>("p0").ShouldBe(id);
        sut.GetValue<int>("p1").ShouldBe(age);
        sut.GetValue<string>("p2").ShouldBe(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildsSqlWithInnerFormattableString_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"INSERT INTO Table (Id, Age, Type){Environment.NewLine}VALUES (@p0, @p1, @p2)";
        FormattableString values = $"{id}, {age}, {type}";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Columns($"Id").Columns($"Age, Type")
            .Values($"{values}");

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.ParameterNames.Count().ShouldBe(3);
        sut.GetValue<int>("p0").ShouldBe(id);
        sut.GetValue<int>("p1").ShouldBe(age);
        sut.GetValue<string>("p2").ShouldBe(type);
    }

    [Theory]
    [InlineAutoData(null)]
    public void InsertInto_BuildsSqlWithRawValues_ReturnsFluentSqlBuilder(int? groupId, string tableName, int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"INSERT INTO {tableName}{Environment.NewLine}VALUES ({id}, '', @p0, '{type}')";
        FormattableString values = $"{age}, '{type:raw}'";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .InsertInto($"{tableName:raw}")
            .Values($"{id:raw}")
            .Values($"'{groupId:raw}'")
            .Values($"{values}");

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.ParameterNames.ShouldHaveSingleItem();
        sut.GetValue<int>("p0").ShouldBe(age);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildsSqlWithSimpleParameterInfoValues_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var idParam = id.DefineParam(System.Data.DbType.Int32, 1, 1, 1);
        var typeParam = type.DefineParam();
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (@p0, @p1, @p0, @p1)";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Values($"{idParam}")
            .Values($"{typeParam}")
            .Values($"{idParam}")
            .Values($"{typeParam}");

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.ParameterNames.Count().ShouldBe(2);
        sut.GetValue<int>("@p0").ShouldBe(id);
        sut.GetValue<string>("@p1").ShouldBe(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildsSqlWithSimpleParameterInfoValuesAndReuseDisabled_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var idParam = id.DefineParam(System.Data.DbType.Int32, 1, 1, 1, reuse: false);
        var typeParam = type.DefineParam(reuse: false);
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (@p0, @p1, @p2, @p3)";

        // Act
        var sut = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Values($"{idParam}")
            .Values($"{typeParam}")
            .Values($"{idParam}")
            .Values($"{typeParam}");

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.ParameterNames.Count().ShouldBe(4);
        sut.GetValue<int>("@p0").ShouldBe(id);
        sut.GetValue<string>("@p1").ShouldBe(type);
        sut.GetValue<int>("@p2").ShouldBe(id);
        sut.GetValue<string>("@p3").ShouldBe(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildsSqlAndAddParameter_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (@{nameof(id)}, @{nameof(type)})";

        var sut = SimpleBuilder.CreateFluent()
            .InsertInto($"Table")
            .Values($"@{nameof(id):raw}")
            .Values($"@{nameof(type):raw}");

        // Act
        sut.AddParameter(nameof(id), id);
        sut.AddParameter(nameof(type), type);

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.ParameterNames.Count().ShouldBe(2);
        sut.GetValue<int>(nameof(id)).ShouldBe(id);
        sut.GetValue<string>(nameof(type)).ShouldBe(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildsSqlWithCustomParameterPrefix_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (:p0, :p1, :p2)";

        // Act
        var sut = SimpleBuilder.CreateFluent(parameterPrefix: ":")
            .InsertInto($"Table")
            .Values($"{id}")
            .Values($"{age}, {type}");

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.ParameterNames.Count().ShouldBe(3);
        sut.GetValue<int>("p0").ShouldBe(id);
        sut.GetValue<int>("p1").ShouldBe(age);
        sut.GetValue<string>("p2").ShouldBe(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildsSqlAndReuseParameters_ReturnsFluentSqlBuilder(int id, string type)
    {
        // Arrange
        var expectedSql = $"INSERT INTO Table{Environment.NewLine}VALUES (@p0, @p1, @p0, @p1)";

        // Act
        var sut = SimpleBuilder.CreateFluent(reuseParameters: true)
            .InsertInto($"Table")
            .Values($"{id}, {type}, {id}, {type}");

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.ParameterNames.Count().ShouldBe(2);
        sut.GetValue<int>("p0").ShouldBe(id);
        sut.GetValue<string>("p1").ShouldBe(type);
    }

    [Theory]
    [AutoData]
    public void InsertInto_BuildsSqlAndUseLowerCaseClauses_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        // Arrange
        var expectedSql = $"insert into Table{Environment.NewLine}values (@p0, @p1, @p2)";

        // Act
        var sut = SimpleBuilder.CreateFluent(useLowerCaseClauses: true)
            .InsertInto($"Table")
            .Values($"{id}")
            .Values($"{age}")
            .Values($"{type}");

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.ParameterNames.Count().ShouldBe(3);
        sut.GetValue<int>("p0").ShouldBe(id);
        sut.GetValue<int>("p1").ShouldBe(age);
        sut.GetValue<string>("p2").ShouldBe(type);
    }

    [Fact]
    public void InsertInto_DeleteFromMethodIsCalledAfterInsertInto_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = SimpleBuilder.CreateFluent();
        sut.InsertInto($"*");

        // Act
        Action act = () => sut.DeleteFrom($"*");

        // Assert
        act.ShouldThrow<InvalidOperationException>()
            .Message.ShouldBe($"Clause action \"{ClauseAction.Delete}\" is not allowed after \"{ClauseAction.Insert}\" has been initiated on the same Fluent Builder.");
    }

    [Fact]
    public void InsertInto_SelectMethodIsCalledAfterInsertInto_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = SimpleBuilder.CreateFluent();
        sut.InsertInto($"*");

        // Act
        Action act = () => sut.Select($"*");

        // Assert
        act.ShouldThrow<InvalidOperationException>()
            .Message.ShouldBe($"Clause action \"{ClauseAction.Select}\" is not allowed after \"{ClauseAction.Insert}\" has been initiated on the same Fluent Builder.");
    }

    [Fact]
    public void InsertInto_SelectDistinctMethodIsCalledAfterInsertInto_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = SimpleBuilder.CreateFluent();
        sut.InsertInto($"*");

        // Act
        Action act = () => sut.SelectDistinct($"*");

        // Assert
        act.ShouldThrow<InvalidOperationException>()
            .Message.ShouldBe($"Clause action \"{ClauseAction.SelectDistinct}\" is not allowed after \"{ClauseAction.Insert}\" has been initiated on the same Fluent Builder.");
    }

    [Fact]
    public void InsertInto_UpdateMethodIsCalledAfterInsertInto_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = SimpleBuilder.CreateFluent();
        sut.InsertInto($"*");

        // Act
        Action act = () => sut.Update($"*");

        // Assert
        act.ShouldThrow<InvalidOperationException>()
            .Message.ShouldBe($"Clause action \"{ClauseAction.Update}\" is not allowed after \"{ClauseAction.Insert}\" has been initiated on the same Fluent Builder.");
    }
}
