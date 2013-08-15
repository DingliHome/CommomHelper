using System;
using System.Collections.Generic;
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
using ClassLibraryHelper.C_.Print;

namespace UITest
{
    /// <summary>
    /// PrintView.xaml 的交互逻辑
    /// </summary>
    public partial class PrintView : Window
    {
        public PrintView()
        {
            InitializeComponent();

            var printDialogHelper = new PrintDialogHelper();
            printDialogHelper.PrintVisual(root);
        }
    }
}
