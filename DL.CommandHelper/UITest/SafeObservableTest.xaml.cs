using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClassLibraryHelper.Collection;

namespace UITest
{
    /// <summary>
    /// Interaction logic for SafeObservableTest.xaml
    /// </summary>
    public partial class SafeObservableTest : Window
    {
        public SafeObservableTest()
        {
            InitializeComponent();
        }

        private Random rand = new Random(DateTime.Now.Millisecond);
        private SafeObservable<TestData> data=new SafeObservable<TestData>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            list.DataContext = data;
            List<Action> work=new List<Action>();
            for (int i = 0; i < 100; i++)
            {
                work.Add(new Action(DoWorkAdd));
                work.Add(new Action(DoWorkClear));
                work.Add(new Action(DoWorkRemove));
                work.Add(new Action(DoWorkRemoveAt));
                work.Add(new Action(DoWorkInsert));
                work.Add(new Action(DoWorkReplace));
            }
            for (int i = 0; i < 1000; i++)
                work[rand.Next(0, work.Count)].BeginInvoke(null, null);
        }

        void DoWorkAdd()
        {
            Thread.Sleep(rand.Next(500, 30000));
            data.Add(new TestData() { Text = string.Format("Thread {0} Added", Thread.CurrentThread.ManagedThreadId) });
        }

        void DoWorkClear()
        {
            Thread.Sleep(rand.Next(500, 10000));
            data.Clear();
            Debug.WriteLine((string.Format("Thread {0} Clear", Thread.CurrentThread.ManagedThreadId)));
        }

        void DoWorkRemove()
        {
            Thread.Sleep(rand.Next(500, 10000));
            if (data.Count == 0)
                return;
            var item = data[0];
            data.Remove(item);
            Debug.WriteLine((string.Format("Thread {0} Remove", Thread.CurrentThread.ManagedThreadId)));
        }

        void DoWorkRemoveAt()
        {
            Thread.Sleep(rand.Next(500, 10000));
            if (data.Count == 0)
                return;
            data.RemoveAt(0);
            Debug.WriteLine((string.Format("Thread {0} RemoveAt", Thread.CurrentThread.ManagedThreadId)));
        }

        void DoWorkInsert()
        {
            Thread.Sleep(rand.Next(500, 10000));
            data.Insert(rand.Next(0, data.Count), new TestData() { Text = string.Format("Thread {0} Insert", Thread.CurrentThread.ManagedThreadId) });
        }

        void DoWorkReplace()
        {
            Thread.Sleep(rand.Next(500, 10000));
            data[rand.Next(0, data.Count)] = new TestData() { Text = string.Format("Thread {0} Replace", Thread.CurrentThread.ManagedThreadId) };
        }

        public class TestData
        {
            public string Text { get; set; }

        }
    }
}
