namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

public class SimpleBuilderSettingsTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Configure_ParameterNameTemplateIsNullOrWhiteSpace_ThrowsArgumentException(string parameterNameTemplate)
    {
        //Act
        Action act = () => SimpleBuilderSettings.Configure(parameterNameTemplate);

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"'{nameof(parameterNameTemplate)}' cannot be null or whitespace.*")
            .WithParameterName(nameof(parameterNameTemplate));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Configure_ParameterPrefixIsNullOrWhiteSpace_ThrowsArgumentException(string parameterPrefix)
    {
        //Act
        Action act = () => SimpleBuilderSettings.Configure(parameterPrefix: parameterPrefix);

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"'{nameof(parameterPrefix)}' cannot be null or whitespace.*")
            .WithParameterName(nameof(parameterPrefix));
    }

    [Fact]
    public void Configure_DefaultValues_ReturnsVoid()
    {
        //Act
        SimpleBuilderSettings.Configure();

        //Assert
        SimpleBuilderSettings.DatabaseParameterNameTemplate.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate);
        SimpleBuilderSettings.DatabaseParameterPrefix.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterPrefix);
        SimpleBuilderSettings.ReuseParameters.Should().Be(SimpleBuilderSettings.DefaultReuseParameters);
    }

    [Theory]
    [InlineData("param", ":", true)]
    public void Configure_ConfigureSettings_ReturnsVoid(string parameterNameTemplate, string parameterPrefix, bool reuseParameters)
    {
        //Act
        SimpleBuilderSettings.Configure(parameterNameTemplate, parameterPrefix, reuseParameters);

        //Assert
        SimpleBuilderSettings.DatabaseParameterNameTemplate.Should().Be(parameterNameTemplate);
        SimpleBuilderSettings.DatabaseParameterPrefix.Should().Be(parameterPrefix);
        SimpleBuilderSettings.ReuseParameters.Should().Be(reuseParameters);
    }
}