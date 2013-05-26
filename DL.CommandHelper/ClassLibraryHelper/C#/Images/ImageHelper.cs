using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace ClassLibraryHelper.C_.Images
{
    public class ImageHelper
    {
        private ImageHelper _instance;
        public ImageHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    return _instance = new ImageHelper();
                }
                return _instance;
            }
        }

        public void ImageToBitmapSource(string path)
        {
            var uri = new Uri(path);
            var bitmapImage = new BitmapImage(uri);
            
        }
    }
}
