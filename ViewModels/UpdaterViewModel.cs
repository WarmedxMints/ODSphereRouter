using ODSphereRouter.DTOs;
using ODSphereRouter.Models;
using ODSphereRouter.Services.Database;
using ODUtils.Dialogs;
using ODUtils.IO;
using System.Windows;

namespace ODSphereRouter.ViewModels
{
    public sealed class UpdaterViewModel(ISphereRouterDatabaseProvider sphereRouterDatabaseProvider) : ViewModelBase
    {
        private string updateText = "Checking For Application Update...";

        public string UpdateText
        {
            get { return updateText; }
            set { updateText = value; OnPropertyChanged(); }
        }

        public ISphereRouterDatabaseProvider SphereRouterDatabaseProvider { get; } = sphereRouterDatabaseProvider;

        public async ValueTask CheckForUpdates(Window updateWindow)
        {
            //Give the window a moment to open
            await Task.Delay(500);

            var updateInfo = await Json.GetJsonFromUrlAndDeserialise<UpdateInfo>("https://raw.githubusercontent.com", "/WarmedxMints/ODUpdates/main/ODSphereRouteUpdate.json");

            if (updateInfo.Version > App.AppVersion)
            {
                var result = ODMessageBox.ShowUpdateWindow(updateWindow, "OD Sphere Router", $"Version {updateInfo.Version} is available.\nWould you like to go to the download page?", updateInfo.PatchNotes);

                if (result == MessageBoxResult.Yes)
                {
                    ODUtils.Helpers.OperatingSystem.OpenUrl(updateInfo.Url);
                }
            }

            //Update Systems DataBase
            UpdateText = "Checking For Systems Updates...";

            var systems = await Json.GetJsonFromUrlAndDeserialise<IEnumerable<StarSystemDTO>>("https://raw.githubusercontent.com", "/WarmedxMints/ODUpdates/main/SystemsDTO.Json");
            
            if (systems != null && systems.Any())
            {
                var ret = await SphereRouterDatabaseProvider.UpdateStarSystems(systems);

                if (ret > 1)
                {
                    UpdateText = $"Updated {ret:N0} Systems...";
                    await Task.Delay(2000);
                }
            }

            UpdateText = "Launching Main Application";
            await Task.Delay(500);
        }
    }
}
