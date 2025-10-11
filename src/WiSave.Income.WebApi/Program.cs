using MassTransit;
using Scalar.AspNetCore;
using WiSave.Income.WebApi.Configuration;
using WiSave.Income.WebApi.Endpoints;
using WiSave.Shared.Income.Infrastructure.Configuration;
using WiSave.Shared.Income.Infrastructure.MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddWiSaveConfiguration(builder.Configuration)
    .AddMassTransit<IIncomeBus>(x =>
    {
        x.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(".", "", true));
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTheme(ScalarTheme.Kepler)
            .WithDarkModeToggle()
            .WithClientButton();
    });
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();