﻿using Microsoft.Extensions.DependencyInjection;

namespace Dapper.SimpleSqlBuilder.DependencyInjection;

/// <summary>
/// An extension class for <see cref="IServiceCollection"/> to configure the Simple SQL builder.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds services for the Simple SQL builder.
    /// </summary>
    /// <param name="service">The <see cref="IServiceCollection"/> instance.</param>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> for the <see cref="ISimpleBuilder"/>.</param>
    /// <param name="configure">The action to configure the <see cref="SimpleBuilderOptions"/> for the Simple builder settings.</param>
    /// <returns><see cref="IServiceCollection"/>.</returns>
    /// <exception cref="ArgumentNullException">Throws a <see cref="ArgumentNullException"/> when <paramref name="service"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddSimpleSqlBuilder(this IServiceCollection service, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton, Action<SimpleBuilderOptions>? configure = null)
    {
        if (service is null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        var serviceDescriptor = ServiceDescriptor.Describe(typeof(ISimpleBuilder), typeof(SimpleBuilderFactory), serviceLifetime);
        service.Add(serviceDescriptor);
        var optionsBuilder = service.AddOptions<SimpleBuilderOptions>();

        if (configure is null)
        {
            return service;
        }

        optionsBuilder.Configure(configure);
        ConfigureStaticSimpleBuilderSettings(configure);

        return service;
    }

    private static void ConfigureStaticSimpleBuilderSettings(Action<SimpleBuilderOptions> configure)
    {
        var options = new SimpleBuilderOptions();
        configure(options);

        SimpleBuilderSettings.Configure(
            options.DatabaseParameterNameTemplate,
            options.DatabaseParameterPrefix,
            options.ReuseParameters,
            options.UseLowerCaseClauses);
    }
}
