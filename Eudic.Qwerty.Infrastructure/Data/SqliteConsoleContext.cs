namespace Eudic.Qwerty.Infrastructure.Data
{
    using Eudic.Qwerty.Core.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Entity framework context
    /// </summary>
    public class SqliteConsoleContext : DbContext
    {
        public SqliteConsoleContext(DbContextOptions<SqliteConsoleContext> options)
            : base(options)
        { }

        public DbSet<Example> Examples { get; set; }

        public DbSet<Dict> StarDict { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Example>()
                .Property(e => e.Name)
                .HasColumnType("varchar(512)");

            builder.Entity<Dict>().ToTable("stardict");
        }
    }

    public static class SqliteConsoleContextFactory
    {
        public static SqliteConsoleContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqliteConsoleContext>();
            optionsBuilder.UseSqlite(connectionString);

            var context = new SqliteConsoleContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}