using ODSphereRouter.Models;
using ODSphereRouter.Stores;
using ODUtils.Commands;
using ODUtils.Dialogs;
using ODUtils.Helpers;
using ODUtils.Models;
using ODUtils.PathFinding;
using ODUtils.Wpf3D;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Application = System.Windows.Application;

namespace ODSphereRouter.ViewModels
{
    public sealed class RouteMapViewModel : ViewModelBase
    {
        private readonly SphereDataStore sphereDataStore;
        private readonly JournalWatcherStore journalWatcherStore;
        private readonly SettingsStore settingsStore;
        private readonly List<RouteStopViewModel> route;
        private bool calculatingRoute;
        private RouteStopViewModel? selectedItem;
        public RouteStopViewModel? SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));

                if (selectedItem != null)
                {
                    OnSeletedItemChanged?.Invoke(this, selectedItem);                 
                }
            }
        }

        public SettingsStoreViewModel Settings => settingsStore.ViewModel;

        private string distance = "0 ly";
        public string Distance { get => distance; set { distance = value; OnPropertyChanged(nameof(Distance)); } }

        private string systemCount = "0";
        public string SystemCount { get => systemCount; set { systemCount = value; OnPropertyChanged(nameof(SystemCount)); } }

        private string startSystem = "Cubeo";
        public string StartSystem { get => startSystem; set { startSystem = value; OnPropertyChanged(nameof(StartSystem)); } }

        public IEnumerable<RouteStopViewModel> Route => route;
        public IAsyncRelayCommand GenerateRouteCommand { get; }
        public ICommand ChangeSelectedItem { get; }
        public ICommand CopySelectedItemToClipboard { get; }
        public ICommand ZoomMapOut { get; }
        public ICommand ZoomMapIn { get; }
        public ICommand MapLeft { get; }
        public ICommand MapRight { get; }
        public ICommand MapUp { get; }
        public ICommand MapDown { get; }
        public IAsyncRelayCommand SetSelectedAsStart { get; }
        public IAsyncRelayCommand SetCurrentAsStart { get; }

        public EventHandler<RouteStopViewModel>? OnSeletedItemChanged;
        public EventHandler<RouteStopViewModel>? OnSystemNameCopiedToClipboard;
        public RouteMapViewModel(SphereDataStore sphereDataStore, JournalWatcherStore journalWatcherStore, SettingsStore settingsStore)
        {
            this.sphereDataStore = sphereDataStore;
            this.journalWatcherStore = journalWatcherStore;
            this.settingsStore = settingsStore;
            route = [];

            ChangeSelectedItem = new RelayCommand<bool>(OnChangeSelectedItem, (_) => Route.Any());
            CopySelectedItemToClipboard = new RelayCommand(OnCopyToClipboard, (_) => selectedItem != null);
            ZoomMapOut = new RelayCommand(OnMapZoomOut, (_) => ModelGroup.Children.Count > 0);
            ZoomMapIn = new RelayCommand(OnZoomMapIn, (_) => ModelGroup.Children.Count > 0);
            MapLeft = new RelayCommand(OnMapLeft, (_) => ModelGroup.Children.Count > 0);
            MapRight = new RelayCommand(OnMapRight, (_) => ModelGroup.Children.Count > 0);
            MapUp = new RelayCommand(OnMapUp, (_) => ModelGroup.Children.Count > 0);
            MapDown = new RelayCommand(OnMapDown, (_) => ModelGroup.Children.Count > 0);
            SetSelectedAsStart = new AsyncRelayCommand(OnSetSelectedAsStart, () =>  !calculatingRoute && SelectedItem is not null);
            SetCurrentAsStart = new AsyncRelayCommand(OnSelectCurrentAsStart, () => Route.Any() && !calculatingRoute && string.IsNullOrEmpty(this.journalWatcherStore.CurrentSystem) == false );

            StartSystem = journalWatcherStore.CurrentSystem ?? "Cubeo";
            journalWatcherStore.OnCurrentSystemChanged += OnCurrentSystemChanged;
            GenerateRouteCommand = new AsyncRelayCommand(OnGenerateRoute, () => sphereDataStore.RouteList.Any() && !calculatingRoute);
            _= OnGenerateRoute();
        }

        public override void Dispose()
        {
            journalWatcherStore.OnCurrentSystemChanged -= OnCurrentSystemChanged;
            base.Dispose();
        }

        private void OnCurrentSystemChanged(object? sender, string? e)
        {
            if (e != null)
            {
                CheckIfNext(e);
            }
        }

        private async Task OnSelectCurrentAsStart()
        {
            if (string.IsNullOrEmpty(journalWatcherStore.CurrentSystem))
            {
                return;
            }
            StartSystem = journalWatcherStore.CurrentSystem;
            await GenerateRouteCommand.ExecuteAsync();
        }

        private async Task OnSetSelectedAsStart()
        {
            if (SelectedItem is null)
            {
                return;
            }
            StartSystem = SelectedItem.Name;
            await GenerateRouteCommand.ExecuteAsync();
        }

        private void OnCopyToClipboard(object? obj)
        {
            if (selectedItem != null && ClipboardHelper.SetStringToClipboard(selectedItem.Name))
            {
                OnSystemNameCopiedToClipboard?.Invoke(this, selectedItem);
            }
        }

        private void CheckIfNext(string systemName)
        {
            if (SelectedItem is not null &&
                string.Compare(SelectedItem.Name, systemName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ChangeRow(1);
                });
            }
        }

        private void OnChangeSelectedItem(bool up)
        {
            ChangeRow(up ? -1 : 1);
        }

        private void ChangeRow(int value)
        {
            if (selectedItem == null)
            {
                return;
            }

            var newIndex = route.IndexOf(selectedItem) + value;

            var count = route.Count - 1;

            if (newIndex < 1)
            {
                newIndex = count - 1;
            }
            if (newIndex > count)
            {
                newIndex = 1;
            }
            var item = route[newIndex];
            SelectedItem = item;
            ColourMap(newIndex);
            if(Settings.AutoCopyToClipboard) 
            {
                OnCopyToClipboard(null);
            }
        }

        private async Task OnGenerateRoute()
        {
            if (sphereDataStore.RouteList.Any() == false)
            {
                return;
            }

           
            var stops = new List<RouteStop>();

            var startSystem = sphereDataStore.GetSystem(this.startSystem);

            if(startSystem is null)
            {
                _ = ODMessageBox.Show(null, "Error", $"Start System {this.startSystem} not found in the database", System.Windows.MessageBoxButton.OK);
                return;
            }

            calculatingRoute = true;
            var firtStop = new RouteStop(startSystem.Pos, startSystem);

            foreach (var sys in sphereDataStore.RouteList)
            {
                var knownSystem = sphereDataStore.GetSystem(sys.Name);

                //Should never happen but keeps the compiler happy
                if (knownSystem == null)
                { continue; }

                stops.Add(new(knownSystem.Pos, knownSystem));
            }

            var routeCalc = new TravellingSalesmAlgorithm(firtStop, stops);
            var routeStops = await routeCalc.GetRouteAsync();

            if (routeStops == null)
            {
                return;
            }
            stops.Clear();
            stops = routeStops.ToList();

            var count = stops.Count;
            double totalDistance = 0;
            route.Clear();

            route.Add(new RouteStopViewModel((StarSystem)stops[0].System));

            for (var i = 1; i < count; i++)
            {
                var modelToAdd = new RouteStopViewModel((StarSystem)stops[i].System, (StarSystem)stops[i - 1].System);
                route.Add(modelToAdd);
                totalDistance += modelToAdd.Distance;
            }

            var endSystem = new RouteStopViewModel((StarSystem)stops[0].System, (StarSystem)stops[count - 1].System);
            route.Add(endSystem);
            totalDistance += endSystem.Distance;

            Distance = $"{totalDistance:N2} ly";
            SystemCount = $"{route.Count - 1:N0}";
            SelectedItem = route[0];
            OnPropertyChanged(nameof(Route));

            CreateMap();
            ChangeRow(1);
            calculatingRoute = false;
        }

        #region 3D map
        private Model3DGroup modelGroup = new();
        public Model3DGroup ModelGroup { get => modelGroup; set { modelGroup = value; OnPropertyChanged(); } }
        private Dictionary<string, GeometryModel3D> SystemSpheres = [];
        private Dictionary<string, GeometryModel3D> SystemLines = [];

        readonly DiffuseMaterial green = new(new SolidColorBrush(Colors.Green));
        readonly DiffuseMaterial grey = new(new SolidColorBrush(Colors.LightSlateGray));
        readonly DiffuseMaterial red = new(new SolidColorBrush(Colors.Red));
        readonly DiffuseMaterial blue = new(new SolidColorBrush(Colors.SkyBlue));
        readonly DiffuseMaterial axisblue = new(new SolidColorBrush(Colors.Blue));
        // The camera.
        private readonly PerspectiveCamera TheCamera = new();
        public PerspectiveCamera PerspectiveCamera { get => TheCamera; }
        // The camera's current location.
        private double CameraPhi = Math.PI / 3.0;
        private double CameraTheta = Math.PI / 3.0;
        private double CameraR = 3.0;

        // The change in CameraPhi when you press the up and down arrows.
        private const double CameraDPhi = 0.1;

        // The change in CameraTheta when you press the left and right arrows.
        private const double CameraDTheta = 0.1;

        // The change in CameraR when you press + or -.
        private const double CameraDR = 5;

        private void CreateMap()
        {
            if (route.Count == 0)
            {
                return;
            }

            var routeCount = route.Count;
            SystemSpheres = [];
            SystemLines = [];

            var centroid = new Position(0, 0, 0);

            foreach (var system in route)
            {
                centroid += system.Pos;
            }

            centroid /= routeCount;

            Model3DGroup gr = new();
            gr.Children.Add(new AmbientLight());

            //Axis arrows
            //var line3 = MeshGeneration.CreatBoxLine(new Position(0, -0.5, 0), new Position(0, 5, 0), green, 1, true);
            //gr.Children.Add(line3);
            //line3 = MeshGeneration.CreatBoxLine(new Position(0.5, 0, 0), new Position(5, 0, 0), red, 1, true);
            //gr.Children.Add(line3);
            //line3 = MeshGeneration.CreatBoxLine(new Position(0, 0, -0.5), new Position(0, 0, -5), axisblue, 1, true);
            //gr.Children.Add(line3);

            for (var i = 0; i < routeCount - 1; i++)
            {
                var sys = route[i];
                var start = sys.Pos - centroid;
                //ED uses a Z+ system where as wpf seems to use a Z- so we need to flip it
                start = start.FlipZ;
                var sphere = MeshGeneration.CreateSphere(start, grey, 2, 10, 10);
                SystemSpheres.Add(sys.Name, sphere);
                gr.Children.Add(sphere);
                Position end = new(0,0,0);
                if (i == routeCount - 2)
                {
                    end = route[0].Pos - centroid;
                    end = end.FlipZ;
                    var line = MeshGeneration.CreatBoxLine(start, end, grey, 1);
                    SystemLines.Add(sys.Name, line);
                    gr.Children.Add(line);
                    continue;
                }
                end = route[i + 1].Pos - centroid;
                end = end.FlipZ;
                var line1 = MeshGeneration.CreatBoxLine(start, end, grey, 1);
                gr.Children.Add(line1);
                SystemLines.Add(sys.Name, line1);
            }

            var maxDistance = route.Max(x => x.Pos.DistanceFrom(centroid));
            CameraR = maxDistance * 3;
            CameraTheta = Math3d.ConvertToRadians(90);// 3.14159;
            CameraPhi = 0;
            PositionCamera();
            ModelGroup = gr;
        }

        private void ColourMap(int index)
        {
            GetSystemNames(index, out string? fromSystem, out string? toSystem);

            foreach (var sphere in SystemSpheres)
            {
                if (sphere.Key.Equals(fromSystem))
                {
                    sphere.Value.Material = green;
                    continue;
                }
                if (sphere.Key.Equals(toSystem))
                {
                    sphere.Value.Material = red;
                    continue;
                }
                sphere.Value.Material = grey;
            }

            foreach (var system in SystemLines)
            {
                if (system.Key.Equals(fromSystem))
                {
                    system.Value.Material = blue;
                    continue;
                }
                system.Value.Material = grey;
            }
        }

        private void GetSystemNames(int index, out string fromSystem, out string toSystem)
        {
            if (index > 0 && index < route.Count)
            {
                fromSystem = route[index - 1].Name;
                toSystem = route[index].Name;
                return;
            }
            if (index == 0)
            {
                fromSystem = route[index].Name;
                toSystem = route[index + 1].Name;
                return;
            }
            fromSystem = route[index - 1].Name;
            toSystem = route[index].Name;
        }

        private void PositionCamera()
        {
            if (TheCamera == null) return;
            // Calculate the camera's position in Cartesian coordinates.
            double y = CameraR * Math.Sin(CameraPhi);
            double hyp = CameraR * Math.Cos(CameraPhi);
            double x = hyp * Math.Cos(CameraTheta);
            double z = hyp * Math.Sin(CameraTheta);
            TheCamera.Position = new Point3D(x, y, z);
            // Look toward the origin.
            TheCamera.LookDirection = new Vector3D(-x, -y, -z);
            // Set the Up direction.
            TheCamera.UpDirection = new Vector3D(0, 1, 0);
        }

        private void OnMapDown(object? obj)
        {
            CameraPhi -= CameraDPhi;
            if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;
            PositionCamera();
        }

        private void OnMapUp(object? obj)
        {
            CameraPhi += CameraDPhi;
            if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;
            PositionCamera();
        }

        private void OnMapRight(object? obj)
        {
            CameraTheta -= CameraDTheta;
            PositionCamera();
        }

        private void OnMapLeft(object? obj)
        {
            CameraTheta += CameraDTheta;
            PositionCamera();
        }

        private void OnZoomMapIn(object? obj)
        {
            CameraR -= CameraDR;
            if (CameraR < CameraDR) CameraR = CameraDR;
            PositionCamera();
        }

        private void OnMapZoomOut(object? obj)
        {
            CameraR += CameraDR;
            PositionCamera();
        }
        #endregion
    }
}