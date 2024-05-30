using ODSphereRouter.Models;
using ODSphereRouter.ViewModels;
using ODUtils.EliteDangerousHelpers;
using ODUtils.IO;
using System.IO;
using System.Windows;

namespace ODSphereRouter.Stores
{
    public sealed class SettingsStore
    {
        private const string saveFile = "Settings.json";
        private Settings settings;
        private readonly SettingsStoreViewModel viewModel;

        public SettingsStoreViewModel ViewModel { get => viewModel; }

        public WindowPositionViewModel WindowPosition { get; }

        public SettingsStore ()
        {
            WindowPosition = new();
            settings = GetSettings();
            viewModel = new(settings);
            SetWindowPos();
        }

        public bool SaveSettings()
        {
            settings = new(viewModel.ExploitedControlledOnly, viewModel.AutoCopyToClipboard, viewModel.JournalPath, new(WindowPosition.Top, WindowPosition.Left, WindowPosition.Height, WindowPosition.Width, WindowPosition.State));
            return Json.SaveJson(settings, Path.Combine(App.BaseDirectory, saveFile));
        }

        private void SetWindowPos()
        {
            if(settings.Position != null) 
            {
                WindowPosition.State = settings.Position.State;
                WindowPosition.Top = settings.Position.Top;
                WindowPosition.Left = settings.Position.Left;
                WindowPosition.Width = settings.Position.Width;
                WindowPosition.Height = settings.Position.Height;
                return;
            }
            ResetWindowPositionActual(WindowPosition);
        }

        private static Settings GetSettings()
        {
            var saveFile = Path.Combine(App.BaseDirectory, SettingsStore.saveFile);

            if (File.Exists(saveFile))
            {
                var settings = Json.LoadJson<Settings>(saveFile);

                if (settings != null)
                {
                   return settings;
                }   
            }

            var journalPath = JournalPath.GetJournalPath();
            var windowPos = CreateDefaultPos();

            return new Settings(journalPath: journalPath, position: windowPos);
        }

        private static WindowPosition CreateDefaultPos()
        {
            var model = new WindowPositionViewModel();
            ResetWindowPositionActual(model);
            return new WindowPosition(model.Top,model.Left, model.Height, model.Width, model.State);
        }

        public void ResetWindowPosition()
        {
            ResetWindowPositionActual(WindowPosition);
        }

        private static void ResetWindowPositionActual(WindowPositionViewModel windowPosition)
        {
            double windowWidth = 1450;
            double windowHeight = 800;

            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
         
            var left = (screenWidth / 2) - (windowWidth / 2);
            var top = (screenHeight / 2) - (windowHeight / 2);

            if (windowHeight > SystemParameters.VirtualScreenHeight)
            {
                windowHeight = SystemParameters.VirtualScreenHeight;
            }

            if (windowWidth > SystemParameters.VirtualScreenWidth)
            {
                windowWidth = SystemParameters.VirtualScreenWidth;
            }

            windowPosition.Top = top;
            windowPosition.Left = left;
            windowPosition.Width = windowWidth;
            windowPosition.Height = windowHeight;
            windowPosition.State = WindowState.Normal;
        }
    }
}
