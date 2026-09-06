using Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;

namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Core;

public class BuilderFactoryInterpolatedStringHandlerTests
{
    [Fact]
    public void Constructor_BuilderIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        ISimpleBuilder builderFactory = null!;

        // Act
        Action act = () => _ = new BuilderFactoryInterpolatedStringHandler(0, 0, builderFactory);

        // Assert
        act.ShouldThrow<ArgumentNullException>()
           .ParamName.ShouldBe("builderFactory");
    }

    [Theory]
    [AutoNSubstituteData]
    public void Constructor_BuilderDoesNotImplementIBuilderFormatter_ThrowsInvalidOperationException(ISimpleBuilder builderFactory)
    {
        // Arrange
        builderFactory.Create(Arg.Any<FormattableString?>(), Arg.Any<string?>(), Arg.Any<bool?>())
                      .Returns(default(Builder)!);

        // Act
        Action act = () => _ = new BuilderFactoryInterpolatedStringHandler(0, 0, builderFactory);

        // Assert
        act.ShouldThrow<InvalidOperationException>()
           .Message.ShouldBe("ISimpleBuilder.Create does not return a builder that implements IBuilderFormatter.");
    }

    [Theory]
    [AutoNSubstituteData]
    public void AppendLiteral_AppendsLiteral_ReturnsVoid(string value, ISimpleBuilder builderFactory)
    {
        // Arrange
        var builder = Substitute.For<Builder, IBuilderFormatter>();
        builderFactory.Create(Arg.Any<FormattableString?>(), Arg.Any<string?>(), Arg.Any<bool?>())
                      .Returns(builder);

        var sut = new BuilderFactoryInterpolatedStringHandler(0, 0, builderFactory);

        // Act
        sut.AppendLiteral(value);

        // Assert
        ((IBuilderFormatter)builder).Received().AppendLiteral(value);
    }

    [Theory]
    [InlineData("value", null)]
    [InlineData("value", "raw")]
    public void AppendFormatted_AppendsFormatted_ReturnsVoid(string value, string? format)
    {
        // Arrange
        var builderFactory = Substitute.For<ISimpleBuilder>();
        var builder = Substitute.For<Builder, IBuilderFormatter>();
        builderFactory.Create(Arg.Any<FormattableString?>(), Arg.Any<string?>(), Arg.Any<bool?>())
                      .Returns(builder);

        var sut = new BuilderFactoryInterpolatedStringHandler(0, 0, builderFactory);

        // Act
        sut.AppendFormatted(value, format);

        // Assert
        ((IBuilderFormatter)builder).Received().AppendFormatted(value, format);
    }

    [Fact]
    public void GetBuilder_FormatterIsNull_ThrowsInvalidOperationException()
    {
        // Act
        var act = () =>
        {
            var sut = default(BuilderFactoryInterpolatedStringHandler);
            _ = sut.GetBuilder();
        };

        // Assert
        act.ShouldThrow<InvalidOperationException>()
           .Message.ShouldBe("The formatter is null. Ensure BuilderFactoryInterpolatedStringHandler is properly initialized.");
    }
}
