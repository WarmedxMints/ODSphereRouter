using ODSphereRouter.Models;
using System.Windows;

namespace ODSphereRouter.ViewModels
{
    public sealed class WindowPositionViewModel : ViewModelBase
    {
        private double top;
        private double left;
        private double height;
        private double width;
        private WindowState state;

        public double Top { get => top; set { top = value; OnPropertyChanged(); } }
        public double Left { get => left; set { left = value; OnPropertyChanged(); } }
        public double Height { get => height; set { height = value; OnPropertyChanged(); } }
        public double Width { get => width; set { width = value; OnPropertyChanged(); } }
        public WindowState State { get => state; set { state = value; OnPropertyChanged(); } }

        public static WindowPositionViewModel GetModelFromSettings(Settings settings)
        {
            if(settings.Position is null)
            {
                return new();
            }

            return new()
            {
                Top = settings.Position.Top,
                Left = settings.Position.Left,
                Height = settings.Position.Height,
                Width = settings.Position.Width,
                State = settings.Position.State,
            };
        }
    }
}
