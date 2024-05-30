using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace ODSphereRouter.Controls
{
    /// <summary>
    /// Interaction logic for ImageWithText.xaml
    /// </summary>
    public partial class ImageWithText : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ImageWithText), new PropertyMetadata(string.Empty));


        public string ImageURI
        {
            get { return (string)GetValue(ImageURIProperty); }
            set { SetValue(ImageURIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageURI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageURIProperty =
            DependencyProperty.Register("ImageURI", typeof(string), typeof(ImageWithText), new PropertyMetadata(string.Empty));


        public ImageWithText()
        {
            InitializeComponent();
        }
    }
}
