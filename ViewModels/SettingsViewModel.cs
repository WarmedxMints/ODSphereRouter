using ODSphereRouter.Stores;
using ODUtils.Commands;
using System.Windows.Input;

namespace ODSphereRouter.ViewModels
{
    public sealed class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsStore settingsStore;
        private readonly JournalWatcherStore watcherStore;

        public SettingsStoreViewModel ViewModel => settingsStore.ViewModel;

        public ICommand SetNewJournalDir { get; }
        public ICommand OpenPayPal { get; }

        public SettingsViewModel(SettingsStore settingsStore, JournalWatcherStore watcherStore)
        {
            this.settingsStore = settingsStore;
            this.watcherStore = watcherStore;

            SetNewJournalDir = new RelayCommand(OnSetNewDir);
            OpenPayPal = new RelayCommand(OnOpenPayPal);
        }



        private void OnOpenPayPal(object? obj)
        {
            ODUtils.Helpers.OperatingSystem.OpenUrl("https://www.paypal.com/donate/?business=UPEJS3PN7H4XJ&no_recurring=0&item_name=Creator+of+OD+Software.+Thank+you+for+your+donation.&currency_code=GBP");
        }

        private void OnSetNewDir(object? obj)
        {
            var folderDialog = new FolderBrowserDialog
            {
                InitialDirectory = ViewModel.JournalPath
            };

            var result = folderDialog.ShowDialog();

            if (result == DialogResult.OK) 
            {
                ViewModel.JournalPath = folderDialog.SelectedPath;
                watcherStore.RestartWatcher();
            }
        }
    }
}
