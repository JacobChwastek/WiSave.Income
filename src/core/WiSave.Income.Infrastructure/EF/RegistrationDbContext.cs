using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace WiSave.Income.Infrastructure.EF;

public class RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}