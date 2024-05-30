using ODSphereRouter.Models;

namespace ODSphereRouter.ViewModels
{
    public sealed class SettingsStoreViewModel(Settings settings) : ViewModelBase
    {
        private bool exploitedControlledOnly = settings.ExploitedControlledOnly;
        private bool autoCopyToClipboard = settings.AutoCopyToClipboard;
        private string journalPath = settings.JournalPath;
        public bool ExploitedControlledOnly { get => exploitedControlledOnly; set { exploitedControlledOnly = value; OnPropertyChanged(); } }
        public bool AutoCopyToClipboard 
        { 
            get => autoCopyToClipboard; 
            set 
            { 
                autoCopyToClipboard = value; 
                OnPropertyChanged(); 
            } 
        }
        public string JournalPath { get => journalPath; set { journalPath = value; OnPropertyChanged(); } }
    }
}
