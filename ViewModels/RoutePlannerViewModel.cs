using ODSphereRouter.Commands;
using ODSphereRouter.Models;
using ODSphereRouter.Stores;
using ODUtils.Commands;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace ODSphereRouter.ViewModels
{
    public class RoutePlannerViewModel : ViewModelBase
    {
        private readonly NavigationViewModel navigationViewModel;
        private readonly JournalWatcherStore journalWatcherStore;
        private readonly SettingsStore settingsStore;
        private readonly SphereDataStore _dataStore;
        private readonly ObservableCollection<SphereViewModel> _spheres;
        private readonly ObservableCollection<RouteListViewModel> _routeList;
        public IEnumerable<SphereViewModel> SphereList => _spheres;
        public IEnumerable<RouteListViewModel> RouteList => _routeList;
        public ObservableCollection<string> MessageLog => _dataStore.MessageLog;

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public SettingsStoreViewModel Settings => settingsStore.ViewModel;

        private string customSystemsEntry = string.Empty;
        public string CustomSystemsEntry 
        { 
            get => customSystemsEntry; 
            set 
            { 
                customSystemsEntry = value;
                _dataStore.CustomRouteList = value;
                OnPropertyChanged(); 
            } 
        }

        public ICommand LoadSphereDataCommand { get; }
        public IAsyncRelayCommand<SphereViewModel> AddSphereToRouteCommand { get; }
        public IAsyncRelayCommand<SphereViewModel> RemoveSphereFromRouteCommand { get; }
        public IAsyncRelayCommand ClearRouteListCommand { get; }
        public IAsyncRelayCommand<StarSystem> RemoveSystemFromRouteCommand { get; }
        public IAsyncRelayCommand AddCustomListToRouteCommand { get; }
        public IAsyncRelayCommand RemoveCustomListToRouteCommand { get; }
        public ICommand NavigateToRouteMap { get; }

        public RoutePlannerViewModel(SphereDataStore sphereDataStore, NavigationViewModel navigationViewModel, JournalWatcherStore journalWatcherStore, SettingsStore settingsStore)
        {
            _dataStore = sphereDataStore;
            this.navigationViewModel = navigationViewModel;
            this.journalWatcherStore = journalWatcherStore;
            this.settingsStore = settingsStore;
            _spheres = [];
            _routeList = [];
            CustomSystemsEntry = _dataStore.CustomRouteList;

            LoadSphereDataCommand = new LoadSphereDataCommand(this, sphereDataStore);
            AddSphereToRouteCommand = new AsyncRelayCommand<SphereViewModel>(OnAddSphereToRoute);
            RemoveSphereFromRouteCommand = new AsyncRelayCommand<SphereViewModel>(OnRemoveSphereFromRoute);
            ClearRouteListCommand = new AsyncRelayCommand(OnClearRouteList, () => RouteList.Any());
            RemoveSystemFromRouteCommand = new AsyncRelayCommand<StarSystem>(OnRemoveSystemFromRoute);
            AddCustomListToRouteCommand = new AsyncRelayCommand(OnAddCustomList, () => customSystemsEntry.Length > 0);
            RemoveCustomListToRouteCommand = new AsyncRelayCommand(OnRemoveCustomList, () => customSystemsEntry.Length > 0);
            NavigateToRouteMap = new RelayCommand(OnNavigateToRouteMap, (_) => RouteList.Any());
            _spheres.CollectionChanged += OnSphereCollectionChanged;
            _routeList.CollectionChanged += OnRouteListCollectionChanged;
        }

        private void OnNavigateToRouteMap(object? obj)
        {
            navigationViewModel.RouteMapCommand.Execute(obj);
        }


        private async Task OnRemoveCustomList()
        {
            var systemsToModify = CustomSystemsEntry.Split(",");

            var systemsToRemove = new List<SphereSystemViewModel>();

            foreach (var system in systemsToModify)
            {
                var knownSystem = _dataStore.GetSystem(system);

                if (knownSystem != null)
                {
                    systemsToRemove.Add(new(knownSystem));
                    continue;
                }
            }

            var oldCount = _dataStore.RouteList.Count();
            var ret = await _dataStore.RemoveSystemsFromRouteList(systemsToRemove);
            UpdateRouteList(ret);
            var newCount = ret.Count();
            _dataStore.AddToMessageLog($"Removed {oldCount - newCount:N0} systems from route");
        }

        private async Task OnAddCustomList()
        {
            var systemsToModify = CustomSystemsEntry.Split(",");

            var systemsToAdd = new List<SphereSystemViewModel>();

            foreach(var system in systemsToModify)
            {
                var knownSystem = _dataStore.GetSystem(system);

                if(knownSystem != null)
                {
                    systemsToAdd.Add(new(knownSystem));
                    continue;
                }

                _dataStore.AddToMessageLog($"ERROR : Could not find {system} in the database");
            }

            var oldCount = _dataStore.RouteList.Count();
            var ret = await _dataStore.UpdateRouteList(systemsToAdd);
            UpdateRouteList(ret);
            var newCount = ret.Count();
            _dataStore.AddToMessageLog($"Added {newCount - oldCount:N0} systems to route");
        }

        private async Task OnRemoveSystemFromRoute(StarSystem system)
        {
            _= await RemoveSystemsFromRoute(new SphereSystemViewModel[] { new(system) }, false);
        }


        private async Task OnClearRouteList()
        {
            var ret = await _dataStore.ClearRouteList();

            UpdateRouteList(ret);
        }

        private async Task OnRemoveSphereFromRoute(SphereViewModel sphere)
        {
            if (sphere is null || sphere.Systems.Any() == false)
            {
                return;
            }

            var oldCount = _dataStore.RouteList.Count();
            var newCount = await RemoveSystemsFromRoute(sphere.Systems);

            _dataStore.AddToMessageLog($"Removed {oldCount - newCount:N0} systems from the {sphere.ControllingSystem} sphere");
        }

        private async Task<int> RemoveSystemsFromRoute(IEnumerable<SphereSystemViewModel> systems, bool includeState = true)
        {
            var systemsToRemove = Settings.ExploitedControlledOnly && includeState ? systems.Where(x => x.StarSystem.PowerState == PowerPlayState.Controlled || x.StarSystem.PowerState == PowerPlayState.Exploited) : systems;

            var newList = await _dataStore.RemoveSystemsFromRouteList(systemsToRemove);

            UpdateRouteList(newList);

            return newList.Count();
        }

        private async Task OnAddSphereToRoute(SphereViewModel sphere)
        {
            if (sphere is null || sphere.Systems.Any() == false)
            {
                return;
            }

            var oldCount = _dataStore.RouteList.Count();
            var systems = sphere.Systems;
            var systemsToAdd = Settings.ExploitedControlledOnly ? systems.Where(x => x.StarSystem.PowerState == PowerPlayState.Controlled || x.StarSystem.PowerState == PowerPlayState.Exploited) : systems;

            var newList = await _dataStore.UpdateRouteList(systemsToAdd);
            UpdateRouteList(newList);

            var newCount = newList.Count();

            _dataStore.AddToMessageLog($"Added {newCount - oldCount:N0} systems from the {sphere.ControllingSystem} sphere");
        }

        public static RoutePlannerViewModel LoadViewModel(SphereDataStore sphereDataStore, NavigationViewModel navigationViewModel, JournalWatcherStore journalWatcherStore, SettingsStore settingsStore)
        {
            var ret = new RoutePlannerViewModel(sphereDataStore, navigationViewModel, journalWatcherStore, settingsStore);
            ret.LoadSphereDataCommand.Execute(null);
            return ret;
        }

        private void OnSphereCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SphereList));
        }

        private void OnRouteListCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RouteList));
        }

        internal void UpdateLists(SphereDataStore dataStore)
        {
            UpdateSphereList(dataStore.Spheres);

            UpdateRouteList(dataStore.RouteList);
        }

        private void UpdateSphereList(IEnumerable<Sphere> spheres)
        {
            _spheres.Clear();

            foreach (Sphere sp in spheres)
            {
                var sphereView = new SphereViewModel(sp, _dataStore);
                _spheres.Add(sphereView);
            }

            OnPropertyChanged(nameof(SphereList));
        }

        private void UpdateRouteList(IEnumerable<RouteListItem> routeList)
        {
            _routeList.Clear();

            if (routeList.Any())
            {
                foreach (RouteListItem item in routeList)
                {
                    var knownSystem = _dataStore.GetSystem(item.Name);

                    //TODO Log Error
                    if(knownSystem == null)
                    {
                        continue;
                    }

                    var routeListItem = new RouteListViewModel(item, knownSystem);
                    _routeList.Add(routeListItem);
                }
            }

            OnPropertyChanged(nameof(RouteList));
        }
    }
}
