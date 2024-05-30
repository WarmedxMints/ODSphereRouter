using ODSphereRouter.Stores;
using ODUtils.Commands;
using System.Windows.Input;

namespace ODSphereRouter.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        private readonly NavigationViewModel navigationViewModel;
        private readonly JournalWatcherStore journalWatcherStore;
        private readonly SettingsStore settingsStore;

        public string Title { get; }
        public SettingsStore SettingsStore { get => settingsStore; }
        public string? CurrentSystem => journalWatcherStore.CurrentSystem;
        public string? CurrentCommander => journalWatcherStore.CurrentCommander;
        public ViewModelBase? CurrentViewModel => navigationStore.CurrentViewModel;

        public ICommand NavigateToSphereList => navigationViewModel.RouteListCommand;
        public ICommand NavigateToRouteMap => navigationViewModel.RouteMapCommand;
        public ICommand NavigateToSettingView => navigationViewModel.SettingViewCommand;
        public ICommand ResetWindowPositionCommand { get; }

        public MainViewModel(NavigationStore navigationStore, NavigationViewModel navigationViewModel, JournalWatcherStore journalWatcherStore, SettingsStore settingsStore)
        {
            this.navigationStore = navigationStore;
            this.navigationViewModel = navigationViewModel;
            this.journalWatcherStore = journalWatcherStore;
            this.settingsStore = settingsStore;

            ResetWindowPositionCommand = new RelayCommand(OnResetWindowPos);
            navigationStore.CurrentViewModelChanged += NavigationStore_CurrentViewModelChanged;

            journalWatcherStore.OnCurrentSystemChanged += OnCurrentSystemChanged;
            journalWatcherStore.OnCurrentCommanderChanged += OnCurrntCommanderChanged;

            Title = $"OD Sphere Router v{App.AppVersion}";
        }

        private void OnResetWindowPos(object? obj)
        {
            SettingsStore.ResetWindowPosition();
        }

        private void OnCurrntCommanderChanged(object? sender, string? e)
        {
            OnPropertyChanged(nameof(CurrentCommander));
        }

        private void OnCurrentSystemChanged(object? sender, string? e)
        {
            OnPropertyChanged(nameof(CurrentSystem));
        }

        private void NavigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}