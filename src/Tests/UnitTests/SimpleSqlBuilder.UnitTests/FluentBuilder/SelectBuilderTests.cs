using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder;

public class SelectBuilderTests
{
    [Fact]
    public void Select_BuildSql_ReturnsFluentSqlBuilder()
    {
        //Arrange
        var expectedSql = $"SELECT *{Environment.NewLine}FROM Table";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"Table");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(0);
    }

    [Fact]
    public void SelectDistinct_BuildSql_ReturnsFluentSqlBuilder()
    {
        //Arrange
        var expectedSql = $"SELECT DISTINCT Id, Type{Environment.NewLine}FROM Table";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .SelectDistinct($"Id")
            .SelectDistinct($"Type")
            .From($"Table");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(0);
    }

    [Theory]
    [AutoData]
    public void Select_BuildSqlWithWhereMethods_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"SELECT Id, Type{Environment.NewLine}FROM Table{Environment.NewLine}WHERE Id = @p0 OR Type = @p1";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Select($"Id")
            .Select($"Type")
            .From($"Table")
            .Where($"Id = {id}")
            .OrWhere($"Type = {type}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void SelectDistinct_BuildSqlWithWhereMethods_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"SELECT DISTINCT Id, Type{Environment.NewLine}FROM Table{Environment.NewLine}WHERE Id = @p0 OR Type = @p1";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .SelectDistinct($"Id, Type")
            .From($"Table")
            .Where($"Id = {id}")
            .OrWhere($"Type = {type}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Select_BuildSqlWithWhereFilterMethods_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"SELECT Id, Type{Environment.NewLine}FROM Table{Environment.NewLine}WHERE (Id = @p0) OR (Type = @p1)";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Select($"Id, Type")
            .From($"Table")
            .WhereFilter($"Id = {id}")
            .OrWhereFilter($"Type = {type}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<string>("p1").Should().Be(type);
    }

    [Fact]
    public void Select_BuildSqlWithJoinMethods_ReturnsFluentSqlBuilder()
    {
        //Arrange
        var expectedSql = $"SELECT *" +
            $"{Environment.NewLine}FROM Table1" +
            $"{Environment.NewLine}INNER JOIN Table2 ON Table1.Id = Table2.Id" +
            $"{Environment.NewLine}LEFT JOIN Table3 ON Table1.Id = Table3.Id" +
            $"{Environment.NewLine}RIGHT JOIN Table4 ON Table1.Id = Table4.Id";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"Table1")
            .InnerJoin($"Table2 ON Table1.Id = Table2.Id")
            .LeftJoin($"Table3 ON Table1.Id = Table3.Id")
            .RightJoin($"Table4 ON Table1.Id = Table4.Id");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(0);
    }

    [Fact]
    public void Select_BuildSqlWithGroupByMethods_ReturnsFluentSqlBuilder()
    {
        //Arrange
        var expectedSql = $"SELECT *{Environment.NewLine}FROM Table{Environment.NewLine}GROUP BY Id, Type";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"Table")
            .GroupBy($"Id")
            .GroupBy($"Type");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(0);
    }

    [Fact]
    public void Select_BuildSqlWithHavingMethods_ReturnsFluentSqlBuilder()
    {
        //Arrange
        var expectedSql = $"SELECT Id, COUNT(Type) AS TypeCount" +
            $"{Environment.NewLine}FROM Table" +
            $"{Environment.NewLine}HAVING COUNT(Type) > 1 AND COUNT(Type) < 100";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Select($"Id")
            .Select($"COUNT(Type) AS TypeCount")
            .From($"Table")
            .Having($"COUNT(Type) > 1")
            .Having($"COUNT(Type) < 100");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(0);
    }

    [Fact]
    public void Select_BuildSqlWithOrderByMethods_ReturnsFluentSqlBuilder()
    {
        //Arrange
        var expectedSql = $"SELECT *{Environment.NewLine}FROM Table{Environment.NewLine}ORDER BY Id, Type";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"Table")
            .OrderBy($"Id")
            .OrderBy($"Type");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(0);
    }

    [Fact]
    public void Select_BuildSqlWithJoinConditionalMethods_ReturnsFluentSqlBuilder()
    {
        //Arrange
        var expectedSql = $"SELECT *" +
            $"{Environment.NewLine}FROM Table1" +
            $"{Environment.NewLine}LEFT JOIN Table3 ON Table1.Id = Table3.Id" +
            $"{Environment.NewLine}INNER JOIN Table2 ON Table1.Id = Table2.Id";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"Table1")
            .InnerJoin(false, $"Table2 ON Table1.Id = Table2.Id")
            .LeftJoin(true, $"Table3 ON Table1.Id = Table3.Id")
            .RightJoin(false, $"Table4 ON Table1.Id = Table4.Id")
            .InnerJoin(true, $"Table2 ON Table1.Id = Table2.Id");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(0);
    }

    [Theory]
    [AutoData]
    public void Select_BuildSqlWithWhereConditonalMethods_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"SELECT *{Environment.NewLine}FROM Table{Environment.NewLine}WHERE (Age = @p0 OR Type = @p1 AND Age IN (1, 2, 3)) AND Type LIKE '%Type' AND (Age > 10 AND Type = @p2) OR Id NOT IN (1, 2, 3)";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"Table")
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
    public void Select_BuildSqlWithInnerFormattableString_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"SELECT *{Environment.NewLine}FROM Table{Environment.NewLine}WHERE Id = @p0 AND TypeId IN (SELECT TypeId WHERE Type = @p1)";
        FormattableString subQuery = $"SELECT TypeId WHERE Type = {type}";

        //Act
        var builder = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"Table")
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
    public void Select_BuildSqlWithCustomParameterPrefix_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"SELECT *{Environment.NewLine}FROM Table{Environment.NewLine}WHERE Id = :p0 OR Age = :p1 AND Type = :p2";

        //Act
        var builder = SimpleBuilder.CreateFluent(parameterPrefix: ":")
            .Select($"*")
            .From($"Table")
            .Where($"Id = {id}")
            .OrWhere($"Age = {age}")
            .Where($"Type = {type}");

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
    public void Select_BuildSqlAndReuseParameters_ReturnsFluentSqlBuilder(int id, string type)
    {
        //Arrange
        var expectedSql = $"SELECT *{Environment.NewLine}FROM Table{Environment.NewLine}WHERE (Id = @p0 AND Type = @p1) OR Id = @p0 OR Type = @p1";

        //Act
        var builder = SimpleBuilder.CreateFluent(reuseParameters: true)
            .Select($"*")
            .From($"Table")
            .WhereFilter($"Id = {id}").WithFilter($"Type = {type}")
            .OrWhere($"Id = {id}")
            .OrWhere($"Type = {type}");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<string>("p1").Should().Be(type);
    }

    [Theory]
    [AutoData]
    public void Select_BuildSqlAndUseLowerCaseClauses_ReturnsFluentSqlBuilder(int id, int age, string type)
    {
        //Arrange
        var expectedSql = $"select *" +
            $"{Environment.NewLine}from Table1" +
            $"{Environment.NewLine}inner join Table2 on Table1.Id = Table2.Id" +
            $"{Environment.NewLine}left join Table3 on Table1.Id = Table3.Id" +
            $"{Environment.NewLine}right join Table4 on Table1.Id = Table4.Id" +
            $"{Environment.NewLine}where Table1.Id = @p0 or (Table1.Age = @p1 and Table1.Type = @p2)" +
            $"{Environment.NewLine}group by Table1.Id" +
            $"{Environment.NewLine}having count(Table1.Type) > 1" +
            $"{Environment.NewLine}order by Table1.Type";

        //Act
        var builder = SimpleBuilder.CreateFluent(useLowerCaseClauses: true)
            .Select($"*")
            .From($"Table1")
            .InnerJoin($"Table2 on Table1.Id = Table2.Id")
            .LeftJoin($"Table3 on Table1.Id = Table3.Id")
            .RightJoin($"Table4 on Table1.Id = Table4.Id")
            .Where($"Table1.Id = {id}")
            .OrWhereFilter($"Table1.Age = {age}").WithFilter($"Table1.Type = {type}")
            .GroupBy($"Table1.Id")
            .Having($"count(Table1.Type) > 1")
            .OrderBy($"Table1.Type");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(3);
        builder.GetValue<int>("p0").Should().Be(id);
        builder.GetValue<int>("p1").Should().Be(age);
        builder.GetValue<string>("p2").Should().Be(type);
    }

    [Fact]
    public void SelectDistinct_BuildSqlAndUseLowerCaseClauses_ReturnsFluentSqlBuilder()
    {
        //Arrange
        var expectedSql = $"select distinct Id, Type{Environment.NewLine}from Table";

        //Act
        var builder = SimpleBuilder.CreateFluent(useLowerCaseClauses: true)
            .SelectDistinct($"Id, Type")
            .From($"Table");

        //Assert
        builder.Should().BeOfType<FluentSqlBuilder>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(0);
    }
}
