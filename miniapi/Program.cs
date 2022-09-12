using miniapi.extensions;
using miniapi.infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.SetupDatabase();
var app = builder.Build();

app.MigrateAndSeedDatabase();


app.MapGet("/", () => "Hello World!");

app.MapGet("/items/", (HttpContext context) =>
{
    var dbContext = context.RequestServices.GetRequiredService<MiniApiDbContext>();
    return dbContext.Items.ToList();
});

app.Run();

public partial class Program { }
