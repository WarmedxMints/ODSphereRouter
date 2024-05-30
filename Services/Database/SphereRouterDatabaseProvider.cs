using Microsoft.EntityFrameworkCore;
using ODSphereRouter.DbContexts;
using ODSphereRouter.DTOs;
using ODSphereRouter.Models;
using ODSphereRouter.ViewModels;

namespace ODSphereRouter.Services.Database
{
    public sealed class SphereRouterDatabaseProvider(ISphereRouterDbContextFactory dbContextFactory) : ISphereRouterDatabaseProvider
    {
        private readonly ISphereRouterDbContextFactory _dbContextFactory = dbContextFactory;

        public async Task<IEnumerable<Sphere>> GetAllSpheres()
        {
            using var context = _dbContextFactory.CreateDbContext();

            var spheres = await context.Spheres.OrderBy(x => x.Name).ToListAsync();

            return spheres.Select(sp => new Sphere(sp));
        }

        public async Task<IEnumerable<StarSystem>> GetAllSystems()
        {
            using var context = _dbContextFactory.CreateDbContext();

            var systems = await context.StarSystems.OrderBy(x => x.Name).ToListAsync();

            return  systems.Select(s => new StarSystem(s));
        }

        public async Task<IEnumerable<StarSystemDTO>> GetAllSystemDTOs()
        {
            using var context = _dbContextFactory.CreateDbContext();

            var systems = await context.StarSystems.OrderBy(x => x.Name).ToListAsync();

            return systems;
        }

        public StarSystem? GetSystemByName(string name)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var system = context.StarSystems.Where(x => x.Name == name.Trim()).FirstOrDefault();

            return system is null ? null : new StarSystem(system);
        }

        public async Task<IEnumerable<RouteListItem>> GetRouteList()
        {
            using var context = _dbContextFactory.CreateDbContext();

            var systems = await context.RouteList.OrderBy(x => x.System).ToListAsync();

            return systems.Select(s => new RouteListItem(s));
        }

        public void UpdateSystemFromLogs(StarSystemDTO system)
        {
            using var context = _dbContextFactory.CreateDbContext();

            context.StarSystems
                    .Upsert(system)
                    .On(s => s.Address)                    
                    .WhenMatched((matched, newItem) => new StarSystemDTO()
                    {
                        Powers = newItem.Powers,
                        PowerState = newItem.PowerState,
                        Population = newItem.Population,
                    })
                    .Run();
        }

        public async Task<int> UpdateStarSystems(IEnumerable<StarSystemDTO> starSystems)
        {
            using var context = _dbContextFactory.CreateDbContext();

            return await context.StarSystems                    
                    .UpsertRange(starSystems)                    
                    .On(s => s.Address)
                    .UpdateIf((matched, newItem) => matched.Powers != newItem.Powers || matched.PowerState != matched.PowerState)
                    .WhenMatched((matched, newItem) => new StarSystemDTO()
                    {
                        Powers = newItem.Powers,
                        PowerState = newItem.PowerState,
                    })
                    .RunAsync();
        }

        public async Task UpdateRouteList(IEnumerable<SphereSystemViewModel> sphereSystemViewModels)
        {
            using var context = _dbContextFactory.CreateDbContext();

            await context.RouteList
                .UpsertRange(sphereSystemViewModels.Select(s => new RouteListDTO() { Address = s.StarSystem.Address, System = s.Name }))
                .On(s => s.Address)
                .NoUpdate()
                .RunAsync();                
        }

        public async Task RemoveSystemsFromRouteList(IEnumerable<SphereSystemViewModel> sphereSystemViewModels)
        {
            var items = sphereSystemViewModels.Select(s => new RouteListDTO() { Address = s.StarSystem.Address, System = s.Name });

            var currentList = await GetRouteList();

            var itemsToRemove = items.Where(rl => currentList.FirstOrDefault(item => item.Name == rl.System) != null);

            using var context = _dbContextFactory.CreateDbContext();

            context.RouteList.RemoveRange(itemsToRemove);

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RouteListItem>> ClearRouteList()
        {
            using var context = _dbContextFactory.CreateDbContext();

            var systems = await context.RouteList.ToListAsync();

            context.RouteList.RemoveRange(systems);

            await context.SaveChangesAsync();

            systems = await context.RouteList.ToListAsync();
            return systems.Select(s => new RouteListItem(s));
        }
    }
}