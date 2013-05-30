using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClassLibraryHelper.C_.Images;

namespace UITest
{
    /// <summary>
    /// Interaction logic for ImageShow.xaml
    /// </summary>
    public partial class ImageShow : Window
    {
        public ImageShow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(ImageShow_Loaded);
        }

        void ImageShow_Loaded(object sender, RoutedEventArgs e)
        {
            var source = ImageHelper.FrameworkElementToBitmapSource(btn, "D:\\11.jpg");

            image.Source = source;
            image.Stretch=Stretch.Fill;
        }
    }
}
