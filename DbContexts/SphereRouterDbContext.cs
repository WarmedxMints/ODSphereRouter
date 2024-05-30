using Microsoft.EntityFrameworkCore;
using ODSphereRouter.DTOs;

namespace ODSphereRouter.DbContexts
{
    public sealed class SphereRouterDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<SphereDTO> Spheres { get; set; }

        public DbSet<RouteListDTO> RouteList { get; set; }

        public DbSet<StarSystemDTO> StarSystems { get; set; }
    }
}
