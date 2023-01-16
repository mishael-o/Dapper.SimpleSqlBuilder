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
        SimpleBuilderSettings.DatabaseParameterNameTemplate.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate);
        SimpleBuilderSettings.DatabaseParameterPrefix.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterPrefix);
        SimpleBuilderSettings.ReuseParameters.Should().Be(SimpleBuilderSettings.DefaultReuseParameters);
    }

    [Theory]
    [TestPriority(2)]
    [InlineData("param", ":", true)]
    public void Configure_AllArgumentsPassed_ReturnsVoid(string parameterNameTemplate, string parameterPrefix, bool reuseParameters)
    {
        //Act
        SimpleBuilderSettings.Configure(parameterNameTemplate, parameterPrefix, reuseParameters);

        //Assert
        SimpleBuilderSettings.DatabaseParameterNameTemplate.Should().Be(parameterNameTemplate);
        SimpleBuilderSettings.DatabaseParameterPrefix.Should().Be(parameterPrefix);
        SimpleBuilderSettings.ReuseParameters.Should().Be(reuseParameters);
    }

    [Theory]
    [TestPriority(3)]
    [InlineData("myParam", null, null, "myParam", "@", false)]
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
        SimpleBuilderSettings.DatabaseParameterNameTemplate.Should().Be(expectedParameterNameTemplate);
        SimpleBuilderSettings.DatabaseParameterPrefix.Should().Be(expectedParameterPrefix);
        SimpleBuilderSettings.ReuseParameters.Should().Be(expectedReuseParameters);
    }
}
