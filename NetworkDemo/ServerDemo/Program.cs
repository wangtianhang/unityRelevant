using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Init("127.0.0.1", 13000);

            DateTime lastTime = DateTime.Now;
            while (true)
            {
                TimeSpan span = DateTime.Now - lastTime;
                server.OnUpdate(span.Milliseconds);
                Thread.Sleep(30);
                lastTime = DateTime.Now;
            }

            Console.Read();
        }
    }
}
