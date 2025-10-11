using MassTransit;
using WiSave.Income.Application.CommandHandlers;
using WiSave.Income.Infrastructure.Configuration;
using WiSave.Shared.Income.Infrastructure.Configuration;
using WiSave.Shared.Income.Infrastructure.MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddConfiguration(builder.Configuration)
    .AddMassTransit<IIncomeBus>(x =>
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

var host = builder.Build();

host.Run();