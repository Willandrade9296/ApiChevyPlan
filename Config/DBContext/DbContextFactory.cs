using Microsoft.EntityFrameworkCore;

namespace ApiDatalife.Config.DBContext
{

    public interface IDbContextFactory
    {
        DynamicDbContext CreateDbContext(string connectionString);
    }

    public class DbContextFactory : IDbContextFactory
    {
        private readonly DbContextOptions<DynamicDbContext> _options;

        public DbContextFactory(DbContextOptions<DynamicDbContext> options)
        {
            _options = options;
        }

        public DynamicDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>(_options)
                .UseSqlServer(connectionString);

            return new DynamicDbContext(optionsBuilder.Options);
        }
    }
}
