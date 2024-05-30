using System.Windows;

namespace ODSphereRouter.Models
{
    public sealed class WindowPosition(double top, double left, double height, double width, WindowState state)
    {
        public double Top { get; } = top;
        public double Left { get; } = left;
        public double Height { get; } = height;
        public double Width { get; } = width;
        public WindowState State { get; } = state;
    }
}
