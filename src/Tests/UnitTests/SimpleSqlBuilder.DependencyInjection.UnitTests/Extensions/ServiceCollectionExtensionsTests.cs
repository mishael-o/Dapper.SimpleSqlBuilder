using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Extensions;

[Collection($"~ Run Last - {nameof(ServiceCollectionExtensionsTests)}")]
public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddSimpleSqlBuilder_ServiceCollectionIsNull_ThrowsArgumentException()
    {
        //Arrange
        IServiceCollection sut = null!;

        //Act
        var act = () => sut.AddSimpleSqlBuilder();

        //Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("service");
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_DefaultConfiguration_ReturnsIServiceCollection(ServiceCollection sut)
    {
        //Act
        sut.AddSimpleSqlBuilder();
        var provider = sut.BuildServiceProvider();
        var serviceDescriptor = sut.First(x => x.ServiceType == typeof(ISimpleBuilder));
        var configuredOptions = provider.GetService<IOptions<SimpleBuilderOptions>>();

        //Assert
        serviceDescriptor.ImplementationType.Should().Be(typeof(SimpleBuilderFactory));
        serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
        configuredOptions!.Value.DatabaseParameterNameTemplate.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate);
        configuredOptions.Value.DatabaseParameterPrefix.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterPrefix);
        configuredOptions.Value.ReuseParameters.Should().Be(SimpleBuilderSettings.DefaultReuseParameters);
        configuredOptions.Value.UseLowerCaseClauses.Should().Be(SimpleBuilderSettings.DefaultUseLowerCaseClauses);
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_CustomConfiguration_ReturnsIServiceCollection(ServiceLifetime serviceLifetime, ServiceCollection sut)
    {
        //Arrange
        var options = new SimpleBuilderOptions { DatabaseParameterNameTemplate = "myParam", DatabaseParameterPrefix = ":", ReuseParameters = true, UseLowerCaseClauses = true };

        //Act
        sut.AddSimpleSqlBuilder(serviceLifetime, configure =>
        {
            configure.DatabaseParameterNameTemplate = options.DatabaseParameterNameTemplate;
            configure.DatabaseParameterPrefix = options.DatabaseParameterPrefix;
            configure.ReuseParameters = options.ReuseParameters;
            configure.UseLowerCaseClauses = options.UseLowerCaseClauses;
        });

        var provider = sut.BuildServiceProvider();
        var serviceDescriptor = sut.First(x => x.ServiceType == typeof(ISimpleBuilder));
        var configuredOptions = provider.GetRequiredService<IOptions<SimpleBuilderOptions>>();

        //Assert
        serviceDescriptor.ImplementationType.Should().Be(typeof(SimpleBuilderFactory));
        serviceDescriptor.Lifetime.Should().Be(serviceLifetime);
        configuredOptions.Value.DatabaseParameterNameTemplate.Should().Be(options.DatabaseParameterNameTemplate);
        configuredOptions.Value.DatabaseParameterPrefix.Should().Be(options.DatabaseParameterPrefix);
        configuredOptions.Value.ReuseParameters.Should().Be(options.ReuseParameters);
        configuredOptions.Value.UseLowerCaseClauses.Should().Be(options.UseLowerCaseClauses);
        SimpleBuilderSettings.Instance.DatabaseParameterNameTemplate.Should().Be(options.DatabaseParameterNameTemplate);
        SimpleBuilderSettings.Instance.DatabaseParameterPrefix.Should().Be(options.DatabaseParameterPrefix);
        SimpleBuilderSettings.Instance.ReuseParameters.Should().Be(options.ReuseParameters);
        SimpleBuilderSettings.Instance.UseLowerCaseClauses.Should().Be(options.UseLowerCaseClauses);
    }
}