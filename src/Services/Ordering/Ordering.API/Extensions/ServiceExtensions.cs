using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.Application.IntegrationEvents.EventsHandler;
using Shared.Configurations;

namespace Ordering.API.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection(nameof(SMTPEmailSetting))
            .Get<SMTPEmailSetting>();
        services.AddSingleton(emailSettings);//duy tri xuyen suot toan bo ung dung

        var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
            .Get<EventBusSettings>();
        services.AddSingleton(eventBusSettings);//duy tri xuyen suot toan bo ung dung neu su dung them RabbitMQ
        
        return services;
    }

    public static void ConfigureMassTransit(this IServiceCollection services)
    {
        var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
        if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
            throw new ArgumentNullException("EventBusSettings is not configured");
        
        var mqConnection = new Uri(settings.HostAddress);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(config =>
        {
            config.AddConsumersFromNamespaceContaining<BasketCheckOutEventHandler>();//consumer
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(mqConnection);
                // cfg.ReceiveEndpoint("basket-checkout-queue", c =>
                // {
                //     c.ConfigureConsumer<BasketCheckOutEventHandler>(ctx);
                // });
                cfg.ConfigureEndpoints(ctx);
            });
        });
    }
}