using ODSphereRouter.DTOs;
using ODSphereRouter.Models;
using ODSphereRouter.ViewModels;

namespace ODSphereRouter.Services.Database
{
    public interface ISphereRouterDatabaseProvider
    {
        Task<IEnumerable<Sphere>> GetAllSpheres();
        Task<IEnumerable<StarSystem>> GetAllSystems();
        Task<IEnumerable<StarSystemDTO>> GetAllSystemDTOs();
        Task<int> UpdateStarSystems(IEnumerable<StarSystemDTO> starSystems);
        Task<IEnumerable<RouteListItem>> GetRouteList();
        Task UpdateRouteList(IEnumerable<SphereSystemViewModel> sphereSystemViewModels);
        Task RemoveSystemsFromRouteList(IEnumerable<SphereSystemViewModel> sphereSystemViewModels);
        Task<IEnumerable<RouteListItem>> ClearRouteList();
        void UpdateSystemFromLogs(StarSystemDTO system);
        StarSystem? GetSystemByName(string name);
    }
}
