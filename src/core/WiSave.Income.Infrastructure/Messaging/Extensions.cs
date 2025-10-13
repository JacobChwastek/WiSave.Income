using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using WiSave.Income.Application.CommandHandlers;
using WiSave.Income.Infrastructure.EF;
using WiSave.Shared.Income.Infrastructure.Configuration;

namespace WiSave.Income.Infrastructure.Messaging;

public static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(".", "", true));

            x.AddConsumer<CreateIncomeCommandHandler>();
            
            x.AddEntityFrameworkOutbox<RegistrationDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
            });
            
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
            
            x.AddConfigureEndpointsCallback((context, name, cfg) =>
            {
                cfg.UseEntityFrameworkOutbox<RegistrationDbContext>(context);
            });
        });
        
        return services;
    }
}