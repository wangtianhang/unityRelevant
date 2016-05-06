using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageClient messageClient = new MessageClient();
            messageClient.Connect("127.0.0.1", 13000);
            //messageClient.ReceiveMessage((int)MessageId.ExchangeSeq, new Stream());
            messageClient.ReceiveMessage((int)MessageId.LoginS2C, LoginCallback);
            //messageClient.SendMessage((int)MessageId.Login, );
            Console.Read();
        }

        static void LoginCallback(int msgId, Stream data)
        {
            Console.WriteLine("client login");
        }
    }
}
