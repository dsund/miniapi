using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using mini.api.infrastructure;
using System.Reflection;

namespace mini.api.extensions;
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder SetupDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = "Server=.;Database=MiniDB;Trusted_Connection=True;";
        var defaultDbSchema = "dbo";

        builder.Services
          .AddDbContext<MiniApiDbContext>(
              options =>
              {
                  options.UseSqlServer(
                      connectionString,
                      sqlServerOptionsAction: sqlOptions =>
                      {
                          sqlOptions.MigrationsAssembly(typeof(MiniApiDbContext).GetTypeInfo().Assembly.GetName().Name);
                          sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, defaultDbSchema);
                          sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                      });
              },
              ServiceLifetime.Scoped); // Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)

        return builder;
    }
}
