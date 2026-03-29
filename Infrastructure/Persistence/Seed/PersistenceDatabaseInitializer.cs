using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Seed;

public static class PersistenceDatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IHostEnvironment environment, CancellationToken ct = default)
    {
        if (environment.IsDevelopment())
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EfCore.Contexts.DataContext>();
            await context.Database.EnsureCreatedAsync(ct);
        }
        else
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EfCore.Contexts.DataContext>();
            await context.Database.MigrateAsync(ct);
        }
    }
}
