#if NET6_0_OR_GREATER
using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder.Handlers;

public class UpdateSetInterpolatedStringHandlerTests
{
    [Fact]
    public void Constructor_FluentBuilderIsNull_ThrowsArgumentException()
    {
        //Arrange
        IFluentBuilder fluentBuilder = null!;

        //Act
        Action act = () => _ = new UpdateSetInterpolatedStringHandler(0, 0, fluentBuilder, out var _);

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The builder must implement IFluentBuilderFormatter.*")
            .WithParameterName("builder");
    }

    [Theory]
    [AutoData]
    public void Constructor_BuilderDoesNotImplementIFluentBuilderFormatter_ThrowsArgumentException(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Act
        Action act = () => _ = new UpdateSetInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var _);

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The builder must implement IFluentBuilderFormatter.*")
            .WithParameterName("builder");
    }

    [Theory]
    [AutoData]
    public void Constructor_InitialisesHandler_ReturnsHandler(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentFormatterMock = fluentBuilderMock.As<IFluentBuilderFormatter>();

        //Act
        var sut = new UpdateSetInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var isHandlerEnabled);

        //Assert
        isHandlerEnabled.Should().BeTrue();
        fluentFormatterMock.Verify(x => x.StartClauseAction(ClauseAction.UpdateSet));
    }

    [Theory]
    [AutoData]
    public void Constructor_HandlerDisabledByCondition_ReturnsHandler(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        const bool condition = false;
        fluentBuilderMock.As<IFluentBuilderFormatter>();

        //Act
        var sut = new UpdateSetInterpolatedStringHandler(0, 0, condition, fluentBuilderMock.Object, out var isHandlerEnabled);

        //Assert
        isHandlerEnabled.Should().BeFalse();
    }

    [Theory]
    [AutoData]
    public void AppendLiteral_AppendsLiteral_ReturnsVoid(string value, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentFormatterMock = fluentBuilderMock.As<IFluentBuilderFormatter>();
        var sut = new UpdateSetInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var _);

        //Act
        sut.AppendLiteral(value);

        //Assert
        fluentFormatterMock.Verify(x => x.AppendLiteral(value));
    }

    [Theory]
    [AutoData]
    public void AppendFormatted_AppendsFormatted_ReturnsVoid(string value, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentFormatterMock = fluentBuilderMock.As<IFluentBuilderFormatter>();
        var sut = new UpdateSetInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var _);

        //Act
        sut.AppendFormatted(value);

        //Assert
        fluentFormatterMock.Verify(x => x.AppendFormatted(value, null));
    }

    [Theory]
    [InlineAutoData(0, null)]
    [InlineAutoData("value", "raw")]
    public void AppendFormatted_AppendsFormattedWithFormat_ReturnsVoid(object value, string? format, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentFormatterMock = fluentBuilderMock.As<IFluentBuilderFormatter>();
        var sut = new UpdateSetInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var _);

        //Act
        sut.AppendFormatted(value, format);

        //Assert
        fluentFormatterMock.Verify(x => x.AppendFormatted(value, format));
    }

    [Theory]
    [AutoData]
    public void Close_ClosesHandler_ReturnsVoid(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentFormatterMock = fluentBuilderMock.As<IFluentBuilderFormatter>();
        var sut = new UpdateSetInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var _);

        //Act
        sut.Close();

        //Assert
        fluentFormatterMock.Verify(x => x.EndClauseAction());
    }
}
#endif
