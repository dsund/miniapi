using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using mini.api.infrastructure;
using Polly;

namespace mini.api.extensions;
public static class WebHostExtensions
{
    public static void MigrateAndSeedDatabase(this IHost host)
    {
        host.MigrateDbContext<MiniApiDbContext>((context, services) =>
        {
        });
    }

    public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider>? seeder = null )
    where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var env = services.GetRequiredService<IWebHostEnvironment>();
        using var context = services.GetService<TContext>()!;
        //context.Database.EnsureCreated();

        try
        {
            var retryTimeSpans = new TimeSpan[] { TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(8) };
            Polly.Retry.RetryPolicy retry = Policy.Handle<SqlException>().WaitAndRetry(retryTimeSpans);
            retry.Execute(() =>
            {
                if (env.IsDevelopment())
                {
                    logger.LogDebug($"Migrating database associated with context {typeof(TContext).Name}");
                    context.Database.Migrate();
                    logger.LogDebug($"Migrated database associated with context {typeof(TContext).Name}");
                }

                if (seeder != null)
                {
                    logger.LogDebug($"Seeding database associated with context {typeof(TContext).Name}");
                    seeder(context, services);
                    logger.LogDebug($"Seeded database associated with context {typeof(TContext).Name}");
                }
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
        }
        return host;
    }
}