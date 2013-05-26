using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClassLibraryHelper.C_.Images
{
    public class ImageHelper
    {
        public static void ImageToBitmapSource(string path)
        {
            var uri = new Uri(path);
            var bitmapImage = new BitmapImage(uri);
        }

        /// <summary>
        /// Visual to bitmapSource
        /// </summary>
        /// <param name="element"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static BitmapSource FrameworkElementToBitmapSource(FrameworkElement element,int width,int height)
        {
            var renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(element);

            return renderTargetBitmap;
        }

        public static BitmapSource ElementScreenShot(FrameworkElement element)
        {
            var size = new Size(element.ActualWidth, element.ActualHeight);//如果控件为显示在ui上，可以通过 measure和Arrange来呈现
            element.Measure(size);
            element.Arrange(new Rect(new Point(0,0),size));
            return FrameworkElementToBitmapSource(element, (int) element.ActualWidth, (int) element.ActualHeight);
        }
    }
}
