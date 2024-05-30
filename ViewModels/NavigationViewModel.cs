using ODSphereRouter.Commands;
using ODSphereRouter.Services;
using System.Windows.Input;

namespace ODSphereRouter.ViewModels
{
    public sealed class NavigationViewModel(NavigationService<RoutePlannerViewModel> routeListCommand,
                                                NavigationService<RouteMapViewModel> routeMapCommand,
                                                NavigationService<SettingsViewModel> settingViewCommand)
    {
        public ICommand RouteListCommand { get; } = new NavigateCommand<RoutePlannerViewModel>(routeListCommand);
        public ICommand RouteMapCommand { get; } = new NavigateCommand<RouteMapViewModel>(routeMapCommand);
        public ICommand SettingViewCommand { get; } = new NavigateCommand<SettingsViewModel>(settingViewCommand);
    }
}