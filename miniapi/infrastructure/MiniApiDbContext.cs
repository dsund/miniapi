using Microsoft.EntityFrameworkCore;
using mini.api.domain;

namespace mini.api.infrastructure;
public class MiniApiDbContext : DbContext
{
    private readonly string schema;
    public MiniApiDbContext(DbContextOptions<MiniApiDbContext> options)
        : this(options, "identity")
    {
    }

    public MiniApiDbContext(DbContextOptions<MiniApiDbContext> options, string schema)
    : base(options)
    {
        this.schema = schema;
    }

    public DbSet<Item> Items => Set<Item>();
}
