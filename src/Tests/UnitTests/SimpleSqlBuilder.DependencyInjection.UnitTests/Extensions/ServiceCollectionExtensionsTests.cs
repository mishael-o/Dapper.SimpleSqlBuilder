using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddSimpleSqlBuilder_ServiceCollectionIsNull_ThrowsArgumentException()
    {
        //Arrange
        IServiceCollection service = null!;

        //Act
        var act = () => service.AddSimpleSqlBuilder();

        //Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName(nameof(service));
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_DefaultConfiguration_ReturnsIServiceCollection(ServiceCollection service)
    {
        //Act
        service.AddSimpleSqlBuilder();
        var provider = service.BuildServiceProvider();
        var serviceDescriptor = service.First(x => x.ServiceType == typeof(ISimpleBuilder));
        var configuredOptions = provider.GetService<IOptions<SimpleBuilderOptions>>();

        //Assert
        serviceDescriptor.ImplementationType.Should().Be(typeof(InternalSimpleBuilder));
        serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
        configuredOptions!.Value.DatabaseParameterNameTemplate.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate);
        configuredOptions.Value.DatabaseParameterPrefix.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterPrefix);
        configuredOptions.Value.ReuseParameters.Should().Be(SimpleBuilderSettings.DefaultReuseParameters);
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_CustomConfiguration_ReturnsIServiceCollection(ServiceLifetime serviceLifetime, [NoAutoProperties] SimpleBuilderOptions options, ServiceCollection service)
    {
        //Act
        service.AddSimpleSqlBuilder(serviceLifetime, configure =>
        {
            configure.DatabaseParameterNameTemplate = options.DatabaseParameterNameTemplate;
            configure.DatabaseParameterPrefix = options.DatabaseParameterPrefix;
            configure.ReuseParameters = options.ReuseParameters;
        });

        var provider = service.BuildServiceProvider();
        var configuredOptions = provider.GetRequiredService<IOptions<SimpleBuilderOptions>>();
        var serviceDescriptor = service.First(x => x.ServiceType == typeof(ISimpleBuilder));

        //Assert
        serviceDescriptor.ImplementationType.Should().Be(typeof(InternalSimpleBuilder));
        serviceDescriptor.Lifetime.Should().Be(serviceLifetime);
        configuredOptions.Value.DatabaseParameterNameTemplate.Should().Be(options.DatabaseParameterNameTemplate);
        configuredOptions.Value.DatabaseParameterPrefix.Should().Be(options.DatabaseParameterPrefix);
        configuredOptions.Value.ReuseParameters.Should().Be(options.ReuseParameters);
        SimpleBuilderSettings.DatabaseParameterNameTemplate.Should().Be(options.DatabaseParameterNameTemplate);
        SimpleBuilderSettings.DatabaseParameterPrefix.Should().Be(options.DatabaseParameterPrefix);
        SimpleBuilderSettings.ReuseParameters.Should().Be(options.ReuseParameters);
    }
}