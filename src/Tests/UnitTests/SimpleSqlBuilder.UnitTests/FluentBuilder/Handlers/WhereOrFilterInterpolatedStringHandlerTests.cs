#if NET6_0_OR_GREATER
namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder.Handlers;

public class WhereOrFilterInterpolatedStringHandlerTests
{
    [Theory]
    [AutoData]
    public void Constructor_InitialiseHandler_HandlerInitialised(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();

        //Act
        var sut = new WhereOrFilterInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Assert
        fluentSqlFormatterMock.Verify(x => x.StartClauseAction(ClauseAction.WhereOrFilter));
    }

    [Theory]
    [AutoData]
    public void AppendLiteral_LiteralValueAppended_ReturnsVoid(string value, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();
        var sut = new WhereOrFilterInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Act
        sut.AppendLiteral(value);

        //Assert
        fluentSqlFormatterMock.Verify(x => x.FormatLiteral(value));
    }

    [Theory]
    [AutoData]
    public void AppendFormatted_FormattedValueAppended_ReturnsVoid(string value, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();
        var sut = new WhereOrFilterInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Act
        sut.AppendFormatted(value);

        //Assert
        fluentSqlFormatterMock.Verify(x => x.FormatParameter(value, null));
    }

    [Theory]
    [InlineAutoData(0, null)]
    [InlineAutoData("value", "raw")]
    public void AppendFormatted_FormattedValueWithFormatAppended_ReturnsVoid(object value, string? format, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();
        var sut = new WhereOrFilterInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Act
        sut.AppendFormatted(value, format);

        //Assert
        fluentSqlFormatterMock.Verify(x => x.FormatParameter(value, format));
    }

    [Theory]
    [AutoData]
    public void Close_ClauseActionEnded_ReturnsVoid(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();
        var sut = new WhereOrFilterInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Act
        sut.Close();

        //Assert
        fluentSqlFormatterMock.Verify(x => x.EndClauseAction(ClauseAction.WhereOrFilter));
    }
}
#endif
