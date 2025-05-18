using Dapper.SimpleSqlBuilder.UnitTestHelpers.XUnit;

namespace Dapper.SimpleSqlBuilder.UnitTests.Core;

[Collection($"~ Run Last - {nameof(SimpleBuilderSettingsTests)}")]
[TestCaseOrderer("Dapper.SimpleSqlBuilder.UnitTestHelpers.XUnit.PriorityOrderer", "Dapper.SimpleSqlBuilder.UnitTestHelpers")]
public class SimpleBuilderSettingsTests
{
    [Fact]
    [TestPriority(1)]
    public void Configure_ConfiguresDefaultSettings_ReturnsVoid()
    {
        // Arrange
        const string expectedCollectionParameterFormat = SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate + SimpleBuilderSettings.DefaultCollectionParameterTemplateFormat;

        // Act
        SimpleBuilderSettings.Configure();

        // Assert
        SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate.ShouldBe(SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate);
        SimpleBuilderSettings.Instance.DatabaseParameterPrefix.ShouldBe(SimpleBuilderSettings.DefaultDatabaseParameterPrefix);
        SimpleBuilderSettings.Instance.CollectionParameterTemplateFormat.ShouldBe(SimpleBuilderSettings.DefaultCollectionParameterTemplateFormat);
#if NET8_0_OR_GREATER
        SimpleBuilderSettings.Instance.CollectionParameterFormat.Format.ShouldBe(expectedCollectionParameterFormat);
#else
        SimpleBuilderSettings.Instance.CollectionParameterFormat.ShouldBe(expectedCollectionParameterFormat);
#endif
        SimpleBuilderSettings.Instance.ReuseParameters.ShouldBe(SimpleBuilderSettings.DefaultReuseParameters);
        SimpleBuilderSettings.Instance.UseLowerCaseClauses.ShouldBe(SimpleBuilderSettings.DefaultUseLowerCaseClauses);
    }

    [Fact]
    [TestPriority(2)]
    public void Configure_ConfiguresParameterNameTemplate_ReturnsVoid()
    {
        // Arrange
        const int id = 10;
        const string parameterNameTemplate = "cstPrm";
        string expectedSql = $"SELECT * FROM TABLE WHERE ID = {SimpleBuilderSettings.Instance.DatabaseParameterPrefix}{parameterNameTemplate}0";

        // Act
        SimpleBuilderSettings.Configure(parameterNameTemplate: parameterNameTemplate);
        var sut = SimpleBuilder.Create($"SELECT * FROM TABLE WHERE ID = {id}");

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.GetValue<int>($"{parameterNameTemplate}0").ShouldBe(id);
    }

    [Fact]
    [TestPriority(3)]
    public void Configure_ConfiguresParameterNameTemplateFluentBuilder_ReturnsVoid()
    {
        // Arrange
        const int id = 10;
        const string parameterNameTemplate = "fluCstPrm";
        string expectedSql = $"SELECT *{Environment.NewLine}FROM TABLE{Environment.NewLine}WHERE ID = {SimpleBuilderSettings.Instance.DatabaseParameterPrefix}{parameterNameTemplate}0";

        // Act
        SimpleBuilderSettings.Configure(parameterNameTemplate: parameterNameTemplate);
        var sut = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"TABLE")
            .Where($"ID = {id}");

        // Assert
        sut.Sql.ShouldBe(expectedSql);
        sut.GetValue<int>($"{parameterNameTemplate}0").ShouldBe(id);
    }

    [Theory]
    [TestPriority(4)]
    [InlineData("param", ":", "List{0}", true, true)]
    public void Configure_ConfiguresAllSettings_ReturnsVoid(
        string parameterNameTemplate,
        string parameterPrefix,
        string collectionParameterTemplateFormat,
        bool reuseParameters,
        bool useLowerCaseClauses)
    {
        // Arrange
        var expectedCollectionParameterFormat = parameterNameTemplate + collectionParameterTemplateFormat;

        // Act
        SimpleBuilderSettings.Configure(parameterNameTemplate, parameterPrefix, collectionParameterTemplateFormat, reuseParameters, useLowerCaseClauses);

        // Assert
        SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate.ShouldBe(parameterNameTemplate);
        SimpleBuilderSettings.Instance.DatabaseParameterPrefix.ShouldBe(parameterPrefix);
        SimpleBuilderSettings.Instance.CollectionParameterTemplateFormat.ShouldBe(collectionParameterTemplateFormat);
#if NET8_0_OR_GREATER
        SimpleBuilderSettings.Instance.CollectionParameterFormat.Format.ShouldBe(expectedCollectionParameterFormat);
#else
        SimpleBuilderSettings.Instance.CollectionParameterFormat.ShouldBe(expectedCollectionParameterFormat);
#endif
        SimpleBuilderSettings.Instance.ReuseParameters.ShouldBe(reuseParameters);
        SimpleBuilderSettings.Instance.UseLowerCaseClauses.ShouldBe(useLowerCaseClauses);
    }

    [Theory]
    [TestPriority(5)]
    [InlineData(null, null, null, null, null)]
    [InlineData("", "", "", null, null)]
    [InlineData(" ", " ", " ", null, null)]
    [InlineData("   ", "    ", "    ", null, null)]
    public void Configure_ConfiguresSettingsWithNullAndEmptyValues_ReturnsVoid(
        string? parameterNameTemplate,
        string? parameterPrefix,
        string? collectionParameterTemplateFormat,
        bool? reuseParameters,
        bool? useLowerCaseClauses)
    {
        // Arrange
        const string expectedParameterNameTemplate = "prm";
        const string expectedParameterPrefix = "@";
        const string expectedCollectionParameterTemplateFormat = "cl{0}";
        const string expectedCollectionParameterFormat = expectedParameterNameTemplate + expectedCollectionParameterTemplateFormat;
        const bool expectedReuseParameters = false;
        const bool expectedUseLowerCaseClauses = false;

        SimpleBuilderSettings.Configure(
            expectedParameterNameTemplate,
            expectedParameterPrefix,
            expectedCollectionParameterTemplateFormat,
            expectedReuseParameters,
            expectedUseLowerCaseClauses);

        // Act
        SimpleBuilderSettings.Configure(parameterNameTemplate, parameterPrefix, collectionParameterTemplateFormat, reuseParameters, useLowerCaseClauses);

        // Assert
        SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate.ShouldBe(expectedParameterNameTemplate);
        SimpleBuilderSettings.Instance.DatabaseParameterPrefix.ShouldBe(expectedParameterPrefix);
        SimpleBuilderSettings.Instance.CollectionParameterTemplateFormat.ShouldBe(expectedCollectionParameterTemplateFormat);
#if NET8_0_OR_GREATER
        SimpleBuilderSettings.Instance.CollectionParameterFormat.Format.ShouldBe(expectedCollectionParameterFormat);
#else
        SimpleBuilderSettings.Instance.CollectionParameterFormat.ShouldBe(expectedCollectionParameterFormat);
#endif
        SimpleBuilderSettings.Instance.ReuseParameters.ShouldBe(expectedReuseParameters);
        SimpleBuilderSettings.Instance.UseLowerCaseClauses.ShouldBe(expectedUseLowerCaseClauses);
    }

    [Theory]
    [InlineAutoData("{1}")]
    [InlineAutoData("{0:C}")]
    [InlineAutoData("{}")]
    [InlineAutoData("{2}")]
    public void Configure_CollectionParameterTemplateFormatIsNotValidFormat_ThrowsArgumentException(string collectionParameterTemplateFormat)
    {
        // Act
        var act = () => SimpleBuilderSettings.Configure(collectionParameterTemplateFormat: collectionParameterTemplateFormat);

        // Assert
        var exception = act.ShouldThrow<ArgumentException>();
        exception.Message.ShouldStartWith($"'{nameof(collectionParameterTemplateFormat)}' must contain a format placeholder '{{0}}' for the index.");
        exception.ParamName.ShouldBe(nameof(collectionParameterTemplateFormat));
    }
}
