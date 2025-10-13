using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WiSave.Income.Infrastructure.EF.Factory;

public class RegistrationDbContextFactory : IDesignTimeDbContextFactory<RegistrationDbContext>
{
    public RegistrationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RegistrationDbContext>();
        
        var connectionString = "Host=localhost;Database=wisave_income;Username=postgres;Password=postgres123;Port=5432";
        
        optionsBuilder.UseNpgsql(
            connectionString,
            b => b.MigrationsAssembly("WiSave.Income.Infrastructure")
        );
        
        return new RegistrationDbContext(optionsBuilder.Options);
    }
}