using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ODSphereRouter.Services;
using ODSphereRouter.Stores;
using ODSphereRouter.ViewModels;

namespace ODSphereRouter.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient((s) => CreateRoutePlannerViewModel(s));
                services.AddTransient<RouteMapViewModel>();
                services.AddTransient<SettingsViewModel>();

                AddViewModelNavigation<RoutePlannerViewModel>(services);
                AddViewModelNavigation<RouteMapViewModel>(services);
                AddViewModelNavigation<SettingsViewModel>(services);

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<NavigationViewModel>();
            });

            return hostBuilder;
        }

        private static void AddViewModelNavigation<TViewModel>(IServiceCollection services) where TViewModel : ViewModelBase
        {
            services.AddSingleton<Func<TViewModel>>((s) => () => s.GetRequiredService<TViewModel>());
            services.AddSingleton<NavigationService<TViewModel>>();
        }

        private static RoutePlannerViewModel CreateRoutePlannerViewModel(IServiceProvider services)
        {
            return RoutePlannerViewModel.LoadViewModel(services.GetRequiredService<SphereDataStore>(), services.GetRequiredService<NavigationViewModel>(), services.GetRequiredService<JournalWatcherStore>(), services.GetRequiredService<SettingsStore>());
        }
    }
}
