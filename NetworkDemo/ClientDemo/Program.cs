using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

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

            messageClient.ReceiveMessage((int)MessageId.LoginS2C, LoginCallback);
            MessageLoginS2C login = new MessageLoginS2C();
            login.m_content = "login";
            Stream stream = MessageLoginS2C.Serilization(login);
            messageClient.SendMessage((int)MessageId.Login, stream);

            Console.Read();
        }

        static void ConnectCallback(SocketError error)
        {
            Console.WriteLine("ConnectCallback " + error);
        }

        static void LoginCallback(int msgId, Stream data)
        {
            MessageLoginS2C s2c = MessageLoginS2C.Deserilization(data);
            Console.WriteLine("client login " + s2c.m_content);
        }
    }
}
