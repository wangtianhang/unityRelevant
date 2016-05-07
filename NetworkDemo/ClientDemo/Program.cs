using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageClient messageClient = new MessageClient();
            messageClient.Connect("127.0.0.1", 13000, ConnectCallback);

            UIListenr uiListener = new UIListenr();
            messageClient.m_sendMsgEvent += uiListener.SendingCallback;
            messageClient.m_receiveMsgEvent += uiListener.ReceiveCallback;

            messageClient.RegisterReceiveMessage((int)MessageId.LoginS2C, LoginCallback);
            MessageLogin login = new MessageLogin();
            login.m_clientId = 1;
            MemoryStream stream = MessageLogin.Serilization(login);
            messageClient.SendMessage((int)MessageId.Login, stream);

            DateTime lastTime = DateTime.Now;
            while(true)
            {
                TimeSpan span = DateTime.Now - lastTime;
                messageClient.OnUpdate(span.Milliseconds);
                Thread.Sleep(30);
                lastTime = DateTime.Now;
            }

            Console.Read();
        }

        static void ConnectCallback(SocketError error)
        {
            Console.WriteLine("ConnectCallback " + error);
        }

        static void LoginCallback(int msgId, MemoryStream data)
        {
            //MemoryStream stream = data as MemoryStream;
            MessageLoginS2C s2c = MessageLoginS2C.Deserilization(data);
            Console.WriteLine("client login " + s2c.m_content);
        }
    }
}
