using ODSphereRouter.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;
using UserControl = System.Windows.Controls.UserControl;

namespace ODSphereRouter.Controls
{
    /// <summary>
    /// Interaction logic for RouteListControl.xaml
    /// </summary>
    public partial class RouteListControl : UserControl
    {
        private RoutePlannerViewModel? routePlanner;
        public RouteListControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is RoutePlannerViewModel model)
            {
                routePlanner = model;
                model.MessageLog.CollectionChanged += MessageLog_CollectionChanged;
            }
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (routePlanner is not null)
            {
                routePlanner.MessageLog.CollectionChanged -= MessageLog_CollectionChanged;
            }
        }

        private void MessageLog_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (VisualTreeHelper.GetChildrenCount(Log) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(Log, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
        }
    }
}
