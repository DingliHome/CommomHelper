using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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

namespace UITest
{
    /// <summary>
    /// Interaction logic for ByteArrayInListBox.xaml
    /// </summary>
    public partial class ByteArrayInListBox : Window
    {
        private int _datas = 8;
        public ObservableCollection<CheckableItem> Collection { get; set; }
        public bool[] Weeks { get { return _datas.ToBoolenArray(7); } }

        public int Datas
        {
            get { return _datas; }
            set { _datas = value; }
        }

        public ByteArrayInListBox()
        {
            InitializeComponent();
            Collection = new ObservableCollection<CheckableItem>();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }

    public static class Extension
    {
        public static bool[] ToBoolenArray(this int integer, int length)
        {
            var charArray = Convert.ToString(integer, 2).ToCharArray();
            var arrayBoolen = new bool[length];

            for (int i = 0; i < charArray.Length; i++)
            {
                if (i <= arrayBoolen.Length)
                {
                    arrayBoolen[i] = charArray[i] == '1';
                }
            }
            return arrayBoolen;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in enumerable)
            {
                action.Invoke(e, i);
                i++;
            }
        }

        public static int ToInteger(this bool[] booleanArray)
        {
            var length = booleanArray.Length;
            var charArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                charArray[length - i - 1] = booleanArray[i] ? '1' : '0';
            }
            return Convert.ToInt32(new string(charArray), 2);
        }
    }
    public class BoolenToSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumerable = value as bool[];
            var checkableItems = new CheckableItem[enumerable.Count()];
            enumerable.ToList().ForEach<bool>((b, i) =>
                                            {
                                                checkableItems[i] = new CheckableItem(i, enumerable[i]);
                                            });

            return checkableItems;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }

    public class CheckableItem : INotifyPropertyChanged, ISelectable
    {
        private bool _isSelected;
        private int _data;

        public CheckableItem(int data, bool isSelected)
        {
            Data = data;
            IsSelected = isSelected;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); }
        }

        public int Data
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged("Data"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
