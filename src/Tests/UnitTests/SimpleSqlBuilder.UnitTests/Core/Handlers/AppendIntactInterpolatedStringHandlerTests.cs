#if NET6_0_OR_GREATER
namespace Dapper.SimpleSqlBuilder.UnitTests.Core.Handlers;

public class AppendIntactInterpolatedStringHandlerTests
{
    [Fact]
    public void Constructor_BuilderIsNull_ThrowsArgumentException()
    {
        // Arrange
        Builder builder = null!;

        // Act
        Action act = () => _ = new AppendIntactInterpolatedStringHandler(0, 0, builder, out var _);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The builder must implement IBuilderFormatter.*")
            .WithParameterName("builder");
    }

    [Theory]
    [AutoData]
    public void Constructor_BuilderDoesNotImplementIBuilderFormatter_ThrowsArgumentException(Mock<Builder> builderMock)
    {
        // Act
        Action act = () => _ = new AppendIntactInterpolatedStringHandler(0, 0, builderMock.Object, out var _);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The builder must implement IBuilderFormatter.*")
            .WithParameterName("builder");
    }

    [Theory]
    [AutoData]
    public void Constructor_HandlerDisabledByCondition_ReturnsHandler(Mock<Builder> builderMock)
    {
        // Arrange
        const bool condition = false;
        builderMock.As<IBuilderFormatter>();

        // Act
        var sut = new AppendIntactInterpolatedStringHandler(0, 0, condition, builderMock.Object, out var isHandlerEnabled);

        // Assert
        isHandlerEnabled.Should().BeFalse();
    }

    [Theory]
    [AutoData]
    public void AppendLiteral_AppendsLiteral_ReturnsVoid(string value, Mock<Builder> builderMock)
    {
        // Arrange
        var builderFormatterMock = builderMock.As<IBuilderFormatter>();
        var sut = new AppendIntactInterpolatedStringHandler(0, 0, builderMock.Object, out var _);

        // Act
        sut.AppendLiteral(value);

        // Assert
        builderFormatterMock.Verify(x => x.AppendLiteral(value));
    }

    [Theory]
    [AutoData]
    public void AppendFormatted_AppendsFormatted_ReturnsVoid(string value, Mock<Builder> builderMock)
    {
        // Arrange
        var builderFormatterMock = builderMock.As<IBuilderFormatter>();
        var sut = new AppendIntactInterpolatedStringHandler(0, 0, builderMock.Object, out var _);

        // Act
        sut.AppendFormatted(value);

        // Assert
        builderFormatterMock.Verify(x => x.AppendFormatted(value, null));
    }

    [Theory]
    [InlineAutoData(0, null)]
    [InlineAutoData("value", "raw")]
    public void AppendFormatted_AppendsFormattedWithFormat_ReturnsVoid(object value, string? format, Mock<Builder> builderMock)
    {
        // Arrange
        var builderFormatterMock = builderMock.As<IBuilderFormatter>();
        var sut = new AppendIntactInterpolatedStringHandler(0, 0, builderMock.Object, out var _);

        // Act
        sut.AppendFormatted(value, format);

        // Assert
        builderFormatterMock.Verify(x => x.AppendFormatted(value, format));
    }
}
#endif
