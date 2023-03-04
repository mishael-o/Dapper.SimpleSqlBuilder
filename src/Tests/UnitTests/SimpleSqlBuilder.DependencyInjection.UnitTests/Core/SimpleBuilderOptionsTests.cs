namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Core;

public class SimpleBuilderOptionsTests
{
    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(null)]
    [InlineAutoData("   ")]
    public void DatabaseParameterNameTemplate_SetValueIsNullOrWhiteSpace_ThrowsArgumentException(string value, [NoAutoProperties] SimpleBuilderOptions sut)
    {
        // Act
        var act = () => sut.DatabaseParameterNameTemplate = value;

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"'{nameof(SimpleBuilderOptions.DatabaseParameterNameTemplate)}' cannot be null or whitespace.*")
            .WithParameterName(nameof(SimpleBuilderOptions.DatabaseParameterNameTemplate));
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(null)]
    [InlineAutoData("   ")]
    public void DatabaseParameterPrefix_SetValueIsNullOrWhiteSpace_ThrowsArgumentException(string value, [NoAutoProperties] SimpleBuilderOptions sut)
    {
        // Act
        var act = () => sut.DatabaseParameterPrefix = value;

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"'{nameof(SimpleBuilderOptions.DatabaseParameterPrefix)}' cannot be null or whitespace.*")
            .WithParameterName(nameof(SimpleBuilderOptions.DatabaseParameterPrefix));
    }

    [Theory]
    [AutoData]
    public void SimpleBuilderOptions_SetAllProperties_ReturnsVoid(
        string parameterNameTemplate,
        string parameterPrefix,
        bool reuseParameters,
        bool useLowerCaseClauses,
        [NoAutoProperties] SimpleBuilderOptions sut)
    {
        // Act
        sut.DatabaseParameterNameTemplate = parameterNameTemplate;
        sut.DatabaseParameterPrefix = parameterPrefix;
        sut.ReuseParameters = reuseParameters;
        sut.UseLowerCaseClauses = useLowerCaseClauses;

        // Assert
        sut.DatabaseParameterNameTemplate.Should().Be(parameterNameTemplate);
        sut.DatabaseParameterPrefix.Should().Be(parameterPrefix);
        sut.ReuseParameters.Should().Be(reuseParameters);
        sut.UseLowerCaseClauses.Should().Be(useLowerCaseClauses);
    }
}
