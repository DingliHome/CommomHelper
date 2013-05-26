using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClassLibraryHelper.WPF;

namespace UITest
{
    /// <summary>
    /// Interaction logic for TrayIcon.xaml
    /// </summary>
    public partial class TrayIconView : Window
    {
        public TrayIconView()
        {
            InitializeComponent();

            var trayIcon = TrayIcon.CreateTrayIcon();
            var findResource =FindResource("TrayIconMenu") as ContextMenu;
            trayIcon.ContextMenu = findResource;
            trayIcon.Icon = LoadDefaultTrayIcon();
        }

        private BitmapImage LoadDefaultTrayIcon()
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("/Images/Bulb.ico", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            return bi;
        }
    }
}
