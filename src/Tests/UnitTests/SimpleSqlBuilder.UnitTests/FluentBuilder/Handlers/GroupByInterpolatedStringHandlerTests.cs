﻿#if NET6_0_OR_GREATER
namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder.Handlers;

public class GroupByInterpolatedStringHandlerTests
{
    [Theory]
    [AutoData]
    public void Constructor_InitialiseHandler_HandlerInitialised(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();

        fluentSqlFormatterMock
            .Setup(x => x.IsClauseActionEnabled(ClauseAction.GroupBy))
            .Returns(true);

        //Act
        var sut = new GroupByInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var isHandlerEnabled);

        //Assert
        isHandlerEnabled.Should().BeTrue();
        fluentSqlFormatterMock.Verify(x => x.StartClauseAction(ClauseAction.GroupBy));
    }

    [Theory]
    [AutoData]
    public void Constructor_InitialiseHandler_HandlerDisabled(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();

        fluentSqlFormatterMock
            .Setup(x => x.IsClauseActionEnabled(ClauseAction.GroupBy))
            .Returns(false);

        //Act
        var sut = new GroupByInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var isHandlerEnabled);

        //Assert
        isHandlerEnabled.Should().BeFalse();
    }

    [Theory]
    [AutoData]
    public void Constructor_InitialiseHandlerWithCondition_HandlerInitialised(Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        const bool condition = true;
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();

        fluentSqlFormatterMock
            .Setup(x => x.IsClauseActionEnabled(ClauseAction.GroupBy))
            .Returns(true);

        //Act
        var sut = new GroupByInterpolatedStringHandler(0, 0, condition, fluentBuilderMock.Object, out var isHandlerEnabled);

        //Assert
        isHandlerEnabled.Should().BeTrue();
        fluentSqlFormatterMock.Verify(x => x.StartClauseAction(ClauseAction.GroupBy));
    }

    [Theory]
    [InlineAutoData(true, false)]
    [InlineAutoData(false, true)]
    [InlineAutoData(false, false)]

    public void Constructor_InitialiseHandlerWithCondition_HandlerDisabled(bool condition, bool isClauseActionEnabled, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();

        fluentSqlFormatterMock
            .Setup(x => x.IsClauseActionEnabled(ClauseAction.GroupBy))
            .Returns(isClauseActionEnabled);

        //Act
        var sut = new GroupByInterpolatedStringHandler(0, 0, condition, fluentBuilderMock.Object, out var isHandlerEnabled);

        //Assert
        isHandlerEnabled.Should().BeFalse();
    }

    [Theory]
    [AutoData]
    public void AppendLiteral_LiteralValueAppended_ReturnsVoid(string value, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentSqlFormatterMock = fluentBuilderMock.As<IFluentSqlFormatter>();

        fluentSqlFormatterMock
            .Setup(x => x.IsClauseActionEnabled(ClauseAction.GroupBy))
            .Returns(true);

        var sut = new GroupByInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var _);

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

        fluentSqlFormatterMock
            .Setup(x => x.IsClauseActionEnabled(ClauseAction.GroupBy))
            .Returns(true);

        var sut = new GroupByInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var _);

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

        fluentSqlFormatterMock
            .Setup(x => x.IsClauseActionEnabled(ClauseAction.GroupBy))
            .Returns(true);

        var sut = new GroupByInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var _);

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

        fluentSqlFormatterMock
            .Setup(x => x.IsClauseActionEnabled(ClauseAction.GroupBy))
            .Returns(true);

        var sut = new GroupByInterpolatedStringHandler(0, 0, fluentBuilderMock.Object, out var _);

        //Act
        sut.Close();

        //Assert
        fluentSqlFormatterMock.Verify(x => x.EndClauseAction(ClauseAction.GroupBy));
    }
}
#endif