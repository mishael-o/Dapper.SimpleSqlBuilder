#if NET6_0_OR_GREATER
using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder.Handlers;

public class SelectInterpolatedStringHandlerTests
{
    [Fact]
    public void Constructor_FluentBuilderIsNull_ThrowsArgumentNullException()
    {
        //Arrange
        IFluentBuilder fluentBuilder = null!;

        //Act
        Action act = () => _ = new SelectInterpolatedStringHandler(0, 0, fluentBuilder);

        //Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("builder");
    }

    [Theory]
    [AutoData]
    public void Constructor_InitialiseHandler_HandlerInitialised(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();

        //Act
        var sut = new SelectInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Assert
        fluentSqlFormatterMock.Verify(x => x.StartClauseAction(ClauseAction.Select));
    }

    [Theory]
    [AutoData]
    public void AppendLiteral_LiteralValueAppended_ReturnsVoid(string value, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();
        var sut = new SelectInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Act
        sut.AppendLiteral(value);

        //Assert
        fluentSqlFormatterMock.Verify(x => x.AppendLiteral(value));
    }

    [Theory]
    [AutoData]
    public void AppendFormatted_FormattedValueAppended_ReturnsVoid(string value, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();
        var sut = new SelectInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Act
        sut.AppendFormatted(value);

        //Assert
        fluentSqlFormatterMock.Verify(x => x.AppendFormatted(value, null));
    }

    [Theory]
    [InlineAutoData(0, null)]
    [InlineAutoData("value", "raw")]
    public void AppendFormatted_FormattedValueWithFormatAppended_ReturnsVoid(object value, string? format, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();
        var sut = new SelectInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Act
        sut.AppendFormatted(value, format);

        //Assert
        fluentSqlFormatterMock.Verify(x => x.AppendFormatted(value, format));
    }

    [Theory]
    [AutoData]
    public void Close_ClauseActionEnded_ReturnsVoid(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();
        var sut = new SelectInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Act
        sut.Close();

        //Assert
        fluentSqlFormatterMock.Verify(x => x.EndClauseAction(ClauseAction.Select));
    }
}
#endif
