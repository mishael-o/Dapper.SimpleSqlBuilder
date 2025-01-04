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
            .WithMessage($"'{nameof(SimpleBuilderOptions.DatabaseParameterNameTemplate)}' cannot be null, empty, or white-space.*")
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
            .WithMessage($"'{nameof(SimpleBuilderOptions.DatabaseParameterPrefix)}' cannot be null, empty, or white-space.*")
            .WithParameterName(nameof(SimpleBuilderOptions.DatabaseParameterPrefix));
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(null)]
    [InlineAutoData("   ")]
    public void CollectionParameterTemplateFormat_SetValueIsNullOrWhiteSpace_ThrowsArgumentException(string value, [NoAutoProperties] SimpleBuilderOptions sut)
    {
        // Act
        var act = () => sut.CollectionParameterTemplateFormat = value;

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"'{nameof(SimpleBuilderOptions.CollectionParameterTemplateFormat)}' cannot be null, empty, or white-space.*")
            .WithParameterName(nameof(SimpleBuilderOptions.CollectionParameterTemplateFormat));
    }

    [Theory]
    [InlineAutoData("{1}")]
    [InlineAutoData("{0:C}")]
    [InlineAutoData("{}")]
    [InlineAutoData("{2}")]
    public void CollectionParameterTemplateFormat_SetValueIsNotValidFormat_ThrowsArgumentException(string value, [NoAutoProperties] SimpleBuilderOptions sut)
    {
        // Act
        var act = () => sut.CollectionParameterTemplateFormat = value;

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"'{nameof(SimpleBuilderOptions.CollectionParameterTemplateFormat)}' must contain a format placeholder '{{0}}' for the index.*")
            .WithParameterName(nameof(SimpleBuilderOptions.CollectionParameterTemplateFormat));
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
        // Arrange
        const string collectionParameterTemplateFormat = "col{0}_";

        // Act
        sut.DatabaseParameterNameTemplate = parameterNameTemplate;
        sut.DatabaseParameterPrefix = parameterPrefix;
        sut.CollectionParameterTemplateFormat = collectionParameterTemplateFormat;
        sut.ReuseParameters = reuseParameters;
        sut.UseLowerCaseClauses = useLowerCaseClauses;

        // Assert
        sut.DatabaseParameterNameTemplate.Should().Be(parameterNameTemplate);
        sut.DatabaseParameterPrefix.Should().Be(parameterPrefix);
#if NET8_0_OR_GREATER
        sut.CollectionParameterFormat.Format.Should().Be(parameterNameTemplate + collectionParameterTemplateFormat);
#else
        sut.CollectionParameterFormat.Should().Be(parameterNameTemplate + collectionParameterTemplateFormat);
#endif
        sut.ReuseParameters.Should().Be(reuseParameters);
        sut.UseLowerCaseClauses.Should().Be(useLowerCaseClauses);
    }
}
