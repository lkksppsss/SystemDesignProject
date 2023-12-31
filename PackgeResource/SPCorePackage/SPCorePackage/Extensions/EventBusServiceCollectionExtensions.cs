﻿using Autofac;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SPCorePackage.Kafka;
using SPCorePackage.Kafka.Interface;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SPCorePackage.Extensions;

public static class EventBusServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaBus([NotNull] this IServiceCollection services, string connectString, params string[] topics)
    {
        //註冊EventHandler
        foreach (var typeInfo in Assembly.GetCallingAssembly().GetTypes())
        {
            if (typeInfo.GetTypeInfo().ImplementedInterfaces.Any(
                x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)))
            {
                services.AddTransient(typeInfo);
            }
        }

        services.AddSingleton<IEventBus, KafkaService>(sp =>
        {
            var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
            var logger = sp.GetRequiredService<ILogger<KafkaService>>();
            return new KafkaService(connectString, logger, iLifetimeScope, topics);
        });

        return services;
    }
}
