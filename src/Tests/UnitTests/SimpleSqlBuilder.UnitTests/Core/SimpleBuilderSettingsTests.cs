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
    [TestPriority(3)]
    [InlineData("param", ":", true)]
    public void Configure_AllArgumentsPassed_ReturnsVoid(string parameterNameTemplate, string parameterPrefix, bool reuseParameters)
    {
        //Act
        SimpleBuilderSettings.Configure(parameterNameTemplate, parameterPrefix, reuseParameters);

        //Assert
        SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate.Should().Be(parameterNameTemplate);
        SimpleBuilderSettings.Instance.DatabaseParameterPrefix.Should().Be(parameterPrefix);
        SimpleBuilderSettings.Instance.ReuseParameters.Should().Be(reuseParameters);
    }

    [Theory]
    [TestPriority(4)]
    [InlineData("myParamz", null, null, "myParamz", "@", false)]
    [InlineData("", ":", null, "prm", ":", false)]
    [InlineData(null, null, true, "prm", "@", true)]
    public void Configure_ConfigureSingleSetting_ReturnsVoid(
        string? parameterNameTemplate,
        string? parameterPrefix,
        bool? reuseParameters,
        string expectedParameterNameTemplate,
        string expectedParameterPrefix,
        bool expectedReuseParameters)
    {
        //Arrange
        SimpleBuilderSettings.Configure("prm", "@", false);

        //Act
        SimpleBuilderSettings.Configure(parameterNameTemplate, parameterPrefix, reuseParameters);

        //Assert
        SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate.Should().Be(expectedParameterNameTemplate);
        SimpleBuilderSettings.Instance.DatabaseParameterPrefix.Should().Be(expectedParameterPrefix);
        SimpleBuilderSettings.Instance.ReuseParameters.Should().Be(expectedReuseParameters);
    }
}