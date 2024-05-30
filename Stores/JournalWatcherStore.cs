using EliteJournalReader;
using EliteJournalReader.Events;
using ODSphereRouter.DTOs;
using ODSphereRouter.Models;
using ODSphereRouter.Services.Database;

namespace ODSphereRouter.Stores
{
    public sealed class JournalWatcherStore(ISphereRouterDatabaseProvider sphereRouterDatabaseProvider, SettingsStore settingsStore)
    {
        private readonly ISphereRouterDatabaseProvider sphereRouterDatabaseProvider = sphereRouterDatabaseProvider;
        private readonly SettingsStore settingsStore = settingsStore;
        private JournalWatcher? watcher;
        private StarSystemDTO? systemDTO;

        public StarSystemDTO? StarSystemDTO => systemDTO;

        private string? currentSystem = "Unknown";
        public string? CurrentSystem 
        { 
            get => currentSystem; 
            private set 
            { 
                currentSystem = value;
                OnCurrentSystemChanged?.Invoke(this, currentSystem);
            } 
        }

        private string? currentCommander = "No commander found";
        public string? CurrentCommander
        {
            get => currentCommander;
            private set
            {
                currentCommander = value;
                OnCurrentCommanderChanged?.Invoke(this, currentCommander);
            }

        }
        public EventHandler<string?>? OnCurrentSystemChanged;
        public EventHandler<string?>? OnCurrentCommanderChanged;

        public void Initialse()
        {
            watcher = new(settingsStore.ViewModel.JournalPath);

            watcher.GetEvent<CommanderEvent>()?.AddHandler(OnCommaderEvent);
            watcher.GetEvent<LocationEvent>()?.AddHandler(OnLocationEvent);
            watcher.GetEvent<FSDJumpEvent>()?.AddHandler(OnFSDJump);
            _ = watcher.StartWatching();
        }

        public void RestartWatcher()
        {
            watcher?.StopWatching();
            watcher?.Dispose();
            CurrentCommander = "No commander found";
            CurrentSystem = "Unknown";
            Initialse();
        }

        private void OnCommaderEvent(object? sender, CommanderEvent.CommanderEventArgs e)
        {
            CurrentCommander = e.Name;
        }
        private void OnLocationEvent(object? sender, LocationEvent.LocationEventArgs e)
        {
            systemDTO = new()
            {
                Address = e.SystemAddress,
                X = e.StarPos.X,
                Y = e.StarPos.Y,
                Z = e.StarPos.Z,
                Name = e.StarSystem,
                Population = e.Population,
                Powers = (int)Enums.StringToPowers(e.Powers),
                PowerState = (int)e.PowerplayState
            };

            sphereRouterDatabaseProvider.UpdateSystemFromLogs(systemDTO);

            CurrentSystem = e.StarSystem;
        }

        private void OnFSDJump(object? sender, FSDJumpEvent.FSDJumpEventArgs e)
        {
            systemDTO = new()
            {
                Address = e.SystemAddress,
                X = e.StarPos.X,
                Y = e.StarPos.Y,
                Z = e.StarPos.Z,
                Name = e.StarSystem,
                Population = e.Population,
                Powers = (int)Enums.StringToPowers(e.Powers),
                PowerState = (int)e.PowerplayState
            };

            sphereRouterDatabaseProvider.UpdateSystemFromLogs(systemDTO);

            CurrentSystem = e.StarSystem;
        }
    }
}
