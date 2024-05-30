using ODSphereRouter.ViewModels;
using ODUtils.Helpers;
using UserControl = System.Windows.Controls.UserControl;

namespace ODSphereRouter.Controls
{
    /// <summary>
    /// Interaction logic for RouteMapListControl.xaml
    /// </summary>
    public partial class RouteMapListControl : UserControl
    {
        public RouteMapListControl()
        {
            InitializeComponent();
            Loaded += RouteMapListControl_Loaded;
        }

        private void RouteMapListControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is RouteMapViewModel viewModel)
            {
                viewModel.OnSeletedItemChanged += OnSelectedItemChanged;
                viewModel.OnSystemNameCopiedToClipboard += OnSystemNameCopiedToClipboard;
            }            
        }

        private void OnSelectedItemChanged(object? sender, RouteStopViewModel e)
        {
            RouteListGrid.SelectedItem = e;
            RouteListGrid.ScrollIntoView(e);
            RouteListGrid.Items.Refresh();
        }
        private void OnSystemNameCopiedToClipboard(object? sender, RouteStopViewModel e)
        {
            ClipboardText.Text = $"{e.Name} copied to clipboard";
            WPFUiEffects.FadeInOutElement(ClipboardText);
        }
    }
}
