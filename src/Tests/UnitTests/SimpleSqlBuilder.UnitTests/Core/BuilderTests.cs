using Dapper.SimpleSqlBuilder.Extensions;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

public class BuilderTests
{
    [Fact]
    public void Create_CreatesBuilder_ReturnsSqlBuilder()
    {
        // Act
        var sut = SimpleBuilder.Create();

        // Assert
        sut.Should().BeOfType<SqlBuilder>();
        sut.Sql.Should().BeEmpty();
        sut.ParameterNames.Should().BeEmpty();
        sut.Parameters.Should().BeOfType<DynamicParameters>();
    }

    [Fact]
    public void Create_CreatesBuilderWithInterpolatedString_ReturnsSqlBuilder()
    {
        // Arrange
        const string expectedSql = "SELECT * FROM TABLE";

        // Act
        var sut = SimpleBuilder.Create($"SELECT * FROM TABLE");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public void Create_CreatesBuilderWithInterpolatedStringAndRawValue_ReturnsSqlBuilder(int id, string name, IEnumerable<int> ages)
    {
        // Arrange
        var expectedSql = $"SELECT * FROM TABLE WHERE ID = {id} AND WHERE NAME = @p0 AND Age IN @pc1_";

        // Act
        var sut = SimpleBuilder.Create($"SELECT * FROM TABLE WHERE ID = {id:raw} AND WHERE NAME = {name} AND Age IN {ages}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Count().Should().Be(2);
        sut.GetValue<string>("p0").Should().Be(name);
        sut.GetValue<IEnumerable<int>>("pc1_").Should().BeEquivalentTo(ages);
    }

#if !NET6_0_OR_GREATER

    [Fact]
    public void AppendIntact_AppendsNullFormattableString_ReturnsSqlBuilder()
    {
        // Arrange
        var sut = SimpleBuilder.Create();

        FormattableString formattable = null!;

        // Act
        var result = sut.AppendIntact(formattable);

        // Assert
        result.Should().Be(sut);
        sut.Sql.Should().BeEmpty();
        sut.ParameterNames.Should().BeEmpty();
    }

#endif

    [Fact]
    public void AppendIntact_AppendsInterpolatedStringWithNoArguments_ReturnsSqlBuilder()
    {
        // Arrange
        var sut = SimpleBuilder.Create();

        // Act
        var result = sut.AppendIntact($"SELECT * FROM TABLE");

        // Assert
        result.Should().Be(sut);
        sut.Sql.Should().Be("SELECT * FROM TABLE");
        sut.ParameterNames.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public void AppendIntact_AppendsInterpolatedStringWithRawValue_ReturnsSqlBuilder(int id, string name, int[] ages)
    {
        // Arrange
        var expectedSql = $"SELECT * FROM TABLE WHERE ID = {id} AND WHERE NAME = @p0 AND Age IN @pc1_";

        var sut = SimpleBuilder.Create();

        // Act
        sut.AppendIntact($"SELECT * FROM TABLE WHERE ID = {id:raw} AND WHERE NAME = {name} AND Age IN {ages}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Count().Should().Be(2);
        sut.GetValue<string>("p0").Should().Be(name);
        sut.GetValue<int[]>("pc1_").Should().BeEquivalentTo(ages);
    }

    [Theory]
    [AutoData]
    public void AppendIntact_AppendsInterpolatedStringWithArguments_ReturnsSqlBuilder(int id, string name, int[] ages)
    {
        // Arrange
        const string expectedSql = "SELECT * FROM TABLE WHERE ID = @p0 AND NAME = @p1 AND Age IN @pc2_";

        var sut = SimpleBuilder.Create();

        // Act
        sut.AppendIntact($"SELECT * FROM TABLE WHERE ID = {id} AND NAME = {name} AND Age IN {ages}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Count().Should().Be(3);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(name);
        sut.GetValue<int[]>("pc2_").Should().BeEquivalentTo(ages);
    }

    [Theory]
    [AutoData]
    public void AppendIntact_AppendsInterpolatedStringWithArgumentsAndRaw_ReturnsSqlBuilder(int id, string name, int secondId, string type)
    {
        // Arrange
        var expectedSql = $"SELECT * FROM TABLE WHERE ID = @p0 AND NAME = @p1 AND TYPE = '{type}' AND SECOND_ID = {secondId}";

        var sut = SimpleBuilder.Create();

        // Act
        sut.AppendIntact($"SELECT * FROM TABLE WHERE ID = {id.DefineParam(System.Data.DbType.Int32)} AND NAME = {name} AND TYPE = '{type:raw}' AND SECOND_ID = {secondId:raw}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Count().Should().Be(2);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(name);
    }

    [Theory]
    [AutoData]
    public void AppendIntact_AppendsInterpolatedStringWithinFormattableString_ReturnsSqlBuilder(int id, string name, int rowNum, ICollection<int> ages)
    {
        // Arrange
        const string expectedSql = "SELECT * FROM (SELECT m.*, (SELECT DESCRIPTION FROM TABLE2 WHERE ID = @p0) DESCRIPTION FROM TABLE m WHERE NAME = @p1 AND Age IN @pc2_) WHERE ROWNUM > @p3";

        FormattableString subQuery = $"SELECT DESCRIPTION FROM TABLE2 WHERE ID = {id}";
        FormattableString innerQuery = $"SELECT m.*, ({subQuery}) DESCRIPTION FROM TABLE m WHERE NAME = {name} AND Age IN {ages}";

        var sut = SimpleBuilder.Create();

        // Act
        sut.AppendIntact($"SELECT * FROM ({innerQuery}) WHERE ROWNUM > {rowNum}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Count().Should().Be(4);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(name);
        sut.GetValue<ICollection<int>>("pc2_").Should().BeEquivalentTo(ages);
        sut.GetValue<int>("p3").Should().Be(rowNum);
    }

    [Theory]
    [InlineData(false, "")]
    [InlineData(true, "SELECT * FROM TABLE WHERE ID = @p0")]
    public void AppendIntact_AppendsInterpolatedStringWithCondition_ReturnsSqlBuilder(bool condition, string expectedSql)
    {
        // Act
        var sut = SimpleBuilder.Create()
            .AppendIntact(condition, $"SELECT * FROM TABLE WHERE ID = {1}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
    }

    [Theory]
    [AutoData]
    public void Append_AppendsInterpolatedString_ReturnsSqlBuilder(string tableName, int id, string typeCode, int rowNum, HashSet<string> codes)
    {
        // Arrange
        string expectedSql = $" WHERE ID = @p0 AND TypeId IN (SELECT TypeId FROM {tableName} WHERE TypeCode = @p1 AND Codes IN @pc2_) ROWNUM <= {rowNum}";
        FormattableString subQuery = $"SELECT TypeId FROM {tableName:raw} WHERE TypeCode = {typeCode} AND Codes IN {codes}";
        var sut = SimpleBuilder.Create();

        // Act
        sut.Append($"WHERE ID = {id} AND TypeId IN ({subQuery}) ROWNUM <= {rowNum:raw}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Count().Should().Be(3);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(typeCode);
        sut.GetValue<HashSet<string>>("pc2_").Should().BeEquivalentTo(codes);
    }

    [Theory]
    [InlineData(false, "")]
    [InlineData(true, " WHERE ID = @p0")]
    public void Append_AppendsInterpolatedStringWithCondition_ReturnsSqlBuilder(bool condition, string expectedSql)
    {
        // Act
        var sut = SimpleBuilder.Create()
            .Append(condition, $"WHERE ID = {1}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
    }

    [Fact]
    public void AppendNewLine_AppendsNewLine_ReturnsSqlBuilder()
    {
        // Arrange
        var sut = SimpleBuilder.Create();

        // Act
        var result = sut.AppendNewLine();

        // Assert
        result.Should().Be(sut);
        sut.Sql.Should().Be(Environment.NewLine);
        sut.ParameterNames.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(BuilderTestsData.AppendNewLineWithConditionData), MemberType = typeof(BuilderTestsData))]
    public void AppendNewLine_AppendNewLineWithCondition_ReturnsSqlBuilder(bool condition, string expectedSql)
    {
        // Act
        var sut = SimpleBuilder.Create()
            .AppendNewLine(condition, $"WHERE ID = {1}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
    }

    [Theory]
    [AutoData]
    public void AppendNewLine_AppendsNewLineAndInterpolatedString_ReturnsSqlBuilder(string tableName, int id, string typeCode, int rowNum)
    {
        // Arrange
        var expectedSql = $"{Environment.NewLine}WHERE ID = @p0 AND TypeId IN (SELECT TypeId FROM {tableName} WHERE TypeCode = @p1) ROWNUM <= {rowNum}";
        FormattableString subQuery = $"SELECT TypeId FROM {tableName:raw} WHERE TypeCode = {typeCode}";

        var sut = SimpleBuilder.Create();

        // Act
        sut.AppendNewLine($"WHERE ID = {id} AND TypeId IN ({subQuery}) ROWNUM <= {rowNum:raw}");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Count().Should().Be(2);
        sut.GetValue<int>("p0").Should().Be(id);
        sut.GetValue<string>("p1").Should().Be(typeCode);
    }

    [Theory]
    [AutoData]
    public void AddParameter_AddsParameter_ReturnsVoid(int id)
    {
        // Arrange
        var sut = SimpleBuilder.Create();

        // Act
        var result = sut.AddParameter(nameof(id), id);

        // Assert
        result.Should().Be(sut);
        sut.GetValue<int>(nameof(id)).Should().Be(id);
    }

    [Theory]
    [AutoData]
    public void AddDynamicParameter_AddsDynamicParameters_ReturnsVoid(string param1Name, int param1Value, string param2Name, string param2Value)
    {
        // Arrange
        var dynamicParameters = new DynamicParameters();
        dynamicParameters.Add(param1Name, param1Value);
        dynamicParameters.Add(param2Name, param2Value);

        var sut = SimpleBuilder.Create();

        // Act
        var result = sut.AddDynamicParameters(dynamicParameters);

        // Assert
        result.Should().Be(sut);
        sut.GetValue<int>(param1Name).Should().Be(param1Value);
        sut.GetValue<string>(param2Name).Should().Be(param2Value);
    }

    [Theory]
    [AutoData]
    public void AddOperator_AddsInterpolatedString_ReturnsSqlBuilder(int id)
    {
        // Arrange
        Builder sut = SimpleBuilder.Create();

        // Act
        var result = sut += $"SELECT * FROM TABLE WHERE ID = {id}";

        // Assert
        result.Should().Be(sut);
        sut.Sql.Should().Be("SELECT * FROM TABLE WHERE ID = @p0");
        sut.GetValue<int>("p0").Should().Be(id);
    }

    [Fact]
    public void AddOperator_BuilderIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Builder sut = null!;

        // Act
        var act = () => sut += $"SELECT * FROM TABLE WHERE";

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("builder");
    }

    [Fact]
    public void Creates_CreatesBuilderAndReuseParameters_ReturnsSqlBuilder()
    {
        // Arrange
        var model = new
        {
            Id = 10,
            TypeId = 20,
            Age = default(int?),
            Name = "John",
            MiddleName = default(string)
        };

        string expectedSql = "INSERT INTO TABLE VALUES (@p0, @p1, @p2, @p3, @p4)" +
            $"{Environment.NewLine}INSERT INTO TABLE VALUES (@p0, @p1, @p5, @p3, @p6)";

        // Act
        var sut = SimpleBuilder.Create($"INSERT INTO TABLE VALUES ({model.Id}, {model.TypeId}, {model.Age}, {model.Name}, {model.MiddleName})", reuseParameters: true)
            .AppendNewLine($"INSERT INTO TABLE VALUES ({model.Id}, {model.TypeId}, {model.Age}, {model.Name}, {model.MiddleName})");

        // Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(7);
        sut.GetValue<int>("p0").Should().Be(model.Id);
        sut.GetValue<int>("p1").Should().Be(model.TypeId);
        sut.GetValue<int?>("p2").Should().Be(model.Age);
        sut.GetValue<string>("p3").Should().Be(model.Name);
        sut.GetValue<string?>("p4").Should().Be(model.MiddleName);
        sut.GetValue<int?>("p5").Should().Be(model.Age);
        sut.GetValue<string?>("p6").Should().Be(model.MiddleName);
    }

    [Fact]
    public void Reset_ResetsBuilder_ReturnsVoid()
    {
        // Arrange
        var sut = SimpleBuilder.Create($"SELECT * FROM TABLE WHERE ID = {1}");

        // Act
        sut.Reset();

        // Assert
        sut.Sql.Should().BeEmpty();
        sut.ParameterNames.Should().BeEmpty();
    }

    [Fact]
    public void Reset_ResetsBuilderAndBuildNewQuery_ReturnsVoid()
    {
        // Arrange
        var sut = SimpleBuilder.Create($"SELECT * FROM TABLE WHERE ID = {1}");
        const int id = 2;

        // Act
        sut.Reset();
        sut.AppendIntact($"DELETE FROM TABLE WHERE ID = {id}");

        // Assert
        sut.Sql.Should().Be("DELETE FROM TABLE WHERE ID = @p0");
        sut.ParameterNames.Count().Should().Be(1);
        sut.GetValue<int>("p0").Should().Be(id);
    }

    internal static class BuilderTestsData
    {
        public static TheoryData<bool, string> AppendNewLineWithConditionData
            => new()
            {
                { false, string.Empty },
                { true, Environment.NewLine + "WHERE ID = @p0" }
            };
    }
}
