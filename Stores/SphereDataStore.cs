using ODSphereRouter.Models;
using ODSphereRouter.Services.Database;
using ODSphereRouter.ViewModels;
using System.Collections.ObjectModel;
namespace ODSphereRouter.Stores
{
    public sealed class SphereDataStore
    {
        private readonly ISphereRouterDatabaseProvider _sphereDatabaseProvider;

        private Lazy<Task> _initializeLazy;

        private readonly List<Sphere> _spheres;
        private readonly List<StarSystem> _starSystems;
        private readonly List<RouteListItem> _routeList;
        private readonly ObservableCollection<string> _messageLog;

        private string customRouteList = string.Empty;

        public string CustomRouteList { get => customRouteList;  set => customRouteList = value; }
        public IEnumerable<Sphere> Spheres => _spheres;
        public IEnumerable<RouteListItem> RouteList => _routeList;
        public ObservableCollection<string> MessageLog => _messageLog;

        public SphereDataStore(ISphereRouterDatabaseProvider databaseProvider)
        {
            _sphereDatabaseProvider = databaseProvider;

            _initializeLazy = new Lazy<Task>(Initialise);
            _spheres = [];
            _starSystems = [];
            _routeList = [];
            _messageLog = [];
        }

        public async Task Load()
        {
            try
            {
                await _initializeLazy.Value;
            }
            catch (Exception)
            {
                _initializeLazy = new Lazy<Task>(Initialise);
                throw;
            }
        }

        private async Task Initialise()
        {
            var spheres = await _sphereDatabaseProvider.GetAllSpheres();

            _spheres.Clear();
            _spheres.AddRange(spheres);

            var starSystems = await _sphereDatabaseProvider.GetAllSystems();

            _starSystems.Clear();
            _starSystems.AddRange(starSystems);

            var routList = await _sphereDatabaseProvider.GetRouteList();

            UpdateRouteList(routList);
        }

        public StarSystem? GetSystem(string systemName)
        {
            return _sphereDatabaseProvider.GetSystemByName(systemName);
        }

        public async ValueTask<IEnumerable<RouteListItem>> UpdateRouteList(IEnumerable<SphereSystemViewModel> systems)
        {
            await _sphereDatabaseProvider.UpdateRouteList(systems);

            var routList = await _sphereDatabaseProvider.GetRouteList();

            UpdateRouteList(routList);

            return _routeList;
        }

        public async ValueTask<IEnumerable<RouteListItem>> RemoveSystemsFromRouteList(IEnumerable<SphereSystemViewModel> systems)
        {
            await _sphereDatabaseProvider.RemoveSystemsFromRouteList(systems);

            var routList = await _sphereDatabaseProvider.GetRouteList();

            UpdateRouteList(routList);

            return _routeList;
        }

        public async ValueTask<IEnumerable<RouteListItem>> RemoveSystem(SphereSystemViewModel systemToRemove)
        {
            return await RemoveSystemsFromRouteList(new SphereSystemViewModel[] { systemToRemove });
        }

        public async ValueTask<IEnumerable<RouteListItem>> ClearRouteList()
        {
            var routList = await _sphereDatabaseProvider.ClearRouteList();

            UpdateRouteList(routList);

            return _routeList;
        }

        private void UpdateRouteList(IEnumerable<RouteListItem> routList)
        {
            _routeList.Clear();
            _routeList.AddRange(routList);
        }

        public void AddToMessageLog(string message)
        {
            _messageLog.Add(message);
        }
    }
}
