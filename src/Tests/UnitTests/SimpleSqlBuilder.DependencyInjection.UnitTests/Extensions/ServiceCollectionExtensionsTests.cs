using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dapper.SimpleSqlBuilder.DependencyInjection.UnitTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddSimpleSqlBuilder_ServiceCollectionIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IServiceCollection sut = null!;

        // Act
        var act = () => sut.AddSimpleSqlBuilder();

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("service");
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_AddsServices_ReturnsIServiceCollection(ServiceCollection sut, ConfigurationBuilder configurationBuilder)
    {
        // Arrange
        sut.AddSingleton<IConfiguration>(configurationBuilder.Build());

        // Act
        sut.AddSimpleSqlBuilder();

        // Assert
        var provider = sut.BuildServiceProvider();
        var serviceDescriptor = sut.First(x => x.ServiceType == typeof(ISimpleBuilder));
        var configuredOptions = provider.GetRequiredService<IOptionsMonitor<SimpleBuilderOptions>>();

        serviceDescriptor.ImplementationType.Should().Be<SimpleBuilderFactory>();
        serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
        configuredOptions.CurrentValue.DatabaseParameterNameTemplate.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate);
        configuredOptions.CurrentValue.DatabaseParameterPrefix.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterPrefix);
        configuredOptions.CurrentValue.ReuseParameters.Should().Be(SimpleBuilderSettings.DefaultReuseParameters);
        configuredOptions.CurrentValue.UseLowerCaseClauses.Should().Be(SimpleBuilderSettings.DefaultUseLowerCaseClauses);
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_AddsServicesWhenConfigurationFromConfigurationSettings_ReturnsIServiceCollection(
        ServiceCollection sut,
        ConfigurationBuilder configurationBuilder,
        ServiceLifetime serviceLifetime)
    {
        // Arrange
        var option = new { parameterNameTemplate = "pm", parameterPrefix = ":", reuseParameters = false, userLowerCaseClauses = true };
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { $"{SimpleBuilderOptions.ConfigurationSectionName}:DatabaseParameterNameTemplate", option.parameterNameTemplate },
            { $"{SimpleBuilderOptions.ConfigurationSectionName}:DatabaseParameterPrefix", option.parameterPrefix },
            { $"{SimpleBuilderOptions.ConfigurationSectionName}:ReuseParameters", option.reuseParameters.ToString() },
            { $"{SimpleBuilderOptions.ConfigurationSectionName}:UseLowerCaseClauses", option.userLowerCaseClauses.ToString() }
        });

        sut.AddSingleton<IConfiguration>(configurationBuilder.Build());

        // Act
        sut.AddSimpleSqlBuilder(serviceLifetime);

        // Assert
        var provider = sut.BuildServiceProvider();
        var serviceDescriptor = sut.First(x => x.ServiceType == typeof(ISimpleBuilder));
        var configuredOptions = provider.GetRequiredService<IOptionsMonitor<SimpleBuilderOptions>>();

        serviceDescriptor.ImplementationType.Should().Be<SimpleBuilderFactory>();
        serviceDescriptor.Lifetime.Should().Be(serviceLifetime);
        configuredOptions.CurrentValue.DatabaseParameterNameTemplate.Should().Be(option.parameterNameTemplate);
        configuredOptions.CurrentValue.DatabaseParameterPrefix.Should().Be(option.parameterPrefix);
        configuredOptions.CurrentValue.ReuseParameters.Should().Be(option.reuseParameters);
        configuredOptions.CurrentValue.UseLowerCaseClauses.Should().Be(option.userLowerCaseClauses);
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_ServiceCollectionIsNullWhenCofigurationFromConfigureAction_ThrowsArgumentNullException(Action<SimpleBuilderOptions> configure)
    {
        // Arrange
        IServiceCollection sut = null!;

        // Act
        var act = () => sut.AddSimpleSqlBuilder(configure);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("service");
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_ConfigureActionIsNull_ThrowsArgumentNullException(ServiceCollection sut)
    {
        // Arrange
        Action<SimpleBuilderOptions> configure = null!;

        // Act
        var act = () => sut.AddSimpleSqlBuilder(configure);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("configure");
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_AddsServicesWhenConfigurationFromConfigureAction_ReturnsIServiceCollection(ServiceLifetime serviceLifetime, ServiceCollection sut)
    {
        // Arrange
        var option = new { DatabaseParameterNameTemplate = "myParam", DatabaseParameterPrefix = ":", ReuseParameters = true, UseLowerCaseClauses = true };

        // Act
        sut.AddSimpleSqlBuilder(
            configure =>
            {
                configure.DatabaseParameterNameTemplate = option.DatabaseParameterNameTemplate;
                configure.DatabaseParameterPrefix = option.DatabaseParameterPrefix;
                configure.ReuseParameters = option.ReuseParameters;
                configure.UseLowerCaseClauses = option.UseLowerCaseClauses;
            },
            serviceLifetime);

        // Assert
        var provider = sut.BuildServiceProvider();
        var serviceDescriptor = sut.First(x => x.ServiceType == typeof(ISimpleBuilder));
        var configuredOptions = provider.GetRequiredService<IOptionsMonitor<SimpleBuilderOptions>>();

        serviceDescriptor.ImplementationType.Should().Be<SimpleBuilderFactory>();
        serviceDescriptor.Lifetime.Should().Be(serviceLifetime);
        configuredOptions.CurrentValue.DatabaseParameterNameTemplate.Should().Be(option.DatabaseParameterNameTemplate);
        configuredOptions.CurrentValue.DatabaseParameterPrefix.Should().Be(option.DatabaseParameterPrefix);
        configuredOptions.CurrentValue.ReuseParameters.Should().Be(option.ReuseParameters);
        configuredOptions.CurrentValue.UseLowerCaseClauses.Should().Be(option.UseLowerCaseClauses);
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_ServiceCollectionIsNullWhenCofigurationFromConfigurationSectionPath_ThrowsArgumentNullException(string configSectionPath)
    {
        // Arrange
        IServiceCollection sut = null!;

        // Act
        var act = () => sut.AddSimpleSqlBuilder(configSectionPath);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("service");
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(null)]
    [InlineAutoData("   ")]
    public void AddSimpleSqlBuilder_ConfigurationSectionPathIsNullOrWhiteSpace_ThrowsArgumentNullException(string configSectionPath, ServiceCollection sut)
    {
        // Act
        var act = () => sut.AddSimpleSqlBuilder(configSectionPath);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("configurationSectionPath");
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_ConfigurationSectionPathDoesNotExist_ReturnsIServiceCollection(ServiceCollection sut, ConfigurationBuilder configurationBuilder, string configSectionPath)
    {
        // Arrange
        sut.AddSingleton<IConfiguration>(configurationBuilder.Build());

        // Act
        sut.AddSimpleSqlBuilder(configSectionPath);

        // Assert
        var provider = sut.BuildServiceProvider();
        var serviceDescriptor = sut.First(x => x.ServiceType == typeof(ISimpleBuilder));
        var configuredOptions = provider.GetRequiredService<IOptionsMonitor<SimpleBuilderOptions>>();

        serviceDescriptor.ImplementationType.Should().Be<SimpleBuilderFactory>();
        serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
        configuredOptions.CurrentValue.DatabaseParameterNameTemplate.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterNameTemplate);
        configuredOptions.CurrentValue.DatabaseParameterPrefix.Should().Be(SimpleBuilderSettings.DefaultDatabaseParameterPrefix);
        configuredOptions.CurrentValue.ReuseParameters.Should().Be(SimpleBuilderSettings.DefaultReuseParameters);
        configuredOptions.CurrentValue.UseLowerCaseClauses.Should().Be(SimpleBuilderSettings.DefaultUseLowerCaseClauses);
    }

    [Theory]
    [AutoData]
    public void AddSimpleSqlBuilder_AddsServicesWhenConfigurationFromConfigurationSectionPath_ReturnsIServiceCollection(
        ServiceCollection sut,
        ConfigurationBuilder configurationBuilder,
        string configSectionPath,
        ServiceLifetime serviceLifetime)
    {
        // Arrange
        var option = new { parameterNameTemplate = "parm", parameterPrefix = "@", reuseParameters = true, userLowerCaseClauses = false };
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { $"{configSectionPath}:DatabaseParameterNameTemplate", option.parameterNameTemplate },
            { $"{configSectionPath}:DatabaseParameterPrefix", option.parameterPrefix },
            { $"{configSectionPath}:ReuseParameters", option.reuseParameters.ToString() },
            { $"{configSectionPath}:UseLowerCaseClauses", option.userLowerCaseClauses.ToString() }
        });

        sut.AddSingleton<IConfiguration>(configurationBuilder.Build());

        // Act
        sut.AddSimpleSqlBuilder(configSectionPath, serviceLifetime);

        // Assert
        var provider = sut.BuildServiceProvider();
        var serviceDescriptor = sut.First(x => x.ServiceType == typeof(ISimpleBuilder));
        var configuredOptions = provider.GetRequiredService<IOptionsMonitor<SimpleBuilderOptions>>();

        serviceDescriptor.ImplementationType.Should().Be<SimpleBuilderFactory>();
        serviceDescriptor.Lifetime.Should().Be(serviceLifetime);
        configuredOptions.CurrentValue.DatabaseParameterNameTemplate.Should().Be(option.parameterNameTemplate);
        configuredOptions.CurrentValue.DatabaseParameterPrefix.Should().Be(option.parameterPrefix);
        configuredOptions.CurrentValue.ReuseParameters.Should().Be(option.reuseParameters);
        configuredOptions.CurrentValue.UseLowerCaseClauses.Should().Be(option.userLowerCaseClauses);
    }
}
