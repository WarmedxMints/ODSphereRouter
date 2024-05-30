using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;

namespace ODSphereRouter.Controls
{
    /// <summary>
    /// Interaction logic for SphereListControl.xaml
    /// </summary>
    public partial class SphereListControl : UserControl
    {
        public SphereListControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.BindingGroup.Owner is DataGridRow row)
            {
                var visible = row.DetailsVisibility == Visibility.Visible;

                row.DetailsVisibility = visible ? Visibility.Collapsed : Visibility.Visible;
                btn.Content = visible ? "\xE96E" : "\xE96D";
            }
        }

        private void MainGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            GridScrollView.ScrollToVerticalOffset(GridScrollView.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void MainGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                grid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
            }
        }

        private void CustomListBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingOperations.GetBindingExpression(CustomListBox, System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        }
    }
}
