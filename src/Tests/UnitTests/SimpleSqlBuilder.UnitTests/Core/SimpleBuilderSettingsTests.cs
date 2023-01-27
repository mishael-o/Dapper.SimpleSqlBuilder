using Dapper.SimpleSqlBuilder.UnitTestHelpers.XUnit;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

[Collection($"~ Run Last - {nameof(SimpleBuilderSettingsTests)}")]
[TestCaseOrderer("Dapper.SimpleSqlBuilder.UnitTestHelpers.XUnit.PriorityOrderer", "Dapper.SimpleSqlBuilder.UnitTestHelpers")]
public class SimpleBuilderSettingsTests
{
    [Fact]
    [TestPriority(1)]
    public void Configure_NoArgumentsPassed_ReturnsVoid()
    {
        //Act
        SimpleBuilderSettings.Configure();

        //Assert
        SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate);
        SimpleBuilderSettings.Instance.DatabaseParameterPrefix.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterPrefix);
        SimpleBuilderSettings.Instance.ReuseParameters.Should().Be(SimpleBuilderSettings.DefaultReuseParameters);
        SimpleBuilderSettings.Instance.UseLowerCaseClauses.Should().Be(SimpleBuilderSettings.DefaultUseLowerCaseClauses);
    }

    [Fact]
    [TestPriority(2)]
    public void Configure_CustomParameterNameTemplate_ReturnsVoid()
    {
        //Arrange
        const int id = 10;
        const string parameterNameTemplate = "cstPrm";
        string expectedSql = $"SELECT * FROM TABLE WHERE ID = {SimpleBuilderSettings.Instance.DatabaseParameterPrefix}{parameterNameTemplate}0";

        //Act
        SimpleBuilderSettings.Configure(parameterNameTemplate: parameterNameTemplate);
        var sut = SimpleBuilder.Create($"SELECT * FROM TABLE WHERE ID = {id}");

        //Assert
        sut.Sql.Should().Be(expectedSql);
        sut.GetValue<int>($"{parameterNameTemplate}0").Should().Be(id);
    }

    [Theory]
    [TestPriority(2)]
    [InlineData("param", ":", true, true)]
    public void Configure_ConfigureSettings_ReturnsVoid(string parameterNameTemplate, string parameterPrefix, bool reuseParameters, bool useLowerCaseClauses)
    {
        //Act
        SimpleBuilderSettings.Configure(parameterNameTemplate, parameterPrefix, reuseParameters, useLowerCaseClauses);

        //Assert
        SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate.Should().Be(parameterNameTemplate);
        SimpleBuilderSettings.Instance.DatabaseParameterPrefix.Should().Be(parameterPrefix);
        SimpleBuilderSettings.Instance.ReuseParameters.Should().Be(reuseParameters);
        SimpleBuilderSettings.Instance.UseLowerCaseClauses.Should().Be(useLowerCaseClauses);
    }

    [Theory]
    [TestPriority(3)]
    [InlineData("myParam", null, null, null, "myParam", "@", false, false)]
    [InlineData("", ":", null, null, "prm", ":", false, false)]
    [InlineData(null, null, true, null, "prm", "@", true, false)]
    [InlineData(null, null, null, true, "prm", "@", false, true)]
    public void Configure_ConfigureSingleSetting_ReturnsVoid(
        string? parameterNameTemplate,
        string? parameterPrefix,
        bool? reuseParameters,
        bool? useLowerCaseClauses,
        string expectedParameterNameTemplate,
        string expectedParameterPrefix,
        bool expectedReuseParameters,
        bool expectedUseLowerCaseClauses)
    {
        //Arrange
        SimpleBuilderSettings.Configure("prm", "@", false, false);

        //Act
        SimpleBuilderSettings.Configure(parameterNameTemplate, parameterPrefix, reuseParameters, useLowerCaseClauses);

        //Assert
        SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate.Should().Be(expectedParameterNameTemplate);
        SimpleBuilderSettings.Instance.DatabaseParameterPrefix.Should().Be(expectedParameterPrefix);
        SimpleBuilderSettings.Instance.ReuseParameters.Should().Be(expectedReuseParameters);
        SimpleBuilderSettings.Instance.UseLowerCaseClauses.Should().Be(expectedUseLowerCaseClauses);
    }
}
