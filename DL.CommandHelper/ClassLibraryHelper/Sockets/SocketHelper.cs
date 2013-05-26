using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClassLibraryHelper.Sockets
{
    public class SocketHelper
    {
        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Socket SocketServer(int port)
        {
            Socket socket;
            try
            {
                var ipAddress = IPAddress.Any;
                var ipEndPoint = new IPEndPoint(ipAddress, port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(ipEndPoint);
                socket.Listen(10);
                socket.Accept();
            }
            catch (Exception e)
            {
                throw e;
            }
            return socket;
        }


        /// <summary>
        /// 使用TCP协议
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Socket ConnectServer(string ip, int port, AsyncCallback asyncResult)
        {
            Socket socket;
            try
            {
                var ipAddress = IPAddress.Parse(ip);
                var ipEndPoint = new IPEndPoint(ipAddress, port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.BeginConnect(ipEndPoint, asyncResult, socket);
                //if (!socket.Connected)
                //    socket = null;
            }
            catch (Exception e)
            {
                throw e;
            }
            return socket;
        }

        /// <summary>
        /// 根据主机名称连接TCP协议
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Socket ConnectServerByHostName(string hostName, int port)
        {
            Socket socket = null;
            try
            {
                var ipHostEntry = Dns.GetHostEntry(hostName);
                foreach (var ip in ipHostEntry.AddressList)
                {
                    var ipEndPoint = new IPEndPoint(ip, port);
                    var s = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    s.Connect(ipEndPoint);
                    if (s.Connected)
                    {
                        socket = s;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return socket;
        }

        /// <summary>
        /// 向远程主机发送数据
        /// </summary>
        /// <param name="socket">远程socket</param>
        /// <param name="bytes">待发送的数据</param>
        /// <param name="timeout">超时的时间，单位 秒</param>
        /// <returns>0：发送成功，-1：超时，-2发送数据出现错误，-3发送数据出现异常</returns>
        /// <remarks >
        /// 当 outTime 指定为-1时，将一直等待直到有数据需要发送
        /// </remarks>
        public static int SendData(Socket socket, byte[] bytes, int timeout)
        {
            if (socket == null || !socket.Connected)
                throw new ArgumentException("socket can not null and can not disconnected");
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("bytes can not null");

            int flag;
            try
            {
                var left = bytes.Length;
                var sendLen = 0;
                while (true)
                {
                    if (socket.Poll(timeout * 1000, SelectMode.SelectWrite))
                    {
                        sendLen = socket.Send(bytes, sendLen, left, SocketFlags.None);
                        left -= sendLen;
                        if (left == 0) // 数据已经全部发送
                        {
                            flag = 0;
                            break;
                        }
                        if (sendLen > 0) //数据部分已经被发送
                            continue;
                        flag = -2; //发送数据发生错误
                        break;
                    }
                    //超时退出
                    flag = -1;
                    break;
                }
            }
            catch (SocketException e)
            {
                flag = -3;
            }
            return flag;
        }

        public static int SendData(Socket socket, string buffer, int timeOut)
        {
            if (buffer == null || buffer.Length == 0)
                throw new ArgumentException("buffer can not null");
            return SendData(socket, Encoding.Default.GetBytes(buffer), timeOut);
        }

        /// <summary>
        /// 接受远程主机发送的数据
        /// </summary>
        /// <param name="socket">要接收数据且已经连接到远程主机的 socket</param>
        /// <param name="bytes">接收数据的缓冲区</param>
        /// <param name="timeOut">0:接收数据成功；-1:超时；-2:接收数据出现错误；-3:接收数据时出现异常</param>
        /// <returns></returns>
        ///  /// <remarks >
        /// 1、当 outTime 指定为-1时，将一直等待直到有数据需要接收；
        /// 2、需要接收的数据的长度，由 buffer 的长度决定。
        /// </remarks>
        public static int ReceiveData(Socket socket, byte[] bytes, int timeOut)
        {
            if (socket == null || !socket.Connected)
                throw new ArgumentException("socket can not null and can not disconnected");
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("bytes can not null");

            bytes.Initialize();
            var length = bytes.Length;
            var currentReceive = 0;
            int flag;

            try
            {
                while (true)
                {
                    if (socket.Poll(timeOut * 1000, SelectMode.SelectRead))
                    {
                        currentReceive = socket.Receive(bytes, currentReceive, length, SocketFlags.None);
                        length -= currentReceive;
                        if (length == 0)
                        {
                            flag = 0;
                            break;
                        }
                        if (currentReceive > 0)
                            continue;
                        flag = -2;
                        break;
                    }
                    flag = -1;
                    break;
                }
            }
            catch (Exception e)
            {
                flag = -3;
            }
            return flag;
        }

        public static int ReceiveData(Socket socket, string buffer, int bufferLen, int outTime)
        {
            if (buffer == null || bufferLen <= 0)
            {
                throw new ArgumentException("存储待接收数据的缓冲区长度必须大于0");
            }
            var tmp = new byte[bufferLen];
            int flag;
            if ((flag = ReceiveData(socket, tmp, outTime)) == 0)
            {
                buffer = Encoding.Default.GetString(tmp);
            }
            return flag;
        }

        /// <summary>
        /// 向远程主机发送文件
        /// </summary>
        /// <param name="socket">要发送数据且已经连接到远程主机的 socket</param>
        /// <param name="fileName">待发送的文件名称</param>
        /// <param name="maxBufferLength">文件发送时的缓冲区大小</param>
        /// <param name="timeOut"></param>
        /// <returns>0:发送文件成功；-1:超时；-2:发送文件出现错误；-3:发送文件出现异常；-4:读取待发送文件发生错误</returns>
        public static int SendFile(Socket socket, string fileName, int maxBufferLength, int timeOut)
        {
            if (fileName == null || maxBufferLength <= 0)
            {
                throw new ArgumentException("待发送的文件名称为空或发送缓冲区的大小设置不正确.");
            }

            var flag = 0;
            try
            {
                var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                var fileLen = fs.Length;                        // 文件长度
                var leftLen = fileLen;                            // 未读取部分
                int readLen;                                // 已读取部分
                byte[] buffer;

                if (fileLen <= maxBufferLength)
                {            /* 文件可以一次读取*/
                    buffer = new byte[fileLen];
                    readLen = fs.Read(buffer, 0, (int)fileLen);
                    flag = SendData(socket, buffer, timeOut);
                }
                else
                {                                    /* 循环读取文件,并发送 */
                    buffer = new byte[maxBufferLength];
                    while (leftLen != 0)
                    {
                        readLen = fs.Read(buffer, 0, maxBufferLength);
                        if ((flag = SendData(socket, buffer, timeOut)) < 0)
                        {
                            break;
                        }
                        leftLen -= readLen;
                    }
                }
                fs.Close();
            }
            catch (IOException e)
            {
                flag = -4;
            }
            return flag;
        }

        public static int SendFile(Socket socket, string fileName)
        {
            return SendFile(socket, fileName, 2048, 1);
        }

        /// <summary>
        /// 接收远程主机发送的文件
        /// </summary>
        /// <param name="socket">待接收数据且已经连接到远程主机的 socket</param>
        /// <param name="fileName">保存接收到的数据的文件名</param>
        /// <param name="fileLength" >待接收的文件的长度</param>
        /// <param name="maxBufferLength">接收文件时最大的缓冲区大小</param>
        /// <param name="outTime">接受缓冲区数据的超时时间</param>
        /// <returns>0:接收文件成功；-1:超时；-2:接收文件出现错误；-3:接收文件出现异常；-4:写入接收文件发生错误</returns>
        /// <remarks >
        /// 当 outTime 指定为-1时，将一直等待直到有数据需要接收
        /// </remarks>
        public static int ReceiveFile(Socket socket, string fileName, long fileLength, int maxBufferLength, int outTime)
        {
            if (fileName == null || maxBufferLength <= 0)
            {
                throw new ArgumentException("保存接收数据的文件名称为空或发送缓冲区的大小设置不正确.");
            }

            int flag = 0;
            try
            {
                var fs = new FileStream(fileName, FileMode.Create);
                byte[] buffer;

                if (fileLength <= maxBufferLength)
                {                /* 一次读取所传送的文件 */
                    buffer = new byte[fileLength];
                    if ((flag = ReceiveData(socket, buffer, outTime)) == 0)
                    {
                        fs.Write(buffer, 0, (int)fileLength);
                    }
                }
                else
                {                                        /* 循环读取网络数据，并写入文件 */
                    int rcvLen = maxBufferLength;
                    long leftLen = fileLength;                        //剩下未写入的数据
                    buffer = new byte[rcvLen];

                    while (leftLen != 0)
                    {
                        if ((flag = ReceiveData(socket, buffer, outTime)) < 0)
                        {
                            break;
                        }
                        fs.Write(buffer, 0, rcvLen);
                        leftLen -= rcvLen;
                        rcvLen = (maxBufferLength < leftLen) ? maxBufferLength : ((int)leftLen);
                    }
                }
                fs.Close();
            }
            catch (IOException e)
            {
                flag = -4;
            }
            return flag;
        }

        public static int ReceiveFile(Socket socket, string fileName, long fileLength)
        {
            return ReceiveFile(socket, fileName, fileLength, 2048, 1);
        }
    }
}
