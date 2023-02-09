#if NET6_0_OR_GREATER
namespace Dapper.SimpleSqlBuilder.UnitTests.Handlers;

public class AppendIntactSqlIntepolatedStringHandlerTests
{
    [Fact]
    public void Constructor_BuilderIsNull_ThrowsArgumentException()
    {
        //Arrange
        ISqlBuilder builder = null!;

        //Act
        Action act = () => _ = new AppendIntactSqlIntepolatedStringHandler(0, 0, builder);

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The builder must implement IBuilderFormatter.*")
            .WithParameterName("builder");
    }

    [Theory]
    [AutoData]
    public void Constructor_BuilderDoesNotImplementIBuilderFormatter_ThrowsArgumentException(Mock<ISqlBuilder> builderMock)
    {
        //Act
        Action act = () => _ = new AppendIntactSqlIntepolatedStringHandler(0, 0, builderMock.Object);

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The builder must implement IBuilderFormatter.*")
            .WithParameterName("builder");
    }

    [Theory]
    [AutoData]
    public void AppendLiteral_AppendsLiteral_ReturnsVoid(string value, Mock<ISqlBuilder> builderMock)
    {
        //Arrange
        var builderFormatterMock = builderMock.As<IBuilderFormatter>();
        var sut = new AppendIntactSqlIntepolatedStringHandler(0, 0, builderMock.Object);

        //Act
        sut.AppendLiteral(value);

        //Assert
        builderFormatterMock.Verify(x => x.AppendLiteral(value));
    }

    [Theory]
    [AutoData]
    public void AppendFormatted_AppendsFormatted_ReturnsVoid(string value, Mock<ISqlBuilder> builderMock)
    {
        //Arrange
        var builderFormatterMock = builderMock.As<IBuilderFormatter>();
        var sut = new AppendIntactSqlIntepolatedStringHandler(0, 0, builderMock.Object);

        //Act
        sut.AppendFormatted(value);

        //Assert
        builderFormatterMock.Verify(x => x.AppendFormatted(value, null));
    }

    [Theory]
    [InlineAutoData(0, null)]
    [InlineAutoData("value", "raw")]
    public void AppendFormatted_AppendsFormattedWithFormat_ReturnsVoid(object value, string? format, Mock<ISqlBuilder> builderMock)
    {
        //Arrange
        var builderFormatterMock = builderMock.As<IBuilderFormatter>();
        var sut = new AppendIntactSqlIntepolatedStringHandler(0, 0, builderMock.Object);

        //Act
        sut.AppendFormatted(value, format);

        //Assert
        builderFormatterMock.Verify(x => x.AppendFormatted(value, format));
    }
}
#endif
