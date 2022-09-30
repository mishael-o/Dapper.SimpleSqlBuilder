namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Core;

public class SimpleBuilderOptionsTests
{
    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(null)]
    [InlineAutoData("   ")]
    public void Set_DatabaseParameterNameTemplateIsNullOrWhiteSpace_ThrowsArgumentException(string value, [NoAutoProperties] SimpleBuilderOptions sut)
    {
        //Act
        var act = () => sut.DatabaseParameterNameTemplate = value;

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"'{nameof(value)}' cannot be null or whitespace.*")
            .WithParameterName(nameof(value));
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(null)]
    [InlineAutoData("   ")]
    public void Set_DatabaseParameterPrefixIsNullOrWhiteSpace_ThrowsArgumentException(string value, [NoAutoProperties] SimpleBuilderOptions sut)
    {
        //Act
        var act = () => sut.DatabaseParameterPrefix = value;

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"'{nameof(value)}' cannot be null or whitespace.*")
            .WithParameterName(nameof(value));
    }

    [Theory]
    [AutoData]
    public void SimpleBuilderOptions_SetAllProperties_ReturnsVoid(string parameterNameTemplate, string parameterPrefix, bool reuseParameters, [NoAutoProperties] SimpleBuilderOptions sut)
    {
        //Act
        sut.DatabaseParameterNameTemplate = parameterNameTemplate;
        sut.DatabaseParameterPrefix = parameterPrefix;
        sut.ReuseParameters = reuseParameters;

        //Assert
        sut.DatabaseParameterNameTemplate.Should().Be(parameterNameTemplate);
        sut.DatabaseParameterPrefix.Should().Be(parameterPrefix);
        sut.ReuseParameters.Should().Be(reuseParameters);
    }
}