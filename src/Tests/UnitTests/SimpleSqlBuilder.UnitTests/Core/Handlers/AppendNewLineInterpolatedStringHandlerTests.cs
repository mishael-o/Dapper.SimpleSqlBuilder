#if NET8_0_OR_GREATER
using Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;
namespace Dapper.SimpleSqlBuilder.UnitTests.Core.Handlers;

public class AppendNewLineInterpolatedStringHandlerTests
{
    [Fact]
    public void Constructor_BuilderIsNull_ThrowsArgumentException()
    {
        // Arrange
        Builder builder = null!;

        // Act
        Action act = () => _ = new AppendNewLineInterpolatedStringHandler(0, 0, builder, out var _);

        // Assert
        var exception = act.ShouldThrow<ArgumentException>();
        exception.Message.ShouldStartWith("The builder must implement IBuilderFormatter.");
        exception.ParamName.ShouldBe("builder");
    }

    [Theory]
    [AutoNSubstituteData]
    public void Constructor_BuilderDoesNotImplementIBuilderFormatter_ThrowsArgumentException(Builder builder)
    {
        // Act
        Action act = () => _ = new AppendNewLineInterpolatedStringHandler(0, 0, builder, out var _);

        // Assert
        var exception = act.ShouldThrow<ArgumentException>();
        exception.Message.ShouldStartWith("The builder must implement IBuilderFormatter.");
        exception.ParamName.ShouldBe("builder");
    }

    [Fact]
    public void Constructor_InitialisesHandler_ReturnsHandler()
    {
        // Arrange
        var builder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateBuilder(out var builderFormatterMock);

        // Act
        _ = new AppendNewLineInterpolatedStringHandler(0, 0, builder, out var _);

        // Assert
        builderFormatterMock.Received().AppendControl(ControlType.NewLine);
    }

    [Fact]
    public void Constructor_HandlerDisabledByCondition_ReturnsHandler()
    {
        // Arrange
        const bool condition = false;
        var builder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateBuilder(out var _);

        // Act
        _ = new AppendNewLineInterpolatedStringHandler(0, 0, condition, builder, out var isHandlerEnabled);

        // Assert
        isHandlerEnabled.ShouldBeFalse();
    }

    [Theory]
    [AutoNSubstituteData]
    public void AppendLiteral_AppendsLiteral_ReturnsVoid(string value)
    {
        // Arrange
        var builder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateBuilder(out var builderFormatterMock);
        var sut = new AppendNewLineInterpolatedStringHandler(0, 0, builder, out var _);

        // Act
        sut.AppendLiteral(value);

        // Assert
        builderFormatterMock.Received().AppendLiteral(value);
    }

    [Theory]
    [AutoNSubstituteData]
    public void AppendFormatted_AppendsFormatted_ReturnsVoid(string value)
    {
        // Arrange
        var builder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateBuilder(out var builderFormatterMock);
        var sut = new AppendNewLineInterpolatedStringHandler(0, 0, builder, out var _);

        // Act
        sut.AppendFormatted(value);

        // Assert
        builderFormatterMock.Received().AppendFormatted(value, null);
    }

    [Theory]
    [InlineAutoData(0, null)]
    [InlineAutoData("value", "raw")]
    public void AppendFormatted_AppendsFormattedWithFormat_ReturnsVoid(object value, string? format)
    {
        // Arrange
        var builder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateBuilder(out var builderFormatterMock);
        var sut = new AppendNewLineInterpolatedStringHandler(0, 0, builder, out var _);

        // Act
        sut.AppendFormatted(value, format);

        // Assert
        builderFormatterMock.Received().AppendFormatted(value, format);
    }
}
#endif
