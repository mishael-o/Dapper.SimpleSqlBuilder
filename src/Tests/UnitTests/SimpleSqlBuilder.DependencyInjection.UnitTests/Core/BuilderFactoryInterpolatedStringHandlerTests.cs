namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Core;

public class BuilderFactoryInterpolatedStringHandlerTests
{
    [Fact]
    public void Constructor_BuilderIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        ISimpleBuilder builderFactory = null!;

        // Act
        Action act = () => _ = new BuilderFactoryInterpolatedStringHandler(0, 0, builderFactory);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("builderFactory");
    }

    [Theory]
    [AutoData]
    public void Constructor_BuilderDoesNotImplementIBuilderFormatter_ThrowsInvalidOperationException(Mock<ISimpleBuilder> builderFactoryMock)
    {
        // Arrange
        builderFactoryMock
            .Setup(x => x.Create(null, null, null))
            .Returns(default(Builder)!);

        // Act
        Action act = () => _ = new BuilderFactoryInterpolatedStringHandler(0, 0, builderFactoryMock.Object);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("ISimpleBuilder.Create does not return a builder that implements IBuilderFormatter.");
    }

    [Theory]
    [AutoData]
    public void AppendLiteral_AppendsLiteral_ReturnsVoid(string value, Mock<ISimpleBuilder> builderFactoryMock, Mock<Builder> builderMock)
    {
        // Arrange
        var builderFormatterMock = builderMock.As<IBuilderFormatter>();

        builderFactoryMock
            .Setup(x => x.Create(null, null, null))
            .Returns(builderMock.Object);

        var sut = new BuilderFactoryInterpolatedStringHandler(0, 0, builderFactoryMock.Object);

        // Act
        sut.AppendLiteral(value);

        // Assert
        builderFormatterMock.Verify(x => x.AppendLiteral(value));
    }

    [Theory]
    [InlineAutoData("value", null)]
    [InlineAutoData("value", "raw")]
    public void AppendFormatted_AppendsFormatted_ReturnsVoid(string value, string? format, Mock<ISimpleBuilder> builderFactoryMock, Mock<Builder> builderMock)
    {
        // Arrange
        var builderFormatterMock = builderMock.As<IBuilderFormatter>();

        builderFactoryMock
            .Setup(x => x.Create(null, null, null))
            .Returns(builderMock.Object);

        var sut = new BuilderFactoryInterpolatedStringHandler(0, 0, builderFactoryMock.Object);

        // Act
        sut.AppendFormatted(value, format);

        // Assert
        builderFormatterMock.Verify(x => x.AppendFormatted(value, format));
    }

    [Fact]
    public void GetBuilder_FormatterIsNull_ThrowsInvalidOperationException()
    {
        // Act
        var act = () =>
        {
            var sut = default(BuilderFactoryInterpolatedStringHandler);
            _ = sut.GetBuilder();
        };

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The formatter is null. Ensure BuilderFactoryInterpolatedStringHandler is properly initialized.");
    }
}
