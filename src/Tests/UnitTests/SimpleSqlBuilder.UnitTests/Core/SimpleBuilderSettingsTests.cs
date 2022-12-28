namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

[Collection($"~ Run Last - {nameof(SimpleBuilderSettingsTests)}")]
public class SimpleBuilderSettingsTests
{
    [Fact]
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
    [InlineData("param", ":", true, true)]
    public void Configure_ConfigureSettings_ReturnsVoid(string parameterNameTemplate, string parameterPrefix, bool reuseParameters, bool useLowerCaseClauses)
    {
        //Act
        SimpleBuilderSettings.Configure(parameterNameTemplate, parameterPrefix, reuseParameters, useLowerCaseClauses);

        //Assert
        SimpleBuilderSettings.DatabaseParameterNameTemplate.Should().Be(parameterNameTemplate);
        SimpleBuilderSettings.DatabaseParameterPrefix.Should().Be(parameterPrefix);
        SimpleBuilderSettings.ReuseParameters.Should().Be(reuseParameters);
        SimpleBuilderSettings.UseLowerCaseClauses.Should().Be(useLowerCaseClauses);
    }
}
