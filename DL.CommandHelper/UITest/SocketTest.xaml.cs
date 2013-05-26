using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
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
using ClassLibraryHelper.Sockets;

namespace UITest
{
    /// <summary>
    /// Interaction logic for SocketTest.xaml
    /// </summary>
    public partial class SocketTest : Window, INotifyPropertyChanged
    {
        public SocketTest()
        {
            InitializeComponent();
            button2.IsEnabled = false;
            ClientSockets = new ObservableCollection<ClientSockets>();

            this.DataContext = this;
        }

        public ObservableCollection<ClientSockets> ClientSockets
        {
            get { return _clientSockets; }
            set { _clientSockets = value; OnPropertyChanged("ClientSockets"); }
        }

        public ClientSockets SelectedSocket
        {
            get { return _selectedSocket; }
            set { _selectedSocket = value; OnPropertyChanged("SelectedSocket"); }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            port = int.Parse(textBox1.Text.Trim());

            ThreadPool.QueueUserWorkItem(state =>
                                             {

                                                 _connectServer = SocketHelper.SocketServer(port);
                                                 var bytes = new byte[1024];
                                                 button1.IsEnabled = false;
                                                 button2.IsEnabled = true;

                                                 while (true)
                                                 {
                                                     var oldbytes = bytes;
                                                     SocketHelper.ReceiveData(_connectServer, bytes, 1);
                                                     if (bytes != oldbytes && SelectedSocket != null && SelectedSocket.IsConnected)
                                                     {
                                                         SelectedSocket.SendData = bytes.ToString();
                                                         SelectedSocket.Send();
                                                     }
                                                     if (button1.IsEnabled)
                                                         break;
                                                 }
                                             });

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            button1.IsEnabled = true;
            button2.IsEnabled = false;
        }

        private int count = 1;
        private ClientSockets _selectedSocket;
        private ObservableCollection<ClientSockets> _clientSockets;
        private Socket _connectServer;
        private int port;

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var clientSockets = new ClientSockets() { Name = "client" + count, IP = "192.168.1." + count };
            ClientSockets.Add(clientSockets);
            count++;
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clientSockets = e.AddedItems[0] as ClientSockets;
            if (clientSockets != null)
            {
                SelectedSocket = clientSockets;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            SelectedSocket.IsConnected = true;
            SelectedSocket.ClientSocket = SocketHelper.ConnectServer(SelectedSocket.IP, port,new AsyncCallback(CallBack));
        }
        private void CallBack(IAsyncResult result)
        {
            var socket = (Socket) result.AsyncState;
            socket.EndConnect(result);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            SelectedSocket.IsConnected = false;
            SelectedSocket.ClientSocket.Close();
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            var str = textBox2.Text.Trim();
            SocketHelper.SendData(SelectedSocket.ClientSocket, str, 1);
        }
    }

    public class ClientSockets : INotifyPropertyChanged
    {
        private IList<string> _receiveDatas;
        private string _sendData;
        private bool _isConnected;
        public string Name { get; set; }
        public string IP { get; set; }
        public Socket ClientSocket { get; set; }
        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; OnPropertyChanged("IsConnected"); }
        }

        public IList<string> ReceiveDatas
        {
            get { return _receiveDatas; }
            set { _receiveDatas = value; OnPropertyChanged("ReceiveDatas"); }
        }

        public string SendData
        {
            get { return _sendData; }
            set { _sendData = value; OnPropertyChanged("SendData"); }
        }

        public void Send()
        {
            if (ReceiveDatas != null)
                ReceiveDatas.Add(SendData);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
