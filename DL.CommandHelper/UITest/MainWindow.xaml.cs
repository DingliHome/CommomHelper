using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibraryHelper.Win32;
using ClassLibraryHelper.WPF;
namespace UITest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public IList<Person> Persons { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Persons = new List<Person>();
            Persons.Add(new Person());
            Persons.Add(new Person());
            Persons.Add(new Person());
            Persons.Add(new Person());
            Persons.Add(new Person());
            this.DataContext = this;
        }

        private IList _checkedPerson;
        public IList CheckedPerson
        {
            get { return _checkedPerson; }
            set { _checkedPerson = value; }
        }
    }

    public class Person : NotifyPropertyChangeObject
    {
        private static int count = 0;
        public Person()
        {
            Name = "dingli" + count;
            count++;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            //set { _name = value; }
            set
            {
                ApplyPropertyChange<Person,
                    string>(ref _name, o => o.Name, value);
            }

        }
    }
}
