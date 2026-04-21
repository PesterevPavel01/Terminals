using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;
using Terminals.Contracts.Entities;

namespace Terminals.Infrastructure;

public class DellinDictionaryDbContext : DbContext
{
    public DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
        {
            Database.Migrate();
        }
    }

    public DbSet<Office> Offices { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public class DellinDictionaryDbContexFactory : IDesignTimeDbContextFactory<DellinDictionaryDbContext>
    {
        public DellinDictionaryDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Host=localhost;Port=5432;Database=dellin_terminals;Username=admin;Password=admin;";

            var optionsBuilder = new DbContextOptionsBuilder<DellinDictionaryDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new DellinDictionaryDbContext(optionsBuilder.Options);
        }
    }
}