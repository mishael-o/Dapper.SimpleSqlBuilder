#if NET6_0_OR_GREATER
using Dapper.SimpleSqlBuilder.FluentBuilder;

namespace Dapper.SimpleSqlBuilder.UnitTests.FluentBuilder.Handlers;

public class InsertColumnInterpolatedStringHandlerTests
{
    [Fact]
    public void Constructor_FluentBuilderIsNull_ThrowsArgumentException()
    {
        //Arrange
        IFluentBuilder fluentBuilder = null!;

        //Act
        Action act = () => _ = new InsertColumnInterpolatedStringHandler(0, 0, fluentBuilder);

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
        Action act = () => _ = new InsertColumnInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

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
        var sut = new InsertColumnInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Assert
        fluentFormatterMock.Verify(x => x.StartClauseAction(ClauseAction.InsertColumn));
    }

    [Theory]
    [AutoData]
    public void AppendLiteral_AppendsLiteral_ReturnsVoid(string value, Mock<IFluentBuilder> fluentBuilderMock)
    {
        //Arrange
        var fluentFormatterMock = fluentBuilderMock.As<IFluentBuilderFormatter>();
        var sut = new InsertColumnInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

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
        var sut = new InsertColumnInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

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
        var sut = new InsertColumnInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

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
        var sut = new InsertColumnInterpolatedStringHandler(0, 0, fluentBuilderMock.Object);

        //Act
        sut.Close();

        //Assert
        fluentFormatterMock.Verify(x => x.EndClauseAction(ClauseAction.InsertColumn));
    }
}
#endif
