using Microsoft.EntityFrameworkCore;

namespace ODSphereRouter.DbContexts
{
    public class SphereRouterDbContextFactory(string connectionString) : ISphereRouterDbContextFactory
    {
        private readonly string _connectionString = connectionString;

        public SphereRouterDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;

            return new SphereRouterDbContext(options);
        }
    }
}