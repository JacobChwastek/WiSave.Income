using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiSave.Income.Application.CommandHandlers;
using WiSave.Income.Infrastructure.Configuration;
using WiSave.Income.Infrastructure.EventStore;
using WiSave.Shared.Income.Infrastructure.Configuration;
using WiSave.Shared.Income.Infrastructure.MassTransit;

namespace WiSave.Income.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguration(configuration);
        
        services.AddEventStore();
        
        services.AddMassTransit<IIncomeBus>(x =>
        {
            x.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(".", "", true));

            x.AddConsumer<CreateIncomeCommandHandler>();
        
            x.UsingRabbitMq((context, cfg) =>
            {
                var config = context.GetRequiredService<IWiSaveIncomeConfiguration>();
                cfg.Host(config.RabbitMq.Host, config.RabbitMq.VirtualHost, h =>
                {
                    h.Username(config.RabbitMq.Username);
                    h.Password(config.RabbitMq.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}