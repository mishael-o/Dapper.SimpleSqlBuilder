#if NET6_0_OR_GREATER

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
        builder.Should().NotBeNull();
        builder.Sql.Should().Be(value);
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
        builder.Should().NotBeNull();
        builder.Sql.Should().Be(expectedSql);
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
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The formatter is null. Ensure BuilderInterpolatedStringHandler is properly initialized.");
    }
}
#endif
