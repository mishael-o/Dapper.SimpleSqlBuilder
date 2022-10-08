using Dapper.SimpleSqlBuilder.Extensions;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

public class SimpleBuilderTests
{
    [Theory]
    [MemberData(nameof(SimpleBuilderTestCases.FormattableStringWithRawValue_TestCases), MemberType = typeof(SimpleBuilderTestCases))]
    public void Create_FormattableStringWithRawValue_ReturnsVoid(FormattableString formattableString, string expectedSql)
    {
        //Act
        var sut = SimpleBuilder.Create(formattableString);

        //Assert
        sut.ParameterNames.Should().BeEmpty();
        sut.Parameters.Should().BeOfType<DynamicParameters>();
        sut.Sql.Should().Be(expectedSql);
    }

    [Fact]
    public void AppendIntact_FormattableStringIsNull_ReturnsISimpleBuilder()
    {
        //Arrange
        var sut = SimpleBuilder.Create();
        FormattableString formattable = null!;

        //Act
        var result = sut.AppendIntact(formattable);

        //Assert
        result.Should().Be(sut);
        sut.ParameterNames.Should().BeEmpty();
        sut.Sql.Should().BeEmpty();
    }

    [Fact]
    public void AppendIntact_FormattableStringHasNoArguments_ReturnsISimpleBuilder()
    {
        //Arrange
        var sut = SimpleBuilder.Create();
        FormattableString formattable = $"SELECT * FROM TABLE";

        //Act
        var result = sut.AppendIntact(formattable);

        //Assert
        result.Should().Be(sut);
        sut.ParameterNames.Should().BeEmpty();
        sut.Sql.Should().Be(formattable.Format);
    }

    [Theory]
    [MemberData(nameof(SimpleBuilderTestCases.FormattableStringWithRawValue_TestCases), MemberType = typeof(SimpleBuilderTestCases))]
    public void AppendIntact_FormattableStringWithRawValue_ReturnsISimpleBuilder(FormattableString formattableString, string expectedSql)
    {
        //Arrange
        var sut = SimpleBuilder.Create();

        //Act
        var result = sut.AppendIntact(formattableString);

        //Assert
        result.Should().Be(sut);
        sut.ParameterNames.Should().BeEmpty();
        sut.Sql.Should().Be(expectedSql);
    }

    [Theory]
    [MemberData(nameof(SimpleBuilderTestCases.AppendIntact_FormattableStringWithArguments_TestCases), MemberType = typeof(SimpleBuilderTestCases))]
    public void AppendIntact_FormattableStringWithArguments_ReturnsISimpleBuilder(
        FormattableString formattableString,
        string expectedSql,
        int param1,
        string param2)
    {
        //Arrange
        var sut = SimpleBuilder.Create();

        //Act
        var result = sut.AppendIntact(formattableString);

        //Assert
        result.Should().Be(sut);
        sut.Sql.Should().Be(expectedSql);
        sut.GetValue<int>($"{SimpleBuilderSettings.DatabaseParameterNameTemplate}0").Should().Be(param1);
        sut.GetValue<string>($"{SimpleBuilderSettings.DatabaseParameterNameTemplate}1").Should().Be(param2);
    }

    [Theory]
    [MemberData(nameof(SimpleBuilderTestCases.AppendIntact_FormattableStringWithArgumentsAndRaw_TestCases), MemberType = typeof(SimpleBuilderTestCases))]
    public void AppendIntact_FormattableStringWithArgumentsAndRaw_ReturnsISimpleBuilder(
        FormattableString formattableString,
        string expectedSql,
        int param1,
        string param2)
    {
        //Arrange
        var sut = SimpleBuilder.Create();

        //Act
        var result = sut.AppendIntact(formattableString);

        //Assert
        result.Should().Be(sut);
        sut.Sql.Should().Be(expectedSql);
        sut.GetValue<int>($"{SimpleBuilderSettings.DatabaseParameterNameTemplate}0").Should().Be(param1);
        sut.GetValue<string>($"{SimpleBuilderSettings.DatabaseParameterNameTemplate}1").Should().Be(param2);
    }

    [Theory]
    [MemberData(nameof(SimpleBuilderTestCases.AppendIntact_FormattableStringWithinFormattableString_TestCases), MemberType = typeof(SimpleBuilderTestCases))]
    public void AppendIntact_FormattableStringWithinFormattableString_ReturnsISimpleBuilder(FormattableString formattableString, string expectedSql, int param1, string param2, int param3)
    {
        //Arrange
        var sut = SimpleBuilder.Create();

        //Act
        var result = sut.AppendIntact(formattableString);

        //Assert
        result.Should().Be(sut);
        sut.Sql.Should().Be(expectedSql);
        sut.GetValue<int>($"{SimpleBuilderSettings.DatabaseParameterNameTemplate}0").Should().Be(param1);
        sut.GetValue<string>($"{SimpleBuilderSettings.DatabaseParameterNameTemplate}1").Should().Be(param2);
        sut.GetValue<int>($"{SimpleBuilderSettings.DatabaseParameterNameTemplate}2").Should().Be(param3);
    }

    [Fact]
    public void Append_AppendsFormattableString_ReturnsISimpleBuilder()
    {
        //Arrange
        const int id = 10;
        var expectedParameterName = $"{SimpleBuilderSettings.DatabaseParameterNameTemplate}0";
        FormattableString formattable = $"WHERE ID = {id}";
        var expectedSql = $" WHERE ID = {SimpleBuilderSettings.DatabaseParameterPrefix}{expectedParameterName}";

        var sut = SimpleBuilder.Create();

        //Act
        var result = sut.Append(formattable);

        //Assert
        result.Should().Be(sut);
        sut.GetValue<int>(expectedParameterName).Should().Be(id);
        sut.Sql.Should().Be(expectedSql);
    }

    [Fact]
    public void AppendNewLine_FormattableStringIsNull_ReturnsISimpleBuilder()
    {
        //Arrange
        var sut = SimpleBuilder.Create();

        //Act
        var result = sut.AppendNewLine();

        //Assert
        result.Should().Be(sut);
        sut.Sql.Should().Be(Environment.NewLine);
    }

    [Fact]
    public void AppendNewLine_AppendsNewLineAndFormattableString_ReturnsISimpleBuilder()
    {
        //Arrange
        const int id = 10;
        var expectedParameterName = $"{SimpleBuilderSettings.DatabaseParameterNameTemplate}0";
        FormattableString formattable = $"WHERE ID = {id}";
        var expectedSql = $"{Environment.NewLine}WHERE ID = {SimpleBuilderSettings.DatabaseParameterPrefix}{expectedParameterName}";

        var sut = SimpleBuilder.Create();

        //Act
        var result = sut.AppendNewLine(formattable);

        //Assert
        result.Should().Be(sut);
        sut.GetValue<int>(expectedParameterName).Should().Be(id);
        sut.Sql.Should().Be(expectedSql);
    }

    [Fact]
    public void AddParameter_ParameterAdded_ReturnsVoid()
    {
        //Arrange
        const int id = 10;
        var sut = SimpleBuilder.Create();

        //Act
        var result = sut.AddParameter(nameof(id), id);

        //Assert
        result.Should().Be(sut);
        sut.GetValue<int>(nameof(id)).Should().Be(id);
    }

    [Theory]
    [AutoData]
    public void AddDynamicParameter_AddDynamicParameters_ReturnsVoid(
        string param1Name,
        int param1Value,
        string param2Name,
        string param2Value)
    {
        //Arrange
        var dynamicParamters = new DynamicParameters();
        dynamicParamters.Add(param1Name, param1Value);
        dynamicParamters.Add(param2Name, param2Value);
        var sut = SimpleBuilder.Create();

        //Act
        var result = sut.AddDynamicParameters(dynamicParamters);

        //Assert
        result.Should().Be(sut);
        sut.GetValue<int>(param1Name).Should().Be(param1Value);
        sut.GetValue<string>(param2Name).Should().Be(param2Value);
    }

    [Fact]
    public void AddOpertator_AppendsFormattableString_ReturnsISimpleBuilder()
    {
        //Arrange
        const int id = 10;
        var expectedParameterName = $"{SimpleBuilderSettings.DatabaseParameterNameTemplate}0";
        FormattableString formattable = $"SELECT * FROM TABLE WHERE ID = {id}";
        var expectedSql = $"SELECT * FROM TABLE WHERE ID = {SimpleBuilderSettings.DatabaseParameterPrefix}{expectedParameterName}";

        SimpleBuilderBase sut = SimpleBuilder.Create();

        //Act
        var result = sut += formattable;

        //Assert
        result.Should().Be(sut);
        sut.GetValue<int>(expectedParameterName).Should().Be(id);
        sut.Sql.Should().Be(expectedSql);
    }

    [Fact]
    public void SimpleBuilder_ReusableParameters_ReturnsISimpleBuilder()
    {
        //Arrange
        var model = new { Id = 10, TypeId = 20, Age = default(int?), Name = "John", MiddleName = default(string) };
        const int expectedParameterCount = 7;
        var parameterNamePrefix = SimpleBuilderSettings.DatabaseParameterPrefix + SimpleBuilderSettings.DatabaseParameterNameTemplate;

        var expectedSql =
            $@"INSERT INTO TABLE
               VALUES ({parameterNamePrefix}0, {parameterNamePrefix}1, {parameterNamePrefix}2, {parameterNamePrefix}3, {parameterNamePrefix}4)
               INSERT INTO TABLE
               VALUES ({parameterNamePrefix}0, {parameterNamePrefix}1, {parameterNamePrefix}5, {parameterNamePrefix}3, {parameterNamePrefix}6)";

        //Act
        var sut = SimpleBuilder.Create(
            $@"INSERT INTO TABLE
               VALUES ({model.Id}, {model.TypeId}, {model.Age}, {model.Name}, {model.MiddleName})
               INSERT INTO TABLE
               VALUES ({model.Id}, {model.TypeId}, {model.Age}, {model.Name}, {model.MiddleName})",
            reuseParameters: true);

        //Assert
        sut.Sql.Should().Be(expectedSql);
        sut.ParameterNames.Should().HaveCount(expectedParameterCount);
        sut.GetValue<int>($"{parameterNamePrefix}0").Should().Be(model.Id);
        sut.GetValue<int>($"{parameterNamePrefix}1").Should().Be(model.TypeId);
        sut.GetValue<int?>($"{parameterNamePrefix}2").Should().Be(model.Age);
        sut.GetValue<string>($"{parameterNamePrefix}3").Should().Be(model.Name);
        sut.GetValue<string?>($"{parameterNamePrefix}4").Should().Be(model.MiddleName);
        sut.GetValue<int?>($"{parameterNamePrefix}5").Should().Be(model.Age);
        sut.GetValue<string?>($"{parameterNamePrefix}6").Should().Be(model.MiddleName);
    }

    internal static class SimpleBuilderTestCases
    {
        public static IEnumerable<object[]> FormattableStringWithRawValue_TestCases()
        {
            const int id = 1;
            FormattableString formattableString =
            $@"SELECT * FROM TABLE WHERE ID = {id:raw}
               AND WHERE NAME = {nameof(SimpleBuilderTestCases):raw}";

            var expectedSql =
            $@"SELECT * FROM TABLE WHERE ID = {id}
               AND WHERE NAME = {nameof(SimpleBuilderTestCases)}";

            yield return new object[]
            {
            formattableString, //FormattableString
            expectedSql, //expectedSql
            };
        }

        public static IEnumerable<object[]> AppendIntact_FormattableStringWithArguments_TestCases()
        {
            const int id = 2;
            const string name = "tableName";

            FormattableString formattableString =
            $@"SELECT * FROM TABLE WHERE ID = {id}
               AND NAME = {name}";

            var expectedSql =
            $@"SELECT * FROM TABLE WHERE ID = {SimpleBuilderSettings.DatabaseParameterPrefix}{SimpleBuilderSettings.DatabaseParameterNameTemplate}0
               AND NAME = {SimpleBuilderSettings.DatabaseParameterPrefix}{SimpleBuilderSettings.DatabaseParameterNameTemplate}1";

            yield return new object[]
            {
                formattableString, //FormattableString
                expectedSql, //expectedSql
                id, //param1
                name, //param1
            };
        }

        public static IEnumerable<object[]> AppendIntact_FormattableStringWithArgumentsAndRaw_TestCases()
        {
            const int id = 2;
            const string name = "tableName";

            FormattableString formattableString =
            $@"SELECT * FROM TABLE
               WHERE ID = {id.DefineParam(System.Data.DbType.Int32)}
               AND NAME = {name}
               AND TYPE = '{"myType":raw}'
               AND SECOND_ID = {10:raw}";

            var expectedSql =
            $@"SELECT * FROM TABLE
               WHERE ID = {SimpleBuilderSettings.DatabaseParameterPrefix}{SimpleBuilderSettings.DatabaseParameterNameTemplate}0
               AND NAME = {SimpleBuilderSettings.DatabaseParameterPrefix}{SimpleBuilderSettings.DatabaseParameterNameTemplate}1
               AND TYPE = 'myType'
               AND SECOND_ID = 10";

            yield return new object[]
            {
                formattableString, //FormattableString
                expectedSql, //expectedSql
                id, //param1
                name, //param2
            };
        }

        public static IEnumerable<object[]> AppendIntact_FormattableStringWithinFormattableString_TestCases()
        {
            const int id = 2;
            const int rowNum = 10;
            const string name = "tableName";

            FormattableString subQuery = $"SELECT DESCRIPTION FROM TABLE2 WHERE ID = {id}";

            FormattableString query =
            $@"SELECT m.*,
            ({subQuery}) DESCRIPTION
            FROM TABLE m
            WHERE NAME = {name}";

            FormattableString formattableString = $@"
            SELECT * FROM
            (
            {query}
            ) WHERE ROWNUM > {rowNum}";

            var expectedSql = $@"
            SELECT * FROM
            (
            SELECT m.*,
            (SELECT DESCRIPTION FROM TABLE2 WHERE ID = {SimpleBuilderSettings.DatabaseParameterPrefix}{SimpleBuilderSettings.DatabaseParameterNameTemplate}0) DESCRIPTION
            FROM TABLE m
            WHERE NAME = {SimpleBuilderSettings.DatabaseParameterPrefix}{SimpleBuilderSettings.DatabaseParameterNameTemplate}1
            ) WHERE ROWNUM > {SimpleBuilderSettings.DatabaseParameterPrefix}{SimpleBuilderSettings.DatabaseParameterNameTemplate}2";

            yield return new object[]
            {
                formattableString, //FormattableString
                expectedSql, //expectedSql
                id, //param1
                name, //param2
                rowNum //param3
            };
        }
    }
}
