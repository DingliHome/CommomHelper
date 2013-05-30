using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClassLibraryHelper.C_.Images
{
    public class ImageHelper
    {
        /// <summary>
        /// Visual to bitmapSource
        /// </summary>
        /// <param name="element"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static BitmapSource FrameworkElementToBitmapSource(FrameworkElement element, int width, int height, string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                var renderTargetBitmap = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(element);
               
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                encoder.Save(fileStream);
               
                return renderTargetBitmap;
            }
        }

        public static BitmapSource FrameworkElementToBitmapSource(FrameworkElement element,
                                                                  string path)
        {
            var width =(int)( double.IsNaN(element.Width) ? element.ActualWidth:
            element.Width);
            var heigth=(int)( double.IsNaN(element.Height) ? element.ActualHeight:
            element.Height);
            var bitmapSource = FrameworkElementToBitmapSource(element, width, heigth, path);
            return bitmapSource;
        }



    }
}
