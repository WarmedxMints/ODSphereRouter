using UserControl = System.Windows.Controls.UserControl;

namespace ODSphereRouter.Views
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            ODUtils.Helpers.OperatingSystem.OpenUrl(e.Uri.AbsoluteUri);
        }
    }
}
