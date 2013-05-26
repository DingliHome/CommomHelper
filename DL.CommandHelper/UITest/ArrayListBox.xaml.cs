using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UITest
{
    /// <summary>
    /// Interaction logic for ArrayListBox.xaml
    /// </summary>
    public partial class ArrayListBox : UserControl
    {
        public ArrayListBox()
        {
            InitializeComponent();
        }

        public int ArrayInt
        {
            get { return (int)GetValue(ArrayIntProperty); }
            set { SetValue(ArrayIntProperty, value); }
        }

        public static readonly DependencyProperty ArrayIntProperty =
            DependencyProperty.Register("ArrayInt", typeof(int), typeof(ArrayListBox), new UIPropertyMetadata(127));



        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ArrayListBox), new UIPropertyMetadata(null));



        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ArrayListBox), new UIPropertyMetadata(null));

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemsSource == null)
                return;
            var boolArray = ItemsSource.OfType<ISelectable>().Select(x => x.IsSelected).ToArray();
            var integer = boolArray.ToInteger();
            if (integer != ArrayInt)
                ArrayInt = integer;
        }



    }
}
