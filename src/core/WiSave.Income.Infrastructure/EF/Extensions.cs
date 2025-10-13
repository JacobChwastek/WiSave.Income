using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WiSave.Shared.Income.Infrastructure.Configuration;

namespace WiSave.Income.Infrastructure.EF;

public static class Extensions
{
    public static IServiceCollection AddEf(this IServiceCollection services)
    {
        services.AddDbContext<RegistrationDbContext>((context, options) =>
        {
            var config = context.GetRequiredService<IWiSaveIncomeConfiguration>();
            options.UseNpgsql(config.Database.GeneralDbConnectionString);
        });
        
        return services;
    }
}