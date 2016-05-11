using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace UDPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //string ip = "127.0.0.1";
            //int port = 10001;
            //EndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 10001);
            EndPoint remoteEP = (EndPoint)sender;

            socket.Bind(remoteEP);

            byte[] receiveBuff = new byte[100];
            socket.ReceiveFrom(receiveBuff, ref remoteEP);

            Console.WriteLine("Server Receive ");

            byte[] sendBuff = new byte[100];
            socket.SendTo(sendBuff, remoteEP);

            Console.WriteLine("Server Send ");

            Console.Read();
        }
    }
}
