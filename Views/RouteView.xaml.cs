using UserControl = System.Windows.Controls.UserControl;

namespace ODSphereRouter.Views
{
    /// <summary>
    /// Interaction logic for RouteView.xaml
    /// </summary>
    public partial class RouteView : UserControl
    {
        public RouteView()
        {
            InitializeComponent();         
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Focus();
        }
    }
}
