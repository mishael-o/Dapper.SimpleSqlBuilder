using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapper.SimpleSqlBuilder.DependencyInjection;

/// <summary>
/// An extension class for <see cref="IServiceCollection"/> to configure the Simple SQL builder.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds services for the Simple SQL builder.
    /// </summary>
    /// <param name="service">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> for the <see cref="ISimpleBuilder"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> when <paramref name="service"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddSimpleSqlBuilder(this IServiceCollection service, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        return service
            .AddSimpleSqlBuilder(SimpleBuilderOptions.ConfigurationSectionName, serviceLifetime);
    }

    /// <summary>
    /// Adds services for the Simple SQL builder.
    /// </summary>
    /// <param name="service">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configure">The action to configure the <see cref="SimpleBuilderOptions"/>.</param>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> for the <see cref="ISimpleBuilder"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> when <paramref name="service"/> or <paramref name="configure"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddSimpleSqlBuilder(this IServiceCollection service, Action<SimpleBuilderOptions> configure, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        if (service is null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        if (configure is null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        service
            .AddSimpleSqlBuilderDependencies(serviceLifetime)
            .AddOptions<SimpleBuilderOptions>()
            .Configure(configure);

        return service;
    }

    /// <summary>
    /// Adds services for the Simple SQL builder.
    /// </summary>
    /// <param name="service">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configurationSectionPath">The name of the configuration section to bind to the <see cref="SimpleBuilderOptions"/>.</param>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> for the <see cref="ISimpleBuilder"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    /// <exception cref="ArgumentNullException">
    /// Throws an <see cref="ArgumentNullException"/> when <paramref name="service"/> is <see langword="null"/> or when <paramref name="configurationSectionPath"/> is <see langword="null"/>, empty or white-space.
    /// </exception>
    public static IServiceCollection AddSimpleSqlBuilder(this IServiceCollection service, string configurationSectionPath, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        if (service is null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        if (string.IsNullOrWhiteSpace(configurationSectionPath))
        {
            throw new ArgumentNullException(nameof(configurationSectionPath));
        }

        service
            .AddSimpleSqlBuilderDependencies(serviceLifetime)
            .AddOptions<SimpleBuilderOptions>()
            .BindConfiguration(configurationSectionPath);

        return service;
    }

    private static IServiceCollection AddSimpleSqlBuilderDependencies(this IServiceCollection services, ServiceLifetime serviceLifetime)
    {
        var serviceDescriptor = ServiceDescriptor.Describe(typeof(ISimpleBuilder), typeof(SimpleBuilderFactory), serviceLifetime);
        services.TryAdd(serviceDescriptor);

        return services;
    }
}
