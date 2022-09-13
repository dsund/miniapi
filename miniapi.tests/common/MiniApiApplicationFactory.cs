using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using miniapi.domain;
using miniapi.infrastructure;
using System.Threading.Tasks;

namespace miniapi.tests.common;
public class MiniApiApplicationFactory : WebApplicationFactory<Program>
{
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      var root = new InMemoryDatabaseRoot();

      builder.ConfigureServices(services =>
          {
          services.RemoveAll(typeof(DbContextOptions<MiniApiDbContext>));
          services.AddDbContext<MiniApiDbContext>(options =>
                  options.UseInMemoryDatabase("Testing", root));
        });
    });

    var host = base.CreateHost(builder);

    //using var scope = host.Services.CreateScope();
    //var services = scope.ServiceProvider;
    //using var dbContext = services.GetRequiredService<MiniApiDbContext>();
    //dbContext.Database.EnsureCreated();
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