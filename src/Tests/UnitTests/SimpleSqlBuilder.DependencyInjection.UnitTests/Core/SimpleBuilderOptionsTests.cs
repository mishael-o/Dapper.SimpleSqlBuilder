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
        var exception = act.ShouldThrow<ArgumentException>();
        exception.Message.ShouldStartWith($"'{nameof(SimpleBuilderOptions.DatabaseParameterNameTemplate)}' cannot be null, empty, or white-space.");
        exception.ParamName.ShouldBe(nameof(SimpleBuilderOptions.DatabaseParameterNameTemplate));
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
        var exception = act.ShouldThrow<ArgumentException>();
        exception.Message.ShouldStartWith($"'{nameof(SimpleBuilderOptions.DatabaseParameterPrefix)}' cannot be null, empty, or white-space.");
        exception.ParamName.ShouldBe(nameof(SimpleBuilderOptions.DatabaseParameterPrefix));
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
        var exception = act.ShouldThrow<ArgumentException>();
        exception.Message.ShouldStartWith($"'{nameof(SimpleBuilderOptions.CollectionParameterTemplateFormat)}' cannot be null, empty, or white-space.");
        exception.ParamName.ShouldBe(nameof(SimpleBuilderOptions.CollectionParameterTemplateFormat));
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
        var exception = act.ShouldThrow<ArgumentException>();
        exception.Message.ShouldStartWith($"'{nameof(SimpleBuilderOptions.CollectionParameterTemplateFormat)}' must contain a format placeholder '{{0}}' for the index.");
        exception.ParamName.ShouldBe(nameof(SimpleBuilderOptions.CollectionParameterTemplateFormat));
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
        sut.DatabaseParameterNameTemplate.ShouldBe(parameterNameTemplate);
        sut.DatabaseParameterPrefix.ShouldBe(parameterPrefix);
#if NET8_0_OR_GREATER
        sut.CollectionParameterFormat.Format.ShouldBe(parameterNameTemplate + collectionParameterTemplateFormat);
#else
        sut.CollectionParameterFormat.ShouldBe(parameterNameTemplate + collectionParameterTemplateFormat);
#endif
        sut.ReuseParameters.ShouldBe(reuseParameters);
        sut.UseLowerCaseClauses.ShouldBe(useLowerCaseClauses);
    }
}
