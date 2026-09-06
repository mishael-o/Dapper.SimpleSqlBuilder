#if NET8_0_OR_GREATER
using Dapper.SimpleSqlBuilder.FluentBuilder;
using Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder.Handlers;

public class InnerJoinInterpolatedStringHandlerTests
{
    [Fact]
    public void Constructor_FluentBuilderIsNull_ThrowsArgumentException()
    {
        // Arrange
        IFluentBuilder fluentBuilder = null!;

        // Act
        Action act = () => _ = new InnerJoinInterpolatedStringHandler(0, 0, fluentBuilder, out var _);

        // Assert
        var exception = act.ShouldThrow<ArgumentException>();
        exception.Message.ShouldMatch("The builder must implement IFluentBuilderFormatter.*");
        exception.ParamName.ShouldBe("builder");
    }

    [Theory]
    [AutoNSubstituteData]
    public void Constructor_BuilderDoesNotImplementIFluentBuilderFormatter_ThrowsArgumentException(IFluentBuilder fluentBuilder)
    {
        // Act
        Action act = () => _ = new InnerJoinInterpolatedStringHandler(0, 0, fluentBuilder, out var _);

        // Assert
        var exception = act.ShouldThrow<ArgumentException>();
        exception.Message.ShouldMatch("The builder must implement IFluentBuilderFormatter.*");
        exception.ParamName.ShouldBe("builder");
    }

    [Fact]
    public void Constructor_InitialisesHandler_ReturnsHandler()
    {
        // Arrange
        var fluentBuilder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateFluentBuilder(out var fluentFormatterMock);

        // Act
        _ = new InnerJoinInterpolatedStringHandler(0, 0, fluentBuilder, out var isHandlerEnabled);

        // Assert
        isHandlerEnabled.ShouldBeTrue();
        fluentFormatterMock.Received().StartClauseAction(ClauseAction.InnerJoin);
    }

    [Fact]
    public void Constructor_HandlerDisabledByCondition_ReturnsHandler()
    {
        // Arrange
        const bool condition = false;
        var fluentBuilder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateFluentBuilder(out var _);

        // Act
        _ = new InnerJoinInterpolatedStringHandler(0, 0, condition, fluentBuilder, out var isHandlerEnabled);

        // Assert
        isHandlerEnabled.ShouldBeFalse();
    }

    [Theory]
    [AutoNSubstituteData]
    public void AppendLiteral_AppendsLiteral_ReturnsVoid(string value)
    {
        // Arrange
        var fluentBuilder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateFluentBuilder(out var fluentFormatterMock);
        var sut = new InnerJoinInterpolatedStringHandler(0, 0, fluentBuilder, out var _);

        // Act
        sut.AppendLiteral(value);

        // Assert
        fluentFormatterMock.Received().AppendLiteral(value);
    }

    [Theory]
    [AutoNSubstituteData]
    public void AppendFormatted_AppendsFormatted_ReturnsVoid(string value)
    {
        // Arrange
        var fluentBuilder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateFluentBuilder(out var fluentFormatterMock);
        var sut = new InnerJoinInterpolatedStringHandler(0, 0, fluentBuilder, out var _);

        // Act
        sut.AppendFormatted(value);

        // Assert
        fluentFormatterMock.Received().AppendFormatted(value, null);
    }

    [Theory]
    [InlineAutoData(0, null)]
    [InlineAutoData("value", "raw")]
    public void AppendFormatted_AppendsFormattedWithFormat_ReturnsVoid(object value, string? format)
    {
        // Arrange
        var fluentBuilder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateFluentBuilder(out var fluentFormatterMock);
        var sut = new InnerJoinInterpolatedStringHandler(0, 0, fluentBuilder, out var _);

        // Act
        sut.AppendFormatted(value, format);

        // Assert
        fluentFormatterMock.Received().AppendFormatted(value, format);
    }

    [Fact]
    public void Close_ClosesHandler_ReturnsVoid()
    {
        // Arrange
        var fluentBuilder = Dapper.SimpleSqlBuilder.UnitTests.UnitTestSubstitutes.CreateFluentBuilder(out var fluentFormatterMock);
        var sut = new InnerJoinInterpolatedStringHandler(0, 0, fluentBuilder, out var _);

        // Act
        sut.Close();

        // Assert
        fluentFormatterMock.Received().EndClauseAction();
    }
}
#endif
