using ODSphereRouter.ViewModels;
using System.Windows;

namespace ODSphereRouter.Windows
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdaterViewModel UpdaterView { get; }
        public UpdateWindow(UpdaterViewModel updaterView)
        {
            this.UpdaterView = updaterView;
            DataContext = updaterView;
            InitializeComponent();
            Loaded += UpdateWindow_Loaded;
        }

        private async void UpdateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdaterView.CheckForUpdates(this);

            DialogResult = true;
        }
    }
}
