using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using miniapi.domain;
using miniapi.infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace miniapi.tests.common;
public class MiniApiApplicationFactory : WebApplicationFactory<Program>
{
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MiniApiDbContext>));
      if (descriptor != null)
        services.Remove(descriptor);
      services.AddDbContext<MiniApiDbContext>(options => options.UseInMemoryDatabase("Testing"));

      var sp = services.BuildServiceProvider();
      using (var scope = sp.CreateScope())
      using (var appContext = scope.ServiceProvider.GetRequiredService<MiniApiDbContext>())
      {
        try
        {
          appContext.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
          //Log errors or do anything you think it's needed
          throw;
        }
      }
    });

    var host = base.CreateHost(builder);
    return host;
  }

  public void TestCleanup()
  {
    using var scope = this.Services.CreateScope();
    using var applicationDbContext = scope.ServiceProvider.GetRequiredService<MiniApiDbContext>();
    applicationDbContext.Database.EnsureDeleted();
  }

  protected override void Dispose(bool disposing)
  {
    if (true)
    {
    }
  }

  public async Task<Item> SeedItemAsync(Item item)
  {
    using var scope = this.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MiniApiDbContext>();

    var result = await context.Items.AddAsync(item);
    await context.SaveChangesAsync();
    return await context.Items.SingleOrDefaultAsync(x => x.Id == result.Entity.Id);
  }
}