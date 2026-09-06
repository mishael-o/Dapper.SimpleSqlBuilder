#if NET8_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.UnitTests.Core.Handlers;

public class BuilderInterpolatedStringHandlerTests
{
    [Theory]
    [AutoData]
    public void AppendLiteral_AppendsLiteral_ReturnsVoid(string value)
    {
        // Arrange
        var sut = new BuilderInterpolatedStringHandler(0, 0);

        // Act
        sut.AppendLiteral(value);

        // Assert
        var builder = sut.GetBuilder();
        builder.ShouldNotBeNull();
        builder.Sql.ShouldBe(value);
    }

    [Theory]
    [InlineAutoData("value", null, "@p0")]
    [InlineAutoData("value", "raw", "value")]
    public void AppendFormatted_AppendsFormattedValue_ReturnsVoid(string value, string format, string expectedSql)
    {
        // Arrange
        var sut = new BuilderInterpolatedStringHandler(0, 0);

        // Act
        sut.AppendFormatted(value, format);

        // Assert
        var builder = sut.GetBuilder();
        builder.ShouldNotBeNull();
        builder.Sql.ShouldBe(expectedSql);
    }

    [Fact]
    public void GetBuilder_FormatterIsNull_ThrowsInvalidOperationException()
    {
        // Act
        var act = () =>
        {
            var sut = default(BuilderInterpolatedStringHandler);
            _ = sut.GetBuilder();
        };

        // Assert
        act.ShouldThrow<InvalidOperationException>()
           .Message.ShouldBe("The formatter is null. Ensure BuilderInterpolatedStringHandler is properly initialized.");
    }
}
#endif
