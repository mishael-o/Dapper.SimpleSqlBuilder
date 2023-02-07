﻿using Dapper.SimpleSqlBuilder.FluentBuilder;
using Dapper.SimpleSqlBuilder.UnitTestHelpers.AutoFixture;
using Microsoft.Extensions.Options;

namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Core;

public class SimpleBuilderFactoryTests
{
    [Theory]
    [AutoMoqData(true)]
    internal void Create_NoArgumentsPassed_ReturnsSimpleBuilderBase(SimpleBuilderFactory sut)
    {
        //Act
        var result = sut.Create();

        //Assert
        result.Should().BeOfType<SqlBuilder>();
        result.ParameterNames.Should().HaveCount(0);
    }

    [Theory]
    [AutoMoqData]
    internal void Create_FormattableStringArgumentOnly_ReturnsSimpleBuilderBase(
        [Frozen] Mock<IOptions<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut,
        [NoAutoProperties] SimpleBuilderOptions options,
        int id)
    {
        //Arrange
        optionsMock.SetupGet(x => x.Value).Returns(options);
        var baseParamName = options.DatabaseParameterPrefix + options.DatabaseParameterNameTemplate;
        string expectedSql = $"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {baseParamName}0) FROM TABLE WHERE Id = {baseParamName}1";

        //Act
        var result = sut.Create($"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {id}) FROM TABLE WHERE Id = {id}");

        //Assert
        result.Should().BeOfType<SqlBuilder>();
        result.Sql.Should().Be(expectedSql);
        result.ParameterNames.Should().HaveCount(2);
    }

    [Theory]
    [AutoMoqData]
    internal void Create_AllArgumentsPassed_ReturnsSimpleBuilderBase(
        [Frozen] Mock<IOptions<SimpleBuilderOptions>> optionsMock,
        SimpleBuilderFactory sut,
        [NoAutoProperties] SimpleBuilderOptions options,
        int id)
    {
        //Arrange
        const string parameterPrefix = ":";
        optionsMock.SetupGet(x => x.Value).Returns(options);
        var parameterName = $"{parameterPrefix}{options.DatabaseParameterNameTemplate}0";
        string expectedSql = $"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {parameterName}) FROM TABLE WHERE Id = {parameterName}";

        //Act
        var result = sut.Create($"SELECT x.*, (SELECT DESC FROM DESC_TABLE WHERE Id = {id}) FROM TABLE WHERE Id = {id}", parameterPrefix, true);

        //Assert
        result.Should().BeOfType<SqlBuilder>();
        result.Sql.Should().Be(expectedSql);
        result.ParameterNames.Should().HaveCount(1);
    }

    [Theory]
    [AutoMoqData(true)]
    [InlineAutoMoqData(configureMembers: true, generateDelegates: false, null, null, null)]
    internal void CreateFluent_InitialiseFluentBuilder_ReturnsISimpleFluentBuilder(string? parameterPrefix, bool? reuseParameters, bool? useLowerCaseClauses, SimpleBuilderFactory sut)
    {
        //Act
        var result = sut.CreateFluent(parameterPrefix, reuseParameters, useLowerCaseClauses);

        //Assert
        result.Should().BeOfType<FluentSqlBuilder>();
    }
}