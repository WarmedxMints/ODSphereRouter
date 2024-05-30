using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ODSphereRouter.DbContexts;
using ODSphereRouter.HostBuilders;
using ODSphereRouter.Services;
using ODSphereRouter.Services.Database;
using ODSphereRouter.Stores;
using ODSphereRouter.ViewModels;
using ODSphereRouter.Windows;
using System.Windows;
using Application = System.Windows.Application;

namespace ODSphereRouter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly Version AppVersion = new(1,0);
#if INSTALL
        public readonly static string BaseDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ODSphereRouter");
#else
        public readonly static string BaseDirectory =  AppDomain.CurrentDomain.BaseDirectory;
#endif
        private const string database = "SphereRouter.db";
        private readonly string connectionString = $"DataSource={System.IO.Path.Combine(BaseDirectory, database)}";

        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .AddViewModels()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ISphereRouterDbContextFactory>(new SphereRouterDbContextFactory(connectionString));
                    services.AddSingleton<ISphereRouterDatabaseProvider, SphereRouterDatabaseProvider>();

                    services.AddSingleton<NavigationStore>();
                    services.AddSingleton<SphereDataStore>();
                    services.AddSingleton<JournalWatcherStore>();
                    services.AddSingleton<SettingsStore>();

                    services.AddSingleton(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //Global window style does not apply by default as TargetType in styles doesn't manage derived types
            //This gets around that
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
            {
                DefaultValue = Current.FindResource(typeof(Window))
            });

            if (System.IO.Directory.Exists(BaseDirectory) == false)
            {
                System.IO.Directory.CreateDirectory(BaseDirectory);
            }

            _host.Start();

            var factory = _host.Services.GetRequiredService<ISphereRouterDbContextFactory>();
            using (var dbContext = factory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }

#if !DEBUG
            //Disable shutdown when the dialog closes
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var updater = new UpdateWindow(new UpdaterViewModel(_host.Services.GetRequiredService<ISphereRouterDatabaseProvider>()));
            updater.ShowDialog();
#endif
            var watcher = _host.Services.GetRequiredService<JournalWatcherStore>();
            watcher.Initialse();

            var navigationService = _host.Services.GetRequiredService<NavigationService<RoutePlannerViewModel>>();
            navigationService.Navigate();

            ShutdownMode = ShutdownMode.OnMainWindowClose;

            MainWindow = _host.Services.GetRequiredService<MainWindow>();          
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var settings = _host.Services.GetRequiredService<SettingsStore>();
            settings.SaveSettings();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
