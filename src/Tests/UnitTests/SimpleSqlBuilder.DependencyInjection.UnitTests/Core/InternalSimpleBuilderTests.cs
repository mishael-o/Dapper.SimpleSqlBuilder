using Microsoft.Extensions.Options;
using SimpleSqlBuilder.UnitTestHelpers;

namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Core;

public class InternalSimpleBuilderTests
{
    [Theory]
    [AutoMoqData(configureMembers: true)]
    internal void Create_NoArgumentsPassed_ReturnsSimpleBuilderBase(InternalSimpleBuilder sut)
    {
        //Act
        var builder = sut.Create();

        //Assert
        builder.Should().BeOfType<SqlBuilder>().And.BeAssignableTo<SimpleBuilderBase>();
        builder.ParameterNames.Should().HaveCount(0);
    }

    [Theory]
    [AutoMoqData]
    internal void Create_FormattableStringArgumentOnly_ReturnsSimpleBuilderBase(
        [Frozen] Mock<IOptions<SimpleBuilderOptions>> optionsMock,
        InternalSimpleBuilder sut,
        [NoAutoProperties] SimpleBuilderOptions options,
        int id)
    {
        //Arrange
        optionsMock.SetupGet(x => x.Value).Returns(options);
        var baseParamName = SimpleBuilderSettings.DatabaseParameterPrefix + SimpleBuilderSettings.DatabaseParameterNameTemplate;
        string expectedSql = $"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {baseParamName}0) FROM TABLE WHERE Id = {baseParamName}1";

        //Act
        var builder = sut.Create($"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {id}) FROM TABLE WHERE Id = {id}");

        //Assert
        builder.Should().BeOfType<SqlBuilder>().And.BeAssignableTo<SimpleBuilderBase>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(2);
    }

    [Theory]
    [AutoMoqData]
    internal void Create_AllArgumentsPassed_ReturnsSimpleBuilderBase(
        [Frozen] Mock<IOptions<SimpleBuilderOptions>> optionsMock,
        InternalSimpleBuilder sut,
        [NoAutoProperties] SimpleBuilderOptions options,
        int id)
    {
        //Arrange
        const string parameterPrefix = ":";
        optionsMock.SetupGet(x => x.Value).Returns(options);
        var parameterName = $"{parameterPrefix}{SimpleBuilderSettings.DatabaseParameterNameTemplate}0";
        string expectedSql = $"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {parameterName}) FROM TABLE WHERE Id = {parameterName}";

        //Act
        var builder = sut.Create($"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {id}) FROM TABLE WHERE Id = {id}", parameterPrefix, true);

        //Assert
        builder.Should().BeOfType<SqlBuilder>().And.BeAssignableTo<SimpleBuilderBase>();
        builder.Sql.Should().Be(expectedSql);
        builder.ParameterNames.Should().HaveCount(1);
    }
}